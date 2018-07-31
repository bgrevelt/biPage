using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.FrontEnd.CPP
{
    public class CodeGenerator
    {
        private const uint tab_to_spaces = 4;
        private List<Model.Enumeration> Enumerations;
        private List<Model.Structure> Structures;
        System.IO.StreamWriter Writer;

        // TODO: in practice we would not get a write here because we would want to be able to write to multiple files

        public CodeGenerator(List<Model.Enumeration> enumerations, List<Model.Structure> structures, System.IO.StreamWriter writer)
        {
            this.Enumerations = enumerations;
            this.Structures = structures;
            this.Writer = writer;
        }

        public void Generate()
        {
            // TODO: this will have to get more sophisticated. Each strcuture should get specific files and we must make sure to include the right headers for structures that require other types
            // and also include them in the right order            

            foreach (var e in Enumerations)
                GenerateEnum(e, 0);

            foreach (var s in Structures)
                GenerateStructure(s, 0);           
        }

        private void AnalyzeFields(Model.Structure structure, out List<Model.Field> static_fields, out List<Model.Field> dynamic_fields)
        {
            static_fields = new List<Model.Field>();
            dynamic_fields = new List<Model.Field>();

            foreach (var field in structure.Fields)
            {

                if (field.HasStaticSize())
                {
                    static_fields.Add(field);                    
                }               
                else
                {
                    static_fields.Add(field);
                }
            }
        }

        private void GenerateEnum(Model.Enumeration e, uint indent)
        {
            write_indented(indent, String.Format("enum class {0} : {1}", e.Name, type_to_cpp_type((dynamic)e.Type)));
            write_indented(indent, "{");
            for(int i = 0; i < e.Enumerators.Count; ++i)
            {
                var enumerator = e.Enumerators[i];
                write_indented(indent + 1, String.Format("{0} = {1}{2}", enumerator.Name, enumerator.Value, i != e.Enumerators.Count-1 ? "," : ""));
            }
            write_indented(indent, "};");
        }

        private void GenerateStructure(Model.Structure s, uint indent)
        {
            List<Model.Field> static_fields;
            List<Model.Field> dynamic_fields;
            AnalyzeFields(s, out static_fields, out dynamic_fields);
            bool dynamic_size = dynamic_fields.Count > 0; 

            write_indented(indent, String.Format("class {0}", s.Name));
            write_indented(indent, "{");
            write_indented(indent, "public:");
            if(!dynamic_size)
            {
                WriteDeletedConstructor(s.Name, indent + 1);
                WriteDeletedCopyConstructor(s.Name,indent + 1);
                WriteDeletedAssignmentOperator(s.Name, indent + 1);
                foreach (var field in s.Fields)
                    GenerateField(field, indent + 1);
            }
            else
            {
                WriteConstructor(s.Name, indent + 1, dynamic_fields);
            }
           
            
            if (dynamic_size)
            {
                write_indented(indent, "private:");
                // TODO: Add milepost members
            }
            write_indented(indent, "};");
        }

        public void WriteConstructor(String name, uint indent, List<Model.Field> dynamic_fields)
        {
            // The ctor for a variable sized field takes all variable sized fields as ctor arguments.
            var ctor_string = $"{name}(";
            for(int i = 0; i < dynamic_fields.Count; ++i)
            {
                var field = dynamic_fields[i];
                ctor_string += $"const {type_to_cpp_type((dynamic)field.Type)}& {field.Name}";
                if (i < dynamic_fields.Count - 1)
                    ctor_string += ", ";
            }
            ctor_string += ")";
            write_indented(indent, ctor_string);
            write_indented(indent, "{");
            write_indented(indent +1, "// We need a body here");
            write_indented(indent, "}");
        }


        public void WriteDeletedConstructor(String name, uint indent)
        {
            write_indented(indent, $"{name}() = delete");
        }
        public void WriteDeletedCopyConstructor(String name, uint indent)
        {
            write_indented(indent, $"{name}(const {name}&) = delete");
        }
        public void WriteDeletedAssignmentOperator(String name, uint indent)
        {
            write_indented(indent, $"{name}& operator=(const {name}&) = delete");
        }

        private void GenerateField(Model.Field field, uint indent)
        {
            FieldGetterGenerator getterGenerator = new FieldGetterGenerator(field);

            write_indented(indent, getterGenerator.GetDeclaration());
            write_indented(indent, "{");
            write_indented(indent + 1, String.Join(Environment.NewLine, getterGenerator.GetBody()));           
            write_indented(indent, "}");            
        }

        private uint GetFieldByteOffset(Model.Field field)
        {
            return field.Offset - (field.Offset % 8);
        }

        private uint GetCaptureSize(Model.Field field)
        {
            var byte_aligned_offset = GetFieldByteOffset(field);
            var minimum_capture_size = field.Offset - byte_aligned_offset + field.SizeInBits();
            var capture_type_width = (uint)Math.Max(Math.Pow(2, Math.Ceiling(Math.Log(minimum_capture_size) / Math.Log(2))), 8);
            return capture_type_width;
        }

        private String type_to_cpp_type(Model.FieldTypes.SignedIntegral s)
        {
            var size = (uint)Math.Max(8, Math.Pow(2, Math.Ceiling(Math.Log(s.size) / Math.Log(2))));
            Debug.Assert(size % 8 == 0);
            return String.Format("std::int{0}_t", size);
        }

        private String type_to_cpp_type(Model.FieldTypes.UnsignedIntegral u)
        {
            var size = (uint)Math.Max(8, Math.Pow(2, Math.Ceiling(Math.Log(u.size) / Math.Log(2))));
            Debug.Assert(size % 8 == 0);
            return String.Format("std::uint{0}_t", size);
        }

        private String type_to_cpp_type(Model.FieldTypes.AsciiString s)
        {
            // TODO: 
            return "ASCIISTring";
        }

        private String type_to_cpp_type(Model.FieldTypes.Collection c)
        {
            // TODO:
            return "Collection";
        }

        private String type_to_cpp_type(Model.FieldTypes.Boolean b)
        {
            return "bool";
        }

        private String type_to_cpp_type(Model.FieldTypes.FloatingPoint f)
        {
            Debug.Assert(f.Size == 32 || f.Size == 64);

            if (f.Size == 32)
                return "float";
            else
                return "double";
        }

        private String type_to_cpp_type(Model.Structure s)
        {
            return s.Name;
        }

        private String type_to_cpp_type(Model.Enumeration e)
        {
            return e.Name;
        }



        private void write_indented(uint indent, String text)
        {
            Writer.WriteLine(String.Format("{0}{1}", new String(' ', (int)(indent * tab_to_spaces)), text));
        }
    }
}

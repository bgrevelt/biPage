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

                if (field.Type is Model.FieldTypes.Reference)
                {
                    // TODO: we somehow need to find out if the type it is referencing is dynamic or not...
                }
                else if (field.Type is Model.FieldTypes.DynamicField) // TODO: I think we've used an oversimplification here. Collections and strings are not necessarilly dynamic. Only if the size expression cannot be resolved at compile time....
                {
                    dynamic_fields.Add(field);
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
            bool dynamic_size = dynamic_fields.Count > 0; // TODO: this is important. We need to know if the structure contains dynamic elements now because the ctor will be different!

            write_indented(indent, String.Format("class {0}", s.Name));
            write_indented(indent, "{");
            write_indented(indent, "public:");
            if(!dynamic_size)
            {
                WriteDeletedConstructor(s.Name, indent + 1);
                WriteDeletedCopyConstructor(s.Name,indent + 1);
                WriteDeletedAssignmentOperator(s.Name, indent + 1);
            }
           
            foreach (var field in s.Fields)
                GenerateField(field);
            if (dynamic_size)
            {
                write_indented(indent, "private:");
                // TODO: Add milepost members
            }
            write_indented(indent, "};");
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

        private void GenerateDynamicSizeStructure(Model.Structure s, uint indent)
        {
            write_indented(indent, String.Format("class {0}", s.Name));
            write_indented(indent, "{");
            foreach (var field in s.Fields)
                GenerateField(field);
            write_indented(indent, "};");
        }

        private void GenerateField(Model.Field field)
        {
            // const& <field_type> name() const { return reinterpret_cast<
        }

        private String type_to_cpp_type(Model.FieldTypes.SignedIntegral s)
        {
            // An enumeration cannot have a size that is not a multiple of 8, so round up
            var size = ((int)Math.Ceiling((decimal)s.size / 8.0M)) * 8;
            Debug.Assert(size % 8 == 0);
            return String.Format("std::int{0}_t", size);
        }

        private String type_to_cpp_type(Model.FieldTypes.UnsignedIntegral u)
        {
            // An enumeration cannot have a size that is not a multiple of 8, so round up
            var size = ((int)Math.Ceiling((decimal)u.size / 8.0M)) * 8;
            Debug.Assert(size % 8 == 0);
            return String.Format("std::uint{0}_t", size);
        }

        private void write_indented(uint indent, String text)
        {
            Writer.WriteLine(String.Format("{0}{1}", new String(' ', (int)(indent * tab_to_spaces)), text));
        }
    }
}

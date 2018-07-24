using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiPaGe.Model;
using BiPaGe.Model.FieldTypes;

namespace BiPaGe.FrontEnd.CPP
{ 
    class FieldGetterGenerator : Model.IFieldTypeVisitor
    {
        private String declaration = null;
        private String body = null;
        private Model.Field field;
        public String GetDeclaration(Model.Field field)
        {
            this.field = field;
            field.Type.Accept(this);
            return declaration;
        }

        public String GetBody(Model.Field field)
        {
            this.field = field;
            field.Type.Accept(this);
            return body;
        }

        public void CreateIntegralDeclaration(String typeTemplate)
        {
            var return_type = String.Format(typeTemplate, toStandardSize(field.SizeInBits()));
            var byte_algined_offset = GetFieldByteOffset(field.Offset);
            var capture_size = GetCaptureSize(field.Offset, byte_algined_offset, field.SizeInBits());
            
            var shift = GetShift(field.Offset, byte_algined_offset);

            bool needs_mask = (byte_algined_offset != field.Offset) || (capture_size != field.SizeInBits());
            bool needs_shift = shift != 0;

            if (needs_mask || needs_shift)
            {
                // Return by value
                declaration =  $"{return_type} {field.Name}() const";
            }
            else
            {
                // Return by reference
                declaration = $"const {return_type}& {field.Name}() const";
            }
        }

        public void CreateSignedIntegerDeclaration()
        {
            CreateIntegralDeclaration("std::int{0}_t");
        }

        public void CreateUnsignedIntegerDeclaration()
        {
            CreateIntegralDeclaration("std::uint{0}_t");
        }

        public void CreateIntegralBody(String typeTemplate)
        {
            var return_type = String.Format(typeTemplate, toStandardSize(field.SizeInBits()));
            var byte_algined_offset = GetFieldByteOffset(field.Offset);
            bool isByteAlgined = field.Offset % 8 == 0;
            bool isStandardSize = toStandardSize(field.SizeInBits()) == field.SizeInBits();

            var body = field.OffsetFrom != null ? field.OffsetFrom + "().end()" : "reinterpret_cast<const std::uint8_t*>(this)";
            if (byte_algined_offset > 0)
                body += $" + {byte_algined_offset / 8}";

            if (isByteAlgined && isStandardSize)
            {
                // This is the easiest case. We just have to create a getter that returns a pointer at the offset of the right type
                body = $"*reinterpret_cast<const {return_type}*>({body})";
            }
            else
            {
                var capture_size = GetCaptureSize(field.Offset, byte_algined_offset, field.SizeInBits());
                var capture_type = String.Format(typeTemplate, capture_size);
                var mask = GetMask(field.Offset, field.SizeInBits(), byte_algined_offset);
                var shift = GetShift(field.Offset, byte_algined_offset);
                bool needs_mask = (byte_algined_offset != field.Offset) || (capture_size != field.SizeInBits());
                bool needs_shift = shift != 0;
                bool needs_type_cast = capture_type != return_type;

                var offset_from = field.OffsetFrom != null ? field.OffsetFrom + "().end()" : "reinterpret_cast<const std::uint8_t*>(this)";
                var offset = field.Offset == 0 ? "" : $" + {field.Offset}";

                body = $"*reinterpret_cast<const {capture_type}*>({body})";
                // var body = $"*(reinterpret_cast<const {capture_type}*>({offset_from} + {byte_algined_offset / 8}))";
                if (needs_mask)
                    body = $"({body} & 0x{mask.ToString("x")})";
                if (needs_shift)
                    body = $"({body} >> {shift})";
                if (needs_type_cast)
                    body = $"static_cast<{return_type}>({body})";
            }



            this.body = $"return {body};";
        }

        public void CreateSignedIntegerBody()
        {
            CreateIntegralBody("std::int{0}_t");
        }

        public void CreateUnsignedIntegerBody()
        {
            CreateIntegralBody("std::uint{0}_t");
        }

        private uint GetFieldByteOffset(uint offset)
        {
            return offset - (offset % 8);
        }

        private uint GetCaptureSize(uint offset, uint byte_aligned_offset, uint size)
        {
            var minimum_capture_size = offset - byte_aligned_offset + size;
            var capture_type_width = (uint)Math.Max(Math.Pow(2, Math.Ceiling(Math.Log(minimum_capture_size) / Math.Log(2))), 8);
            return capture_type_width;
        }

        private String GetCaptureType(Model.Field field)
        {
            var byte_aligned_offset = GetFieldByteOffset(field.Offset);
            var minimum_capture_size = field.Offset - byte_aligned_offset + field.SizeInBits();
            var capture_type_width = (uint)Math.Max(Math.Pow(2, Math.Ceiling(Math.Log(minimum_capture_size) / Math.Log(2))), 8);
            return String.Format("std::uint{0}_t", capture_type_width);
        }

        private ulong GetMask(uint offset, uint size, uint byte_aligned_offset)
        {
            var mask = (ulong)Math.Pow(2, size) - 1;
            return mask << (int)(offset - byte_aligned_offset);
        }

        private uint GetShift(uint offset, uint byte_aligned_offset)
        {           
            return offset - byte_aligned_offset;
        }

        private uint toStandardSize(uint fieldSize)
        {
            var size = (uint)Math.Max(8, Math.Pow(2, Math.Ceiling(Math.Log(fieldSize) / Math.Log(2))));
            Debug.Assert(size % 8 == 0);
            return size;
        }

        private String type_to_cpp_type(Model.FieldTypes.SignedIntegral s, uint? overrule_size = null)
        {
            uint field_size = overrule_size ?? s.size;
            var size = (uint)Math.Max(8, Math.Pow(2, Math.Ceiling(Math.Log(field_size) / Math.Log(2))));
            Debug.Assert(size % 8 == 0);
            return String.Format("std::int{0}_t", size);
        }

        private String type_to_cpp_type(Model.FieldTypes.UnsignedIntegral u, uint? overrule_size = null)
        {
            uint field_size = overrule_size ?? u.size;
            var size = (uint)Math.Max(8, Math.Pow(2, Math.Ceiling(Math.Log(field_size) / Math.Log(2))));
            Debug.Assert(size % 8 == 0);
            return String.Format("std::uint{0}_t", size);
        }

        private String type_to_cpp_type(Model.FieldTypes.AsciiString s, uint? overrule_size = null)
        {
            // TODO: 
            return "ASCIISTring";
        }

        private String type_to_cpp_type(Model.FieldTypes.Collection c, uint? overrule_size = null)
        {
            // TODO:
            return "Collection";
        }

        private String type_to_cpp_type(Model.FieldTypes.Boolean b, uint? overrule_size = null)
        {
            return "bool";
        }

        private String type_to_cpp_type(Model.FieldTypes.FloatingPoint f, uint? overrule_size = null)
        {
            Debug.Assert(f.Size == 32 || f.Size == 64);

            if (f.Size == 32)
                return "float";
            else
                return "double";
        }

        private String type_to_cpp_type(Model.Structure s, uint? overrule_size = null)
        {
            return s.Name;
        }

        private String type_to_cpp_type(Model.Enumeration e, uint? overrule_size = null)
        {
            return e.Name;
        }

        public void Visit(AsciiString s)
        {
            throw new NotImplementedException();
        }

        public void Visit(Model.FieldTypes.Boolean b)
        {
            throw new NotImplementedException();
        }

        public void Visit(Collection c)
        {
            throw new NotImplementedException();
        }

        public void Visit(FloatingPoint f)
        {
            throw new NotImplementedException();
        }

        public void Visit(SignedIntegral s)
        {
            CreateSignedIntegerDeclaration();
            CreateSignedIntegerBody();
        }

        public void Visit(UnsignedIntegral u)
        {
            CreateUnsignedIntegerDeclaration();
            CreateUnsignedIntegerBody();
        }

        public void Visit(Structure s)
        {
            throw new NotImplementedException();
        }

        public void Visit(Enumeration e)
        {
            throw new NotImplementedException();
        }
    }
}

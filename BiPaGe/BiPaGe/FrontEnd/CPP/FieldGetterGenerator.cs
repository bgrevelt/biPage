using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.FrontEnd.CPP
{ 
    class FieldGetterGenerator
    {
        public String GetDeclaration(Model.Field field)
        {
            var capture_size = GetCaptureSize(field);
            var byte_algined_offset = GetFieldByteOffset(field);
            var shift = GetShift(field);

            bool needs_mask = (byte_algined_offset != field.Offset) || (capture_size != field.SizeInBits());
            bool needs_shift = shift != 0;

            if (needs_mask || needs_shift)
            {
                // Return by value
                return $"{type_to_cpp_type((dynamic)field.Type)} {field.Name}() const";
            }
            else
            {
                // Return by reference
                return $"const {type_to_cpp_type((dynamic)field.Type)}& {field.Name}() const";
            }
            
        }

        public String GetBody(Model.Field field)
        {
            var capture_size = GetCaptureSize(field);
            var capture_type = type_to_cpp_type((dynamic)field.Type, capture_size);
            var byte_algined_offset = GetFieldByteOffset(field);
            var mask = GetMask(field);
            var shift = GetShift(field);
            var return_type = type_to_cpp_type((dynamic)field.Type);

           
            bool needs_mask = (byte_algined_offset != field.Offset) || (capture_size != field.SizeInBits());
            bool needs_shift = shift != 0;
            bool needs_type_cast = capture_type != return_type;

            var offset_from = field.OffsetFrom != null ? field.OffsetFrom + "()" : "reinterpret_cast<const std::uint8_t*>(this)";
            var offset = field.Offset == 0 ? "" : $" + {field.Offset}";

            var body = $"*(reinterpret_cast<const {capture_type}*>({offset_from} + {byte_algined_offset / 8}))";
            if (needs_mask)
                body = $"({body} & 0x{mask.ToString("x")})";
            if (needs_shift)
                body = $"({body} >> {shift})";
            if (needs_type_cast)
                body = $"static_cast<{return_type}>({body})";

            return $"return {body};";
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

        private String GetCaptureType(Model.Field field)
        {
            var byte_aligned_offset = GetFieldByteOffset(field);
            var minimum_capture_size = field.Offset - byte_aligned_offset + field.SizeInBits();
            var capture_type_width = (uint)Math.Max(Math.Pow(2, Math.Ceiling(Math.Log(minimum_capture_size) / Math.Log(2))), 8);
            return String.Format("std::uint{0}_t", capture_type_width);
        }

        private ulong GetMask(Model.Field field)
        {
            var byte_aligned_offset = GetFieldByteOffset(field);
            var mask = (ulong)Math.Pow(2, field.SizeInBits()) - 1;
            return mask << (int)(field.Offset - byte_aligned_offset);
        }

        private uint GetShift(Model.Field field)
        {
            var byte_aligned_offset = GetFieldByteOffset(field);
            return field.Offset - byte_aligned_offset;
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
    }
}

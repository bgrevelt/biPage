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
        private List<String> declaration = new List<string>();
        private List<String> body = new List<string>();
        private Model.Field field;
        private readonly FieldTypeConverter fieleTypeConverter = new FieldTypeConverter();

        public FieldGetterGenerator(Model.Field field)
        {
            this.field = field;
            field.Type.Accept(this);
            Debug.Assert(this.body != null);
            Debug.Assert(this.declaration != null);
        }

        public String GetDeclaration()
        {
            return String.Join("\n", declaration.ToArray());
        }

        public List<String> GetBody()
        {
            return this.body;
        }

        public void CreateEnumerationDeclaration()
        {
            String return_type = fieleTypeConverter.Convert(this.field.Type);
            var byte_algined_offset = GetFieldByteOffset(field.Offset);
            var capture_size = GetCaptureSize(field.Offset, byte_algined_offset, field.SizeInBits());

            var shift = GetShift(field.Offset, byte_algined_offset);

            bool needs_mask = (byte_algined_offset != field.Offset) || (capture_size != field.SizeInBits());
            bool needs_shift = shift != 0;

            if (needs_mask || needs_shift)
                declaration.Add($"{return_type} {field.Name}() const");
            else
                declaration.Add($"const {return_type}& {field.Name}() const");
        }

        public void CreateEnumerationBody()
        {
            String return_type = fieleTypeConverter.Convert(this.field.Type);

            var byte_algined_offset = GetFieldByteOffset(field.Offset);

            var offset = field.OffsetFrom != null ? field.OffsetFrom + "().end()" : "reinterpret_cast<const std::uint8_t*>(this)";
            if (byte_algined_offset > 0)
                offset += $" + {byte_algined_offset / 8}";

            body.Add($"const std::uint8_t* data_offset = {offset};");
            String to_return = "*data_offset";

            var capture_size = GetCaptureSize(field.Offset, byte_algined_offset, field.SizeInBits());
            if (byte_algined_offset == this.field.Offset && capture_size == this.field.SizeInBits())
            {
                // We can simply cast directly to the return type
                body.Add($"const {return_type}* captured_data = reinterpret_cast<const {return_type}*>(data_offset);");
                to_return = "*captured_data";
            }
            else
            {                
                var capture_type = fieleTypeConverter.Convert((this.field.Type as Model.Enumeration).Type, capture_size);
                bool needs_capture = capture_type != "std::uint8_t";
                if (needs_capture)
                {

                    body.Add($"const {capture_type}* captured_data = reinterpret_cast<const {capture_type}*>(data_offset);");
                    to_return = "*captured_data";
                }

                // If the data we need is not byte algined or not one of C++'s standard data types, we will need to mask out the bits that we don't need
                // If we do that we will also have to return by value instead of by reference. 
                bool needs_mask = (byte_algined_offset != field.Offset) || (capture_size != field.SizeInBits());
                var shift = GetShift(field.Offset, byte_algined_offset);
                if (needs_mask)
                {
                    var mask = GetMask(field.Offset, field.SizeInBits(), byte_algined_offset);
                    var temp = $"{capture_type} masked_data = (*captured_data & 0x{mask.ToString("x")})";
                    if (shift > 0)
                        temp = $"{temp} >> {shift};";
                    else
                        temp += ";";
                    body.Add(temp);
                    to_return = "masked_data";
                }

                body.Add($"{return_type} typed_data = static_cast<{return_type}>({to_return});");
                to_return = "typed_data";

                // And finally add a line that returns the data
               
            }
            body.Add($"return {to_return};");
            //if (byte_algined_offset == this.field.Offset)
            //{
            //    // We can simply cast directly to the return type
            //    body.Add($"const {return_type}* captured_data = reinterpret_cast<const {return_type}*>(data_offset);");
            //    to_return = "*captured_data";
            //}
            //else
            //{
            //    var capture_size = GetCaptureSize(field.Offset, byte_algined_offset, field.SizeInBits());
            //    var capture_type = String.Format("std::uint{0}_t", capture_size);
            //    var mask = GetMask(field.Offset, field.SizeInBits(), byte_algined_offset);
            //    var temp = $"{capture_type} masked_data = (*captured_data & 0x{mask.ToString("x")})";
            //    var shift = GetShift(field.Offset, byte_algined_offset);
            //    if (shift > 0)
            //        temp = $"{temp} >> {shift};";
            //    else
            //        temp += ";";
            //    body.Add(temp);
            //    to_return = "masked_data";
            //    body.Add($"const {capture_type}* typed_data = static_cast<{return_type }> (masked_data);");
            //    to_return = "*typed_data";
            //}

            //body.Add($"return {to_return};");
        }

        public void CreateFloatingPointDeclaration()
        {
            Debug.Assert(this.field.SizeInBits() == 32 || this.field.SizeInBits() == 64, "Only 32 bit and 64 bit floating point types are supported");
            String return_type = this.field.SizeInBits() == 32 ? "float" : "double";

            var byte_algined_offset = GetFieldByteOffset(field.Offset);
            var capture_size = GetCaptureSize(field.Offset, byte_algined_offset, field.SizeInBits());

            var shift = GetShift(field.Offset, byte_algined_offset);

            bool needs_mask = (byte_algined_offset != field.Offset) || (capture_size != field.SizeInBits());
            bool needs_shift = shift != 0;

            if (needs_mask || needs_shift)
            {
                // Return by value
                declaration.Add($"{return_type} {field.Name}() const");
            }
            else
            {
                // Return by reference
                declaration.Add($"const {return_type}& {field.Name}() const");
            }
        }

        public void CreateFloatingPointBody()
        {
            Debug.Assert(this.field.SizeInBits() == 32 || this.field.SizeInBits() == 64, "Only 32 bit and 64 bit floating point types are supported");
            String return_type = this.field.SizeInBits() == 32 ? "float" : "double";
            var byte_algined_offset = GetFieldByteOffset(field.Offset);

            // Determine offset to the data we need
            var offset = field.OffsetFrom != null ? field.OffsetFrom + "().end()" : "reinterpret_cast<const std::uint8_t*>(this)";
            if (byte_algined_offset > 0)
                offset += $" + {byte_algined_offset / 8}";

            body.Add($"const std::uint8_t* data_offset = {offset};");
            String to_return = "*data_offset";

            if (byte_algined_offset == this.field.Offset)
            {
                // We can simply cast directly to the return type
                body.Add($"const {return_type}* captured_data = reinterpret_cast<const {return_type}*>(data_offset);");
                to_return = "*captured_data";
            }
            else
            {
                var shift = GetShift(field.Offset, byte_algined_offset);
                var capture_size = GetCaptureSize(field.Offset, byte_algined_offset, field.SizeInBits());
                var capture_type = String.Format("std::uint{0}_t", capture_size);
                body.Add($"const {capture_type}* captured_data = reinterpret_cast<const {capture_type}*>(data_offset);");

                var mask = GetMask(field.Offset, field.SizeInBits(), byte_algined_offset);
                var temp = $"{capture_type} masked_data = (*captured_data & 0x{mask.ToString("x")})";
                if (shift > 0)
                    temp = $"{temp} >> {shift};";
                else
                    temp += ";";

                body.Add(temp);
                body.Add($"{return_type} typed_data = static_cast<{return_type}>(masked_data);");

                to_return = "typed_data";
            }

            //// And finally add a line that returns the data
            body.Add($"return {to_return};");
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
                declaration.Add($"{return_type} {field.Name}() const");
            }
            else
            {
                // Return by reference
                declaration.Add($"const {return_type}& {field.Name}() const");
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

        /*
        * return static_cast<std::int32_t>(((*reinterpret_cast<const std::int64_t*>(reinterpret_cast<const std::uint8_t*>(this) + 65) & 0x7fffffff8) >> 3));
        * 
        * auto data_offset = reinterpret_cast<const std::uint8_t*>(this) + 65;
        * auto captured_data = *reinterpret_cast<const std::int64_t*>(data_offset);
        * auto masked_data = (captured_data & 0x7fffffff8) >> 3;
        * return static_cast<std::int32_t>(masked_data);
        */

        public void CreateIntegralBody(String typeTemplate)
        {
            
            var byte_algined_offset = GetFieldByteOffset(field.Offset);

            // Determine offset to the data we need
            var offset = field.OffsetFrom != null ? field.OffsetFrom + "().end()" : "reinterpret_cast<const std::uint8_t*>(this)";
            if (byte_algined_offset > 0)
                offset += $" + {byte_algined_offset / 8}";
            
            body.Add($"const std::uint8_t* data_offset = {offset};");
            String to_return = "*data_offset";


            // Add a line to create a pointer to the encapsulating data type (if that's not an uint8)
            var capture_size = GetCaptureSize(field.Offset, byte_algined_offset, field.SizeInBits());
            var capture_type = String.Format(typeTemplate, capture_size);
            bool needs_capture = capture_type != "std::uint8_t";
            if(needs_capture)
            {
                
                body.Add($"const {capture_type}* captured_data = reinterpret_cast<const {capture_type}*>(data_offset);");
                to_return = "*captured_data";
            }

            // If the data we need is not byte algined or not one of C++'s standard data types, we will need to mask out the bits that we don't need
            // If we do that we will also have to return by value instead of by reference. 
            bool needs_mask = (byte_algined_offset != field.Offset) || (capture_size != field.SizeInBits());
            var shift = GetShift(field.Offset, byte_algined_offset);
            if(needs_mask)
            {
                var mask = GetMask(field.Offset, field.SizeInBits(), byte_algined_offset);
                var temp = $"{capture_type} masked_data = (*captured_data & 0x{mask.ToString("x")})";
                if (shift > 0)
                    temp = $"{temp} >> {shift};";
                else
                    temp += ";";
                body.Add(temp);
                to_return = "masked_data";
            }

            // In some cases our capture data is larger than the data type we want to return (for example when a standard type is not byte algined). We removed all the data we don't need in the masking and shifting step
            // so now cast back to the type we want to return
            var return_type = String.Format(typeTemplate, toStandardSize(field.SizeInBits()));
            bool needs_type_cast = capture_type != return_type;
            if(needs_type_cast)
            {
                body.Add($"{return_type} typed_data = static_cast<{return_type}>({to_return});");
                to_return = "typed_data";
            }

            // And finally add a line that returns the data
            body.Add($"return {to_return};");
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
            CreateFloatingPointDeclaration();
            CreateFloatingPointBody();
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
            CreateEnumerationDeclaration();
            CreateEnumerationBody();
        }
    }
}

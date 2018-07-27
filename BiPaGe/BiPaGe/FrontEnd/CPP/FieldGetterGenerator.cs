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

            String to_return = "*" + AddOffsetLine("data_offset", byte_algined_offset, field.OffsetFrom);            

            var capture_size = GetCaptureSize(field.Offset, byte_algined_offset, field.SizeInBits());
            if (byte_algined_offset == this.field.Offset && capture_size == this.field.SizeInBits())
            {
                // We can simply cast directly to the return type
                to_return = "*" + AddCaptureLine("captured_data", return_type);
            }
            else
            {                
                var capture_type = fieleTypeConverter.Convert((this.field.Type as Model.Enumeration).Type, capture_size);
                bool needs_capture = capture_type != "std::uint8_t";
                if (needs_capture)
                    to_return = "*" + AddCaptureLine("captured_data", capture_type);
             
                // If the data we need is not byte algined or not one of C++'s standard data types, we will need to mask out the bits that we don't need
                // If we do that we will also have to return by value instead of by reference. 
                bool needs_mask = (byte_algined_offset != field.Offset) || (capture_size != field.SizeInBits());
                var shift = GetShift(field.Offset, byte_algined_offset);
                if (needs_mask)
                    to_return = AddMaskLine("masked_data", capture_type, to_return, GetMask(field.Offset, field.SizeInBits(), byte_algined_offset), shift);                    

                to_return = AddStaticCast(to_return, "typed_data", return_type);               
            }
            body.Add($"return {to_return};");            
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
            String to_return = "*"+ AddOffsetLine("data_offset", byte_algined_offset, field.OffsetFrom);           

            if (byte_algined_offset == this.field.Offset)
            {
                to_return = "*" + AddCaptureLine("captured_data", return_type);                
            }
            else
            {
                var shift = GetShift(field.Offset, byte_algined_offset);
                var capture_size = GetCaptureSize(field.Offset, byte_algined_offset, field.SizeInBits());
                var capture_type = String.Format("std::uint{0}_t", capture_size);

                AddCaptureLine("captured_data", capture_type);

                var mask = GetMask(field.Offset, field.SizeInBits(), byte_algined_offset);
                to_return = AddMaskLine("masked_data", capture_type, "*captured_data", GetMask(field.Offset, field.SizeInBits(), byte_algined_offset), shift);
                to_return = AddStaticCast(to_return, "typed_data", return_type);
            }
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

        private String AddOffsetLine(String variable_name, uint byte_aligned_offset, String offset_from)
        {
            var offset = offset_from != null ? field.OffsetFrom + "().end()" : "reinterpret_cast<const std::uint8_t*>(this)";
            if (byte_aligned_offset > 0)
                offset += $" + {byte_aligned_offset / 8}";

            body.Add($"const std::uint8_t* {variable_name} = {offset};");
            return variable_name;
        }

        private String AddCaptureLine(String variable_name, String capture_type)
        {
            body.Add($"const {capture_type}* captured_data = reinterpret_cast<const {capture_type}*>(data_offset);");
            return variable_name;
        }

        private String AddMaskLine(String variable_name, String return_type, String variable_to_mask, ulong mask, uint shift)
        {
            var temp = $"{return_type} {variable_name} = ({variable_to_mask} & 0x{mask.ToString("x")})";
            if (shift > 0)
                temp = $"{temp} >> {shift};";
            else
                temp += ";";
            body.Add(temp);
            return variable_name;
        }

        private String AddStaticCast(String from, String to, String type)
        {
            body.Add($"{type} {to} = static_cast<{type}>({from});");
            return to;
        }

        public void CreateIntegralBody(String typeTemplate)
        {
            
            var byte_algined_offset = GetFieldByteOffset(field.Offset);

            String to_return = "*" + AddOffsetLine("data_offset", byte_algined_offset, field.OffsetFrom);            

            // Add a line to create a pointer to the encapsulating data type (if that's not an uint8)
            var capture_size = GetCaptureSize(field.Offset, byte_algined_offset, field.SizeInBits());
            var capture_type = String.Format(typeTemplate, capture_size);

            bool needs_capture = capture_type != "std::uint8_t";
            if(needs_capture)   
                to_return = "*" + AddCaptureLine("captured_data", capture_type);       

            // If the data we need is not byte algined or not one of C++'s standard data types, we will need to mask out the bits that we don't need
            // If we do that we will also have to return by value instead of by reference. 
            bool needs_mask = (byte_algined_offset != field.Offset) || (capture_size != field.SizeInBits());
            var shift = GetShift(field.Offset, byte_algined_offset);
            if(needs_mask)            
                to_return = AddMaskLine("masked_data", capture_type, to_return, GetMask(field.Offset, field.SizeInBits(), byte_algined_offset), shift);            

            // In some cases our capture data is larger than the data type we want to return (for example when a standard type is not byte algined). We removed all the data we don't need in the masking and shifting step
            // so now cast back to the type we want to return
            var return_type = String.Format(typeTemplate, toStandardSize(field.SizeInBits()));
            bool needs_type_cast = capture_type != return_type;
            if(needs_type_cast)
                to_return = AddStaticCast(to_return, "typed_data", return_type);

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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using BiPaGe.Model;
using BiPaGe.Model.FieldTypes;

namespace BiPaGe.FrontEnd.CPP
{
    class FieldGetterGenerator : Model.IFieldTypeVisitor
    {
        private List<String> declaration = new List<string>();
        private List<String> body = new List<string>();
        private Field field;

        public FieldGetterGenerator(Model.Field field)
        {
            this.field = new Field(field);
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

        private void CreateStringDeclaration()
        {
            // String are always returned by value. We could create an encapsulating type
            // that behaves like a string and cast to the raw data to spare an allocation
            // (like we do with collections) but since strings aren't used that much
            // I did not feel like spending the effort (yet)
            declaration.Add($"std::string {field.Name}() const");
        }

        private void CreateStringBody(AsciiString s)
        {
            String size = new ExpressionTranslator().Translate(s.Size);

            AddOffsetLine("data_offset", field.ByteAlginedOfffset, field.OffsetField);
            this.body.Add($"return std::string(static_cast<const char*>(data_offset), {size});");
        }

        private void CreateDeclaration()
        {
            String return_type = field.CppType;
            var capture_size = field.CaptureSize;

            bool needs_mask = (field.ByteAlginedOfffset != field.Offset) || (capture_size != field.Size);
            bool needs_shift = field.Shift != 0;

            if (needs_mask || needs_shift) // Return by value
                declaration.Add($"{return_type} {field.Name}() const");
            else // Return by reference
                declaration.Add($"const {return_type}& {field.Name}() const");
        }

        private void CreateBody()
        {
            var return_type = field.CppType;
            String to_return = "*" + AddOffsetLine("data_offset", field.ByteAlginedOfffset, field.OffsetField);

            bool needs_capture = field.CaptureType != "std::uint8_t";
            if (needs_capture)
                to_return = "*" + AddCaptureLine("captured_data", field.CaptureType);

            if (field.NeedsSignBitStuffing)
                AddSignBitLine("sign_bit", to_return, this.field.SignBitMask);
            
            // If the data we need is not byte algined or not one of C++'s standard data types, we will need to mask out the bits that we don't need
            // If we do that we will also have to return by value instead of by reference. 
            if (field.NeedsMasking)
                to_return = AddMaskLine("masked_data", field.CaptureType, to_return, field.Mask, field.Shift);

            if (field.NeedsSignBitStuffing)
                to_return = AddSignBitStuffingLine("signed_data", field.Size, field.CaptureSize, field.CaptureType);

            // In some cases our capture data is larger than the data type we want to return (for example when a standard type is not byte algined). We removed all the data we don't need in the masking and shifting step
            // so now cast back to the type we want to return

            bool needs_type_cast = field.CaptureType != return_type;
            if (needs_type_cast)
                to_return = AddStaticCast(to_return, "typed_data", return_type);

            body.Add($"return {to_return};");
        }

        private String AddOffsetLine(String variable_name, uint byte_aligned_offset, String offset_from)
        {
            var offset = offset_from != null ? field.OffsetField + "().end()" : "reinterpret_cast<const std::uint8_t*>(this)";
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

        private String AddSignBitLine(String variable_name, String data_field, ulong signBitMask)
        {
            body.Add($"bool {variable_name} = ({data_field} & 0x{signBitMask.ToString("x")}) == 0x{signBitMask.ToString("x")};");
            return variable_name;
        }

        private String AddSignBitStuffingLine(String variable_name, uint field_size, uint capture_size, String capture_type)
        {
            // Sign bit stuffing means that we have a signed data type that is shorter than the cpp data type. Which is the case for any non standard sized type.
            // For example, and int5 will be exposed as an int8. That means that there are 3 bits that need to be stuffed in. With an unsigned type, we can just leave
            // these at zero, but for a signed type, they will need to have the same value as the sign bit.

            // The mask that identifies the bits in the field that have data after shifting
            var shifted_data_mask = ((System.Numerics.BigInteger)1 << (int)field_size -1) - 1; 
            
            // To get to the bits that we need to set we start out with a mask the size of the data field with every bit set to '1'. Note that we use shifting rather then 
            // math.pow as that uses a double which introduces rounding errors for larger types.
            var sign_mask = ((System.Numerics.BigInteger)1 << (int)capture_size) - 1;
            // Now clear the bits on which we have data
            sign_mask &= ~shifted_data_mask;
            body.Add($"{capture_type} {variable_name} = masked_data | (sign_bit ? 0x{((ulong)sign_mask).ToString("x")} : 0);");
            return variable_name;
        }

        public void Visit(AsciiString s)
        {
            CreateStringDeclaration();
            CreateStringBody(s);
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
            CreateDeclaration();
            CreateBody();
        }

        public void Visit(SignedIntegral s)
        {
            CreateDeclaration();
            CreateBody();
        }

        public void Visit(UnsignedIntegral u)
        {
            CreateDeclaration();
            CreateBody();
        }

        public void Visit(Structure s)
        {
            throw new NotImplementedException();
        }

        public void Visit(Enumeration e)
        {
            CreateDeclaration();
            CreateBody();           
        }
    }
}

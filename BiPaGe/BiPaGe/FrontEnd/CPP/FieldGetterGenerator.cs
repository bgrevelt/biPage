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
    //class Field
    //{
    //    private Model.Field Original;
    //    public Field(Model.Field original)
    //    {
    //        this.Original = original;
    //        this.ByteAlginedOfffset = (original.Offset - (original.Offset % 8));
    //        this.Mask = ComputeMask();
    //        this.Shift = ComputeShift();
    //        this.CaptureSize = ComputeCaptureSize();
    //        this.CppType = buildCppType();
    //    }
    //    public String Name { get { return this.Original.Name; } }
    //    public uint Size { get { return this.Original.SizeInBits(); } }
    //    public uint Offset { get { return this.Original.Offset; } }
    //    public uint ByteAlginedOfffset { get; }
    //    public String OffsetField { get { return this.Original.OffsetFrom; } }
    //    public Model.FieldType Type { get { return this.Original.Type; } }
    //    public ulong Mask { get; }
    //    public uint Shift { get; }
    //    public uint CaptureSize { get; }
    //    public String CppType { get; }

    //    private ulong ComputeMask()
    //    {
    //        var mask = (ulong)Math.Pow(2, this.Size) - 1;
    //        return mask << (int)(this.Offset - this.ByteAlginedOfffset);
    //    }

    //    private uint ComputeShift()
    //    {
    //        return this.Offset - this.ByteAlginedOfffset;
    //    }

    //    private uint ComputeCaptureSize()
    //    {
    //        var minimum_capture_size = this.Offset - this.ByteAlginedOfffset + this.Size;
    //        var capture_type_width = (uint)Math.Max(Math.Pow(2, Math.Ceiling(Math.Log(minimum_capture_size) / Math.Log(2))), 8);
    //        return capture_type_width;
    //    }

    //    private String buildCppType()
    //    {
    //        var cpp_size = (uint)Math.Max(8, Math.Pow(2, Math.Ceiling(Math.Log(this.Size) / Math.Log(2))));
    //        Debug.Assert(cpp_size % 8 == 0);
    //        return new FieldTypeConverter().Convert(this.Type, cpp_size);            
    //    }
    //}


    class FieldGetterGenerator : Model.IFieldTypeVisitor
    {
        private List<String> declaration = new List<string>();
        private List<String> body = new List<string>();
        private Field field;
        private readonly FieldTypeConverter fieleTypeConverter = new FieldTypeConverter();

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

        public void CreateEnumerationDeclaration()
        {
            String return_type = fieleTypeConverter.Convert(this.field.Type);
            var capture_size = field.CaptureSize;

            bool needs_mask = (field.ByteAlginedOfffset != field.Offset) || (capture_size != field.Size);
            bool needs_shift = field.Shift != 0;

            if (needs_mask || needs_shift)
                declaration.Add($"{return_type} {field.Name}() const");
            else
                declaration.Add($"const {return_type}& {field.Name}() const");
        }

        public void CreateEnumerationBody()
        {
            String return_type = fieleTypeConverter.Convert(this.field.Type);
            String to_return = "*" + AddOffsetLine("data_offset", field.ByteAlginedOfffset, field.OffsetField);            

            var capture_size = field.CaptureSize;
            if (field.ByteAlginedOfffset == this.field.Offset && capture_size == this.field.Size)
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
                bool needs_mask = (field.ByteAlginedOfffset != field.Offset) || (capture_size != field.Size);
                if (needs_mask)
                    to_return = AddMaskLine("masked_data", capture_type, to_return, field.Mask, field.Shift);                    

                to_return = AddStaticCast(to_return, "typed_data", return_type);               
            }
            body.Add($"return {to_return};");            
        }

        public void CreateFloatingPointDeclaration()
        {
            Debug.Assert(this.field.Size == 32 || this.field.Size == 64, "Only 32 bit and 64 bit floating point types are supported");
            String return_type = this.field.Size == 32 ? "float" : "double";
            var capture_size = field.CaptureSize;

            bool needs_mask = (field.ByteAlginedOfffset != field.Offset) || (capture_size != field.Size);
            bool needs_shift = field.Shift != 0;

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
            Debug.Assert(this.field.Size == 32 || this.field.Size == 64, "Only 32 bit and 64 bit floating point types are supported");
            String return_type = this.field.Size == 32 ? "float" : "double";

            // Determine offset to the data we need
            String to_return = "*"+ AddOffsetLine("data_offset", field.ByteAlginedOfffset, field.OffsetField);           

            if (field.ByteAlginedOfffset == this.field.Offset)
            {
                to_return = "*" + AddCaptureLine("captured_data", return_type);                
            }
            else
            {
                var capture_size = field.CaptureSize;
                var capture_type = String.Format("std::uint{0}_t", capture_size);

                AddCaptureLine("captured_data", capture_type);

                to_return = AddMaskLine("masked_data", capture_type, "*captured_data", field.Mask, field.Shift);
                to_return = AddStaticCast(to_return, "typed_data", return_type);
            }
            body.Add($"return {to_return};");
        }

        public void CreateIntegralDeclaration(String typeTemplate)
        {
            var return_type = field.CppType;
            var capture_size = field.CaptureSize;

            bool needs_mask = (field.ByteAlginedOfffset != field.Offset) || (capture_size != field.Size);
            bool needs_shift = field.Shift != 0;

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

        public void CreateIntegralBody(String typeTemplate)
        {
            String to_return = "*" + AddOffsetLine("data_offset", field.ByteAlginedOfffset, field.OffsetField);            

            // Add a line to create a pointer to the encapsulating data type (if that's not an uint8)
            var capture_size = field.CaptureSize;
            var capture_type = String.Format(typeTemplate, capture_size);

            bool needs_capture = capture_type != "std::uint8_t";
            if(needs_capture)   
                to_return = "*" + AddCaptureLine("captured_data", capture_type);       

            // If the data we need is not byte algined or not one of C++'s standard data types, we will need to mask out the bits that we don't need
            // If we do that we will also have to return by value instead of by reference. 
            bool needs_mask = (field.ByteAlginedOfffset != field.Offset) || (capture_size != field.Size);
            if(needs_mask)            
                to_return = AddMaskLine("masked_data", capture_type, to_return, field.Mask, field.Shift);

            // In some cases our capture data is larger than the data type we want to return (for example when a standard type is not byte algined). We removed all the data we don't need in the masking and shifting step
            // so now cast back to the type we want to return
            var return_type = field.CppType;
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

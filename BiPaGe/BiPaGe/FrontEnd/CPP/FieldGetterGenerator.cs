﻿using System;
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
        private readonly FieldTypeConverter fieleTypeConverter = new FieldTypeConverter();
        private String to_return = null; 

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
            to_return = "*" + AddOffsetLine("data_offset", field.ByteAlginedOfffset, field.OffsetField);

            bool needs_capture = field.CaptureType != "std::uint8_t";
            if (needs_capture)
                to_return = "*" + AddCaptureLine("captured_data", field.CaptureType);

            bool isSized = this.field.Type is Model.FieldTypes.SignedIntegral ||
                (this.field.Type is Model.Enumeration && (this.field.Type as Enumeration).Type is Model.FieldTypes.SignedIntegral);
            bool hasNonStandardSize = Math.Abs((Math.Log(field.Size) / Math.Log(2)) % 1) >= (double.Epsilon * 100);

            ulong mask = field.Mask;

            if (isSized && hasNonStandardSize)
            {
                var sign_bit_mask = ((ulong)Math.Pow(2, field.Size + (field.Offset - field.ByteAlginedOfffset) - 1));
                body.Add($"bool sign_bit = ({to_return} & 0x{sign_bit_mask.ToString("x")}) == 0x{sign_bit_mask.ToString("x")};");

                // Remove the sign bit from the mask
                mask &= (~sign_bit_mask);

            }

            // If the data we need is not byte algined or not one of C++'s standard data types, we will need to mask out the bits that we don't need
            // If we do that we will also have to return by value instead of by reference. 
            if (field.NeedsMasking)
                to_return = AddMaskLine("masked_data", field.CaptureType, to_return, mask, field.Shift);

            if (isSized && hasNonStandardSize)
            {
                mask = (ulong)Math.Pow(2, field.Size - 1) - 1;
                var sign_mask = (((System.Numerics.BigInteger)1 << (int)field.CaptureSize) - 1) & (~mask);
                body.Add($"{field.CaptureType} signed_data = masked_data | (sign_bit ? 0x{((ulong)sign_mask).ToString("x")} : 0);");
                to_return = "signed_data";
            }


            // In some cases our capture data is larger than the data type we want to return (for example when a standard type is not byte algined). We removed all the data we don't need in the masking and shifting step
            // so now cast back to the type we want to return

            bool needs_type_cast = field.CaptureType != return_type;
            if (needs_type_cast)
                to_return = AddStaticCast(to_return, "typed_data", return_type);
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
            CreateDeclaration();
            CreateBody();
            body.Add($"return {to_return};");
        }

        public void Visit(SignedIntegral s)
        {
            CreateDeclaration();
            CreateBody();
            body.Add($"return {to_return};");
        }

        public void Visit(UnsignedIntegral u)
        {
            CreateDeclaration();
            CreateBody();
            body.Add($"return {to_return};");
        }

        public void Visit(Structure s)
        {
            throw new NotImplementedException();
        }

        public void Visit(Enumeration e)
        {
            CreateDeclaration();
            CreateBody();
            body.Add($"return {to_return};");
        }
    }
}

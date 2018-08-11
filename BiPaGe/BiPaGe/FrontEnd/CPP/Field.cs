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
    public class Field : Model.IFieldTypeVisitor
    {
        public String Name { get { return this.Original.Name; } }
        public uint Size { get { return this.Original.SizeInBits(); } }
        public uint Offset { get { return this.Original.Offset; } }
        public uint ByteAlginedOfffset { get; }
        public String OffsetField { get { return this.Original.OffsetFrom; } }
        public Model.FieldType Type { get { return this.Original.Type; } }
        public ulong Mask { get; private set; }
        public ulong SignBitMask { get; private set; }
        public uint Shift { get; }
        public uint CaptureSize { get; }
        public String CppType { get; private set; }
        public String CaptureType { get; private set; }
        public bool NeedsMasking { get; }     
        public bool NeedsSignBitStuffing { get; private set; }

        public Model.Field Original; // TODO: I'm not sure this should be public
        public Field(Model.Field original)
        {
            // Set generic properties
            this.Original = original;
            this.ByteAlginedOfffset = (original.Offset - (original.Offset % 8));
            if (original.Type is Model.FieldTypes.Integral ||
                original.Type is Model.FieldTypes.FloatingPoint ||
                original.Type is Model.Enumeration)
            {

                this.Mask = ComputeMask();
                this.Shift = ComputeShift();
                this.CaptureSize = ComputeCaptureSize();
                this.NeedsMasking = ComputeMaskingRequired();
            }
            this.NeedsSignBitStuffing = false; // Default to false, will be set in the type visitor if required
            // Set type dependent properties
            this.Original.Type.Accept(this);
        }       

        private ulong ComputeMask()
        {
            // Note: do not use Math.pow, it will start rounding when the size is too large!
            var mask = (((System.Numerics.BigInteger)1 << (int)this.Size) - 1) << (int)(this.Offset - this.ByteAlginedOfffset);
            Debug.Assert(mask <= ulong.MaxValue);
            return (ulong)mask;
        }

        private uint ComputeShift()
        {
            return this.Offset - this.ByteAlginedOfffset;
        }

        private uint ComputeCaptureSize()
        {
            var minimum_capture_size = this.Offset - this.ByteAlginedOfffset + this.Size;
            var capture_type_width = (uint)Math.Max(Math.Pow(2, Math.Ceiling(Math.Log(minimum_capture_size) / Math.Log(2))), 8);
            return capture_type_width;
        }

        private bool ComputeMaskingRequired()
        {
            return this.CaptureSize != this.Size;
        }

        public void Visit(AsciiString s)
        {
            this.CaptureType = "std::string";
            this.CppType = "std::string";
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
            Debug.Assert(this.Size == 32 || this.Size == 64);
            if(this.Size == 32)
            {
                this.CppType = "float";
                this.CaptureType = "float";
            }
            else
            {
                this.CppType = "double";
                this.CaptureType = "double";
            }

            // Floating point types are captured as uints if we need to perofrm bit manipulation on them (e.g. masking, shifting)
            if (this.NeedsMasking)
                this.CaptureType = String.Format("std::uint{0}_t", this.CaptureSize);
        }

        public void Visit(SignedIntegral s)
        {
            var cPP_type_size = (uint)Math.Max(8, Math.Pow(2, Math.Ceiling(Math.Log(this.Size) / Math.Log(2))));
            Debug.Assert(cPP_type_size % 8 == 0);
            this.CppType = String.Format("std::int{0}_t", cPP_type_size);
            this.CaptureType = String.Format("std::int{0}_t", this.CaptureSize);

            this.SignBitMask = ((ulong)Math.Pow(2, this.Size + (this.Offset - this.ByteAlginedOfffset) - 1));

            bool hasNonStandardSize = Math.Abs((Math.Log(this.Size) / Math.Log(2)) % 1) >= (double.Epsilon * 100);
            if(hasNonStandardSize)
            {
                this.Mask &= (~this.SignBitMask);
                this.NeedsSignBitStuffing = true;
            }
        }

        public void Visit(UnsignedIntegral u)
        {
            var cPP_type_size = (uint)Math.Max(8, Math.Pow(2, Math.Ceiling(Math.Log(this.Size) / Math.Log(2))));
            Debug.Assert(cPP_type_size % 8 == 0);
            this.CppType = String.Format("std::uint{0}_t", cPP_type_size);
            this.CaptureType = String.Format("std::uint{0}_t", this.CaptureSize);
        }

        public void Visit(Structure s)
        {
            throw new NotImplementedException();
        }

        public void Visit(Enumeration e)
        {   
            if (this.NeedsMasking)
            {
                 // If we need to do some bit mangling, first convert to the underlying type (I don't think we really need to do that, but it is more clear then performing bitwise operations
                // on an enumeraiton
                e.Type.Accept(this);
            }
            else
            {
                this.CaptureType = e.Name;
            }
            // The cpp type is just the name of the enumeration type, so overwrite that.
            this.CppType = e.Name;
        }
    }
}

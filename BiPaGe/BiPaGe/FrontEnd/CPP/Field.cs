using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.FrontEnd.CPP
{
    public class Field
    {
        public String Name { get { return this.Original.Name; } }
        public uint Size { get { return this.Original.SizeInBits(); } }
        public uint Offset { get { return this.Original.Offset; } }
        public uint ByteAlginedOfffset { get; }
        public String OffsetField { get { return this.Original.OffsetFrom; } }
        public Model.FieldType Type { get { return this.Original.Type; } }
        public ulong Mask { get; }
        public uint Shift { get; }
        public uint CaptureSize { get; }
        public String CppType { get; }
        public String CaptureType { get; }
        public bool NeedsMasking { get; }        

        private Model.Field Original;
        public Field(Model.Field original)
        {
            this.Original = original;
            this.ByteAlginedOfffset = (original.Offset - (original.Offset % 8));
            this.Mask = ComputeMask();
            this.Shift = ComputeShift();
            this.CaptureSize = ComputeCaptureSize();
            this.CppType = buildCppType();
            this.NeedsMasking = ComputeMaskingRequired();
            this.CaptureType = BuildCaptureType();            
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

        private String buildCppType()
        {
            var cpp_size = (uint)Math.Max(8, Math.Pow(2, Math.Ceiling(Math.Log(this.Size) / Math.Log(2))));
            Debug.Assert(cpp_size % 8 == 0);
            return new FieldTypeConverter().Convert(this.Type, cpp_size);
        }

        private String BuildCaptureType()
        {            
            return new CaptureTypeGenerator().Generate(this.Type, this.CaptureSize, this.NeedsMasking);
        }

        private bool ComputeMaskingRequired()
        {
            return this.CaptureSize != this.Size;
        }
    }
}

using System;
namespace BiPaGe.AST.FieldTypes
{
    public abstract class SizedType : AST.FieldType
    {
        public uint Size { get; } // The size of the integer field in bits

        public SizedType(SourceInfo sourceInfo, uint size) : base(sourceInfo)
        {
            this.Size = size;
        }

        public override uint SizeInBits()
        {
            return this.Size;
        }
    }
}

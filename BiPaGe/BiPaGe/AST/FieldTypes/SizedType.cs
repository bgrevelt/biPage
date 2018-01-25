using System;
namespace BiPaGe.AST.FieldTypes
{
    public abstract class SizedType : AST.FieldType
    {
        public int Size { get; } // The size of the integer field in bits

        public SizedType(SourceInfo sourceInfo, int size) : base(sourceInfo)
        {
            this.Size = size;
        }
    }
}

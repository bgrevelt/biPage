using System;
namespace BiPaGe.AST.FieldTypes
{
    public abstract class SizedType : AST.FieldType
    {
        public int Size { get; } // The size of the integer field in bits

        public SizedType(SourceInfo sourceIfo, int size) : base(sourceIfo)
        {
            this.Size = size;
        }
    }
}

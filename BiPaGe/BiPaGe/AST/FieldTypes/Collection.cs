using System;
namespace BiPaGe.AST.FieldTypes
{
    public class Collection : AST.FieldType
    {
        public FieldType Type { get; }
        public IMultiplier Size { get; } 
        
        public Collection(FieldType type, IMultiplier multiplier)
        {
            this.Type = type;
            this.Size = multiplier;
        }

        public override void Print(int indentLevel)
        {
            PrintIndented("collection", indentLevel);
            Type.Print(indentLevel + 1);
            Size.Print(indentLevel + 1);
        }
    }
}

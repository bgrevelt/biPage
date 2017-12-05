using System;
namespace BiPaGe.AST.Types
{
    public class Collection : AST.Types.Type
    {
        public Type Type { get; }
        public IMultiplier Size { get; } 
        
        public Collection(Type type, IMultiplier multiplier)
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

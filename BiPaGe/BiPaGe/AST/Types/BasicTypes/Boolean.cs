using System;
namespace BiPaGe.AST.Types.BasicTypes
{
    public class Boolean : BasicType
    {
        public Boolean()
        {
        }

        public override void Print(int indentLevel)
        {
            PrintIndented("Boolean", indentLevel);
        }
    }
}

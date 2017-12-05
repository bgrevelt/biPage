using System;
namespace BiPaGe.AST.FieldTypes
{
    public class Boolean : AST.FieldType
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

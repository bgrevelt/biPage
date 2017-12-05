using System;
namespace BiPaGe.AST.Types.BasicTypes
{
    public class Unsigned : SizedType
    {
        public Unsigned(String typeId) : base(typeId)
        {
        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("{0} bit unsigned integer", Size), indentLevel);
        }
    }
}

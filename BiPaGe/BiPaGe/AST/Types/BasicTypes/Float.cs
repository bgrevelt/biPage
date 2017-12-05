using System;
namespace BiPaGe.AST.Types.BasicTypes
{
    public class Float : SizedType
    {
        public Float(String typeId) : base(typeId)
        {
        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("{0} bit float", Size), indentLevel);
        }
    }
}

using System;
namespace BiPaGe.AST.Identifiers
{
    public class FieldIdentifier : IMultiplier
    {
        private String id;
        public FieldIdentifier(String id)
        {
            this.id = id;
        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("Field id: {0}", id), indentLevel);
        }
    }
}

using System;
namespace BiPaGe.AST.Identifiers
{
    public class ObjectIdentifier : AST.FieldType
    {
        public String Id { get; }
        public ObjectIdentifier(String id)
        {
            this.Id = id;
        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("Object id: {0}", Id), indentLevel);
        }
    }
}

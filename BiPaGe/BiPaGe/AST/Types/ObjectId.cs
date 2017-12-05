using System;
namespace BiPaGe.AST.Types
{
    public class ObjectId : AST.Types.Type
    {
        public String Id { get; }
        public ObjectId(String id)
        {
            this.Id = id;
        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("Object id: {0}", Id), indentLevel);
        }
    }
}

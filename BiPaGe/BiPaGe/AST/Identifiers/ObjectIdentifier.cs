using System;
using System.Collections.Generic;

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

        public override bool CheckSemantics(IList<String> errors, IList<String> warnings)
        {
            // TODO: we should pass in a list of know objects and their fields
            return true;
        }
    }
}

using System;
using System.Collections.Generic;

namespace BiPaGe.AST.Identifiers
{
    public class FieldIdentifier : IMultiplier
    {
        private String id;
        public FieldIdentifier(String id)
        {
            this.id = id;
        }

        public override bool CheckSemantics(IList<String> errors, IList<String> warnings)
        {
            // TODO: we should pass in a list of know objects and their fields
            return true;
        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("Field id: {0}", id), indentLevel);
        }
    }
}

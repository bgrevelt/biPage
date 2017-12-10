using System;
using System.Collections.Generic;

namespace BiPaGe.AST.Identifiers
{
    public class FieldIdentifier : IMultiplier
    {
        private String id;
        public FieldIdentifier(SourceInfo sourceIfo, String id) : base(sourceIfo)
        {
            this.id = id;
        }

        public override bool CheckSemantics(IList<SemanticAnalysis.Error> errors, IList<SemanticAnalysis.Warning> warnings)
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

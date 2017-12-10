using System;
using System.Collections.Generic;

namespace BiPaGe.AST.FieldTypes
{
    public class Boolean : AST.FieldType
    {
        public Boolean(SourceInfo sourceInfo) : base(sourceInfo)
        {
        }

        public override bool CheckSemantics(IList<SemanticAnalysis.Error> errors, IList<SemanticAnalysis.Warning> warnings)
        {
            // Nothing to check for boolean
            return true;
        }

        public override void Print(int indentLevel)
        {
            PrintIndented("Boolean", indentLevel);
        }
    }
}

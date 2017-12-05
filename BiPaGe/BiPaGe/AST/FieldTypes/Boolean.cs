using System;
using System.Collections.Generic;

namespace BiPaGe.AST.FieldTypes
{
    public class Boolean : AST.FieldType
    {
        public Boolean()
        {
        }

        public override bool CheckSemantics(IList<String> errors, IList<String> warnings)
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

using System;
using System.Collections.Generic;
using BiPaGe.AST.Constants;
using BiPaGe.SemanticAnalysis;

namespace BiPaGe.AST.Literals
{
    public class StringLiteral : Literal
    {
        public String value
        {
            get
            {
                return value_as_string;
            }
        }

        public StringLiteral(SourceInfo sourceInfo, String value) : base(sourceInfo, value.Trim(new char[] { '"' }))
        {

        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("String literal: {0}", value_as_string), indentLevel);
        }

        public override bool Equals(Literal other)
        {
            try
            {
                return ((StringLiteral)other).value_as_string == value_as_string;
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }

        public override bool CheckSemantics(IList<Error> errors, IList<Warning> warnings)
        {
            // There is nothing to check here
            return true;
        }

        public override bool Equals(IFixer other)
        {
            try
            {
                return ((StringLiteral)other).value_as_string == value_as_string;
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }
    }
}

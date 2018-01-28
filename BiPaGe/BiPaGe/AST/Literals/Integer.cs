using System;
using System.Collections.Generic;
using BiPaGe.SemanticAnalysis;

namespace BiPaGe.AST.Literals
{
    public class Integer : Literal
    {
        public bool value
        {
            get
            {
                return bool.Parse(value_as_string);
            }
        }

        public Integer(SourceInfo sourceInfo, String value) : base(sourceInfo, value)
        {

        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("Integer literal: {0}", value_as_string), indentLevel);
        }

        public override bool Equals(Literal other)
        {
            try
            {
                return ((Integer)other).value_as_string == value_as_string;
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }

        public override bool CheckSemantics(IList<Error> errors, IList<Warning> warnings)
        {
            try
            {
                bool.Parse(value_as_string);
                return true;
            }
            catch (InvalidCastException)
            {
                errors.Add(new Error(this.sourceInfo, String.Format("Unsupported value {0} for boolean type.", value_as_string)));
                return false;
            }
        }
    }
}

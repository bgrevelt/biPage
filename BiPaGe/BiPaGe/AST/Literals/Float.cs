using System;
using System.Collections.Generic;
using BiPaGe.SemanticAnalysis;

namespace BiPaGe.AST.Literals
{
    public class Float : Literal
    {
        public Double value 
        { 
            get
            {
                return Double.Parse(value_as_string);
            }
        }

        public Float(SourceInfo sourceInfo, String value) : base(sourceInfo, value)
        {
            
        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("Float literal: {0}", value_as_string), indentLevel);
        }

        public override bool CheckSemantics(IList<Error> errors, IList<Warning> warnings)
        {
            try
            {
                Double.Parse(value_as_string);
                return true;
            }
            catch(InvalidCastException)
            {
                errors.Add(new Error(this.sourceInfo, String.Format("Unsupported value {0} for floating point type.", value_as_string)));
                return false;
            }
        }

        public override bool Equals(Literal other)
        {
            try
            {
                return ((Float)other).value_as_string == value_as_string;
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }
    }
}

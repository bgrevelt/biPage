using System;
using System.Collections.Generic;
using BiPaGe.AST.Constants;
using BiPaGe.SemanticAnalysis;
using NUnit.Framework;

namespace BiPaGe.AST.Literals
{
    public class Float : Literal
    {
        public Double Value 
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
                errors.Add(new Error(this.SourceInfo, String.Format("Unsupported value {0} for floating point type.", value_as_string)));
                return false;
            }
        }

        public override void Validate(IASTNode expected)
        {
            Assert.IsInstanceOf<Float>(expected);
            Assert.AreEqual(((Float)expected).value_as_string, this.value_as_string);
        }
    }
}

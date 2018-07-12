using System;
using System.Collections.Generic;
using BiPaGe.AST.Constants;
using BiPaGe.SemanticAnalysis;
using NUnit.Framework;

namespace BiPaGe.AST.Literals
{
    public class Boolean : Literal
    {
        public bool Value
        {
            get
            {
                return bool.Parse(value_as_string);
            }
        }

        public Boolean(SourceInfo sourceInfo, String value) : base(sourceInfo, value)
        {

        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("Bolean literal: {0}", value_as_string), indentLevel);
        }

        public override void Validate(IASTNode expected)
        {
            Assert.IsInstanceOf<Boolean>(expected);
            Assert.AreEqual(((Boolean)expected).value_as_string, this.value_as_string);
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
                errors.Add(new Error(this.SourceInfo, String.Format("Unsupported value {0} for boolean type.", value_as_string)));
                return false;
            }
        }
    }

}

using System;
using System.Collections.Generic;

namespace BiPaGe.AST.Literals
{
    public class NumberLiteral : IMultiplier
    {
        public String content { get;  }

        public NumberLiteral(String content)
        {
            this.content = content;
        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("Number literal: {0}", content), indentLevel);
        }

        public override bool CheckSemantics(IList<String> errors, IList<String> warnings)
        {
            Int64 dummy;
            if(!Int64.TryParse(content, out dummy))
            {
                errors.Add(String.Format("Invalid number literal ({0})", content));
                return false;
            }

            return true;
        }
    }
}

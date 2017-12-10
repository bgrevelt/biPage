using System;
using System.Collections.Generic;

namespace BiPaGe.AST.Literals
{
    public class NumberLiteral : IMultiplier
    {
        public String content { get;  }

        public NumberLiteral(SourceInfo sourceIfo, String content) : base(sourceIfo)
        {
            this.content = content;
        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("Number literal: {0}", content), indentLevel);
        }

        public override bool CheckSemantics(IList<SemanticAnalysis.Error> errors, IList<SemanticAnalysis.Warning> warnings)
        {
            Int64 dummy;
            if(!Int64.TryParse(content, out dummy))
            {
                errors.Add(new SemanticAnalysis.Error(sourceInfo, String.Format("Invalid number literal ({0})", content)));
                return false;
            }

            return true;
        }
    }
}

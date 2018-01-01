using System;
using System.Collections.Generic;

namespace BiPaGe.AST.Literals
{
    public class NumberLiteral : IMultiplier
    {
        public Int64 Number { get; }

        public NumberLiteral(SourceInfo sourceIfo, String content) : base(sourceIfo)
        {
            this.Number = Int64.Parse(content);
            /*
             *             Int64 dummy;
            if(!Int64.TryParse(content, out dummy))
            {
                errors.Add(new SemanticAnalysis.Error(sourceInfo, String.Format("Invalid number literal ({0})", content)));
                return false;
            }
            */
        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("Number literal: {0}", Number), indentLevel);
        }

        public override bool CheckSemantics(IList<SemanticAnalysis.Error> errors, IList<SemanticAnalysis.Warning> warnings)
        {
            return true;
        }
    }
}

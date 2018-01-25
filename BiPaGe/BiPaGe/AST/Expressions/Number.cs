using System;
using System.Collections.Generic;

namespace BiPaGe.AST.Expressions
{
    public class Number : Expression
    {
        public int value { get; }

        public Number(SourceInfo sourceIfo, String content) : base(sourceIfo)
        {
            this.value = int.Parse(content);
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
            PrintIndented(String.Format("Number literal: {0}", value), indentLevel);
        }

        public override bool CheckSemantics(IList<SemanticAnalysis.Error> errors, IList<SemanticAnalysis.Warning> warnings)
        {
            return true;
        }

        public override bool Equals(Expression other)
        {
            try
            {
                return ((Number)other).value == this.value;
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using BiPaGe.SemanticAnalysis;

namespace BiPaGe.AST.Expressions
{
    public class This : Expression
    {
        public This(SourceInfo sourceInfo) : base(sourceInfo)
        {
        }

        public override bool CheckSemantics(IList<Error> errors, IList<Warning> warnings)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(Expression other)
        {
            return other.GetType() == typeof(This);
        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("this"), indentLevel);
        }
    }
}

using System;
using System.Collections.Generic;
using BiPaGe.SemanticAnalysis;

namespace BiPaGe.AST.Expressions
{
    public class This : ASTNode, IExpression
    {
        public This(SourceInfo sourceInfo) : base(sourceInfo)
        {
        }

        public override bool CheckSemantics(IList<Error> errors, IList<Warning> warnings)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IExpression other)
        {
            return other.GetType() == typeof(This);
        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("this"), indentLevel);
        }
    }
}

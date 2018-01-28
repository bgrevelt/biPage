using System;
using System.Collections.Generic;
using BiPaGe.SemanticAnalysis;

namespace BiPaGe.AST.Expressions
{
    public class Multiplication : ASTNode, IExpression
    {
        public IExpression left { get; }
        public IExpression right { get; }

        public Multiplication(SourceInfo sourceInfo, IExpression lhs, IExpression rhs) : base(sourceInfo)
        {
            left = lhs;
            right = rhs;
        }

        public override bool CheckSemantics(IList<Error> errors, IList<Warning> warnings)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IExpression other)
        {
            try
            {
                return left.Equals(((Multiplication)other).left) && right.Equals(((Multiplication)other).right);
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("Multiplication"), indentLevel);
            left.Print(indentLevel + 1);
            right.Print(indentLevel + 1);
        }
    }
}

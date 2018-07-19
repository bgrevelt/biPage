using System;
namespace BiPaGe.AST.Expressions
{
    public interface IExpression : IASTNode
    {
        void Accept(IExpressionVisitor v);
    }
}

﻿namespace BiPaGe.Model.Expressions
{
    public class Division : BinaryExpression
    {
        public Division(Expression lhs, Expression rhs)
        {
            this.Left = lhs;
            this.Right = rhs;
        }

        public override int? Resolve()
        {
            return this.Left.Resolve() / this.Right.Resolve();
        }

        public override void Accept(IExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

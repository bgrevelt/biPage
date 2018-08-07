using System;
namespace BiPaGe.Model
{
    public interface IExpressionVisitor
    {
        void Visit(Expressions.Addition a);
        void Visit(Expressions.Division d);
        void Visit(Expressions.FieldIdentifier f);
        void Visit(Expressions.Multiplication m);
        void Visit(Expressions.Number n);
        void Visit(Expressions.Subtraction s);
        void Visit(Expressions.This t);
    }
}

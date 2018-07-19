using System;
using System.Diagnostics;

namespace BiPaGe.Model
{
    public class ExpressionTranslator : AST.Expressions.IExpressionVisitor
    {
        public Expressions.Expression translated = null;

        public Expressions.Expression Translate(AST.Expressions.IExpression e)
        {
            translated = null;
            e.Accept(this);
            Debug.Assert(translated != null);
            return translated;
        }

        public void Visit(AST.Expressions.Addition a)
        {
            translated = new Expressions.Addition(Translate(a.Left), Translate(a.Right));
        }

        public void Visit(AST.Expressions.Subtraction s)
        {
            translated = new Expressions.Subtraction(Translate(s.Left), Translate(s.Right));
        }

        public void Visit(AST.Expressions.Multiplication m)
        {
            translated = new Expressions.Multiplication(Translate(m.Left), Translate(m.Right));
        }

        public void Visit(AST.Expressions.Division d)
        {
            translated = new Expressions.Division(Translate(d.Left), Translate(d.Right));
        }

        public void Visit(AST.Expressions.This t)
        {
            translated = new Expressions.This();
        }

        public void Visit(BiPaGe.AST.Identifiers.FieldIdentifier f)
        {
            translated = new Expressions.FieldIdentifier(f.Id);
        }

        public void Visit(BiPaGe.AST.Literals.Integer i)
        {
            translated = new Expressions.Number(i.Value);
        }
    }
}

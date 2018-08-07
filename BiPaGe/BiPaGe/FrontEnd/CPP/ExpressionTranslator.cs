using System;
using System.Diagnostics;
using BiPaGe.Model.Expressions;

namespace BiPaGe.FrontEnd.CPP
{
    
    public class ExpressionTranslator : Model.IExpressionVisitor
    {
        private String Translated;

        public ExpressionTranslator()
        {
        }

        public String Translate(Model.Expressions.Expression expression)
        {
            this.Translated = null;
            expression.Accept(this);
            Debug.Assert(this.Translated != null);
            return this.Translated;
        }

        public void Visit(Addition a)
        {
            throw new NotImplementedException();
        }

        public void Visit(Division d)
        {
            throw new NotImplementedException();
        }

        public void Visit(FieldIdentifier f)
        {
            throw new NotImplementedException();
        }

        public void Visit(Multiplication m)
        {
            throw new NotImplementedException();
        }

        public void Visit(Number n)
        {
            this.Translated = n.Value.ToString();
        }

        public void Visit(Subtraction s)
        {
            throw new NotImplementedException();
        }

        public void Visit(This t)
        {
            throw new NotImplementedException();
        }
    }
}

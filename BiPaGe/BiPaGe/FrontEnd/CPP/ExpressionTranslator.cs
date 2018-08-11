using System;
using System.Diagnostics;
using BiPaGe.Model.Expressions;

namespace BiPaGe.FrontEnd.CPP
{
    
    public class ExpressionTranslator : Model.IExpressionVisitor
    {
        private String Translated;
        private Model.Field field;
        public ExpressionTranslator(Model.Field field)
        {
            this.field = field;
        }

        public String Translate(Model.Expressions.Expression expression)
        {
            this.Translated = null;
            expression.Accept(this);
            Debug.Assert(this.Translated != null);

            if(this.Translated.StartsWith("(") && this.Translated.EndsWith(")"))
            {
                this.Translated = this.Translated.Substring(1, this.Translated.Length - 2);
            }

            return this.Translated;
        }

        public void Visit(Addition a)
        {
            a.Left.Accept(this);
            var left = this.Translated;
            a.Right.Accept(this);
            this.Translated = $"({left} + {this.Translated})";
        }

        public void Visit(Division d)
        {
            d.Left.Accept(this);
            var left = this.Translated;
            d.Right.Accept(this);
            this.Translated = $"({left} / {this.Translated})";
        }

        public void Visit(FieldIdentifier f)
        {
            this.Translated = $"{f.Identifier}()";
        }

        public void Visit(Multiplication m)
        {
            m.Left.Accept(this);
            var left = this.Translated;
            m.Right.Accept(this);
            this.Translated = $"({left} * {this.Translated})";
        }

        public void Visit(Number n)
        {
            this.Translated = n.Value.ToString();
        }

        public void Visit(Subtraction s)
        {
            s.Left.Accept(this);
            var left = this.Translated;
            s.Right.Accept(this);
            this.Translated = $"({left} - {this.Translated})";
        }

        public void Visit(This t)
        {
            Debug.Assert(this.field.Offset % 8 == 0, "'this'  should only be used for fields located on a byte aligned offset. This should have been caught in the semantic analysis");
            String translated = "";
            if (this.field.OffsetFrom != null)
                translated = $"({this.field.OffsetFrom}().end() - reinterpret_cast<const std::uint8_t*>(this))";
            
            this.Translated = $"{translated} + {this.field.Offset / 8}";
        }
    }
}

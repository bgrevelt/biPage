using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.Model
{
    // TODO: this whole class feels kind of dodgy to me. It appear to make more sense to just have IsStaticExpression and Resolve as member functions of an expression
    // I also don't really like the concept of throwing an exception when an expression cannot be solved. Maybe we should return null instead. That way we can just test expressions to see
    // if they can be resolved instead of quuerying for 'staticness'
    class ExpressionResolver
    {
        public bool IsStaticExpression(Model.Expressions.Expression expression)
        {
            return IsStaticExpression((dynamic)expression);
        }

        public int Resolve(Model.Expressions.Expression expression)
        {
            return Resolve((dynamic)expression);
        }

        private bool IsStaticExpression(Model.Expressions.BinaryExpression ex)
        {
            return IsStaticExpression((dynamic)ex.Left) && IsStaticExpression((dynamic)ex.Right);
        }

        private bool IsStaticExpression(Model.Expressions.FieldIdentifier f)
        {
            return false;
        }

        private bool IsStaticExpression(Model.Expressions.Number n)
        {
            return true;
        }

        private bool IsStaticExpression(Model.Expressions.This t)
        {
            return true;
        }

        private int Resolve(Model.Expressions.Addition ex)
        {
            return Resolve((dynamic)ex.Left) + Resolve((dynamic)ex.Right);
        }

        private int Resolve(Model.Expressions.Subtraction ex)
        {
            return Resolve((dynamic)ex.Left) - Resolve((dynamic)ex.Right);
        }

        private int Resolve(Model.Expressions.Multiplication ex)
        {
            return Resolve((dynamic)ex.Left) * Resolve((dynamic)ex.Right);
        }

        private int Resolve(Model.Expressions.Division ex)
        {
            return Resolve((dynamic)ex.Left) / Resolve((dynamic)ex.Right);
        }

        private int Resolve(Model.Expressions.FieldIdentifier f)
        {
            throw new InvalidOperationException("Cant resolve an expression with an identifier in it at compile time");
        }

        private int Resolve(Model.Expressions.Number n)
        {
            return n.Value;
        }

        private int Resolve(Model.Expressions.This t)
        {
            // TODO
            throw new NotImplementedException("I don't know quite how to handle this yet. Sorry");
        }
    }
}

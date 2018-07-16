using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.FrontEnd
{
    class ExpressionResolver
    {
        public bool IsStaticExpression(Model.Expressions.Expression expression)
        {
            return IsStaticExpression((dynamic)expression);
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.Model.Expressions
{
    public class BinaryExpression : Expression
    {
        protected Expression left;
        protected Expression right;
        public Expression Left { get { return left; } }
        public Expression Right { get { return right; } }

    }
}

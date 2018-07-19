using BiPaGe.AST.Identifiers;
using BiPaGe.AST.Literals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.AST.Expressions
{
    public interface IExpressionVisitor
    {
        void Visit(Addition a);
        void Visit(Subtraction s);
        void Visit(Multiplication m);
        void Visit(Division d);
        void Visit(This t);
        void Visit(FieldIdentifier f);
        void Visit(Integer i);
    }
}

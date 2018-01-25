using System;
namespace BiPaGe.AST.Expressions
{
    public abstract class Expression : IASTNode
    {
        public Expression(SourceInfo sourceInfo) : base(sourceInfo)
        {
        }

        public abstract bool Equals(Expression other);
    }
}

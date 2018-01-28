using System;
namespace BiPaGe.AST
{
    public abstract class IMultiplier : ASTNode
    {
        public IMultiplier(SourceInfo sourceInfo) : base(sourceInfo)
        {
        }

        public abstract bool Equals(IMultiplier other);
    }
}

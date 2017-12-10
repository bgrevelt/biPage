using System;
namespace BiPaGe.AST
{
    public abstract class IMultiplier : IASTNode
    {
        public IMultiplier(SourceInfo sourceInfo) : base(sourceInfo)
        {
        }
    }
}

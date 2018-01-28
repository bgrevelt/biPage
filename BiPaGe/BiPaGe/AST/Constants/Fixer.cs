using System;
namespace BiPaGe.AST.Constants
{
    public abstract class Fixer : IASTNode
    {
        public Fixer(SourceInfo sourceInfo) : base(sourceInfo)
        {
        }
    }
}

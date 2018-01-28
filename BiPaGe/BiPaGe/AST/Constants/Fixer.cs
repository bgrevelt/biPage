using System;
namespace BiPaGe.AST.Constants
{
    public abstract class Fixer : ASTNode
    {
        public Fixer(SourceInfo sourceInfo) : base(sourceInfo)
        {
        }
    }
}

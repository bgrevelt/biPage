using System;
namespace BiPaGe.AST
{
    public abstract class Element : ASTNode
    {
        public Element(SourceInfo sourceinfo) : base(sourceinfo)
        {
            
        }
    }
}

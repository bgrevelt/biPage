using System;
namespace BiPaGe.AST
{
    public abstract class Element : IASTNode
    {
        public Element(SourceInfo sourceinfo) : base(sourceinfo)
        {
            
        }
    }
}

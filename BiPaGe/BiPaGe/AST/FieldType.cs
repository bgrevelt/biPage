using System;
namespace BiPaGe.AST
{
    public abstract class FieldType : IASTNode
    {
        public FieldType(SourceInfo sourceInfo) : base(sourceInfo)
        {
        }
    }
}

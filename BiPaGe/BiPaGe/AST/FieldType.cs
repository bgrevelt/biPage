using System;
namespace BiPaGe.AST
{
    public abstract class FieldType : ASTNode
    {
        public FieldType(SourceInfo sourceInfo) : base(sourceInfo)
        {
        }

        public abstract bool Equals(FieldType other);
    }
}

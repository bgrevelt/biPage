using System;
namespace BiPaGe.AST
{
    public abstract class FieldType : ASTNode
    {
        public FieldType(SourceInfo sourceInfo) : base(sourceInfo)
        {
        }

        public abstract void Accept(FieldTypes.IFieldTypeVisitor v);
    }
}

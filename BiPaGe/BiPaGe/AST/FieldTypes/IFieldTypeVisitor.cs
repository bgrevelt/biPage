namespace BiPaGe.AST.FieldTypes
{
    public interface IFieldTypeVisitor
    {
        void Visit(AST.Identifiers.Identifier i);
        void Visit(AST.FieldTypes.Boolean b);
        void Visit(AST.FieldTypes.Float f);
        void Visit(AST.FieldTypes.InlineEnumeration ie);
        void Visit(AST.FieldTypes.InlineObject io);
        void Visit(AST.FieldTypes.Signed s);
        void Visit(AST.FieldTypes.Unsigned u);
        void Visit(AST.FieldTypes.AsciiString s);
        void Visit(AST.FieldTypes.Utf8String s);
    }
}

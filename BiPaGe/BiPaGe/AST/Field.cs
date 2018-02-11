using System;
using System.Collections.Generic;
using BiPaGe.AST.Expressions;
namespace BiPaGe.AST
{
    public class Field : ASTNode
    {
        public String Name { get; }
        public FieldType Type { get; }
        public IExpression CollectionSize { get; }
        public Constants.IFixer Fixer { get; }

        public Field(SourceInfo sourceIfo, String name, AST.FieldType type, IExpression collection_size, Constants.IFixer fixer) : base(sourceIfo)
        {
            this.Name = name;
            this.Type = type;
            this.CollectionSize = collection_size;
            this.Fixer = fixer;
        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("Field {0}: ", Name), indentLevel);
            Type.Print(indentLevel + 1);
            CollectionSize?.Print((indentLevel + 1));
            Fixer?.Print(indentLevel + 1);
        }

        public override bool CheckSemantics(IList<SemanticAnalysis.Error> errors, IList<SemanticAnalysis.Warning> warnings)
        {
            return Type.CheckSemantics(errors, warnings);
        }

        public override bool Equals(IASTNode other)
        {
            var other_field = other as Field;

            if (this.Name != other_field.Name)
                return false;

            if (!this.Type.Equals(other_field.Type))
                return false;

            if (this.CollectionSize?.Equals(other_field.CollectionSize) == false)
                return false;

            if (this.Fixer?.Equals(other_field.Fixer) == false)
                return false;

            return true;
        }
    }
}

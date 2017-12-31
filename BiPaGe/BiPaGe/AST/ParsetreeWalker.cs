using System;
using System.Collections.Generic;

namespace BiPaGe.AST
{
    public class ParsetreeWalker : BiPaGeBaseVisitor<AST.IASTNode>
    {
        public ParsetreeWalker() 
        {
        }

        public override AST.IASTNode VisitObjects(BiPaGeParser.ObjectsContext context)
        {
            List<AST.Object> objects = new List<AST.Object>();
            foreach (var obj in context.@object())
            {
                objects.Add((dynamic)obj.Accept(this));
            }

            return new AST.Parser(GetSourceInfo(context.Start), "No name yet", objects);
        }

        public override AST.IASTNode VisitObject(BiPaGeParser.ObjectContext context)
        {
            List<AST.Field> fields = new List<AST.Field>();
            foreach (var field in context.field())
            {
                fields.Add((dynamic)field.Accept(this));
            }

            var objectName = context.Identifier().GetText();

            return new AST.Object(GetSourceInfo(context.Start), objectName, fields);
        }

        public override AST.IASTNode VisitField(BiPaGeParser.FieldContext context)
        {
            var fieldName = context.name.Text;
            AST.FieldType type = (dynamic)context.fieldType().Accept(this);
            return new AST.Field(GetSourceInfo(context.Start), fieldName, type);
        }

        public override AST.IASTNode VisitSingular(BiPaGeParser.SingularContext context)
        {
            if (context.Identifier() != null)
            {
                // this is a complex field (e.g. the type is another object)
                return new AST.Identifiers.ObjectIdentifier(GetSourceInfo(context.Identifier()), context.Identifier().GetText());
            }
            else
            {
                // TODO: assert basictype is valid
                // The basictype identifier is of the form 
                var type = context.BasicType().GetText();
                var type_only = type.TrimEnd("0123456789".ToCharArray());
                switch (type_only)
                {
                    case "int":
                    case "s":
                        return new AST.FieldTypes.Signed(GetSourceInfo(context.Start), type);
                    case "uint":
                    case "u":
                        return new AST.FieldTypes.Unsigned(GetSourceInfo(context.Start), type);
                    case "float":
                    case "f":
                        return new AST.FieldTypes.Float(GetSourceInfo(context.Start), type);
                    case "bool":
                        return new AST.FieldTypes.Boolean(GetSourceInfo(context.Start));
                }
            }

            // TODO: solve better
            throw new ArgumentException();
        }

        public override AST.IASTNode VisitCollection(BiPaGeParser.CollectionContext context)
        {
            AST.FieldType type = (dynamic)context.singular().Accept(this);
            AST.IMultiplier multiplier = (dynamic)context.multiplier().Accept(this);

            return new AST.FieldTypes.Collection(GetSourceInfo(context.Start), type, multiplier);
        }

        public override AST.IASTNode VisitMultiplier(BiPaGeParser.MultiplierContext context)
        {
            if (context.NumberLiteral() != null)
                return new AST.Literals.NumberLiteral(GetSourceInfo(context.Start), context.NumberLiteral().GetText());
            else
            {
                String identifier = "";
                foreach (var id in context.Identifier())
                {
                    identifier = identifier + id.GetText() + ".";
                }
                identifier.Remove(identifier.Length - 1);

                return new AST.Identifiers.FieldIdentifier(GetSourceInfo(context.Start), identifier);
            }
        }

        private AST.SourceInfo GetSourceInfo(Antlr4.Runtime.Tree.ITerminalNode node)
        {
            return GetSourceInfo(node.Symbol);
        }

        private AST.SourceInfo GetSourceInfo(Antlr4.Runtime.IToken token)
        {
            return new AST.SourceInfo(token.Line, token.Column);
        }
    }
}

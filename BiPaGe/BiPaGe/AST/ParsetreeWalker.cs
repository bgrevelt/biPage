using System;
using System.Collections.Generic;
using System.Diagnostics;
using BiPaGe.AST.Identifiers;

namespace BiPaGe.AST
{
    public class ParsetreeWalker : BiPaGeBaseVisitor<AST.IASTNode>
    {
        public ParsetreeWalker() 
        {
        }

        public override IASTNode VisitProgram(BiPaGeParser.ProgramContext context)
        {
            List<AST.Element> elements = new List<AST.Element>();
            foreach (var element in context.element())
            {
                elements.Add((dynamic)element.Accept(this));
            }


            return new AST.Parser(GetSourceInfo(context.Start), "No name yet", elements);
        }

        public override IASTNode VisitElement(BiPaGeParser.ElementContext context)
        {
            Debug.Assert(context.@object() != null || context.enumeration() != null);
            if (context.@object() != null)
                return context.@object().Accept(this);
            else 
                return context.enumeration().Accept(this);
        }

        public override IASTNode VisitEnumeration(BiPaGeParser.EnumerationContext context)
        {
            var enumerators = new List<AST.Enumerator>();
            foreach(var enumerator in context.enumerator())
            {
                enumerators.Add((dynamic)enumerator.Accept(this));
            }
            return new AST.Enumeration(GetSourceInfo(context.Start), context.Identifier().GetText(), enumerators);
        }

        public override IASTNode VisitEnumerator(BiPaGeParser.EnumeratorContext context)
        {
            return new Enumerator(GetSourceInfo(context.Start), context.Identifier().GetText(), context.NumberLiteral().GetText());
        }

        public override AST.IASTNode VisitObject(BiPaGeParser.ObjectContext context)
        {
            List<AST.Field> fields = new List<AST.Field>();
            foreach (var field in context.field())
            {
                var a = field.Accept(this);
                fields.Add((dynamic)a);
            }

            var objectName = context.Identifier().GetText();

            return new AST.Object(GetSourceInfo(context.Start), objectName, fields);
        }

        public override IASTNode VisitField(BiPaGeParser.FieldContext context)
        {
            String identifier = context.Identifier()?.GetText();
            IASTNode type = context.field_type().Accept(this);
            IASTNode collection_size = context.expression()?.Accept(this);
            // TODO: initializer

            return new Field(GetSourceInfo(context.Start), identifier, (dynamic)type, (dynamic)collection_size);
        }

        public override IASTNode VisitField_type(BiPaGeParser.Field_typeContext context)
        {
            // (Type | Identifier | inline_enumeration | inline_object);
            if(context.Type() != null)
            {
                return ParseType(context.Type().GetText(), GetSourceInfo(context.Start));
            }
            else if(context.Identifier() != null)
            {
                return new ObjectIdentifier(GetSourceInfo(context.Start), context.Identifier().GetText());
            }
            else if(context.inline_enumeration() != null)
            {
                
            }
            else if(context.inline_enumeration() != null)
            {
                
            }

            throw new NotImplementedException();
        }

        public override IASTNode VisitExpression(BiPaGeParser.ExpressionContext context)
        {
            if(context.NumberLiteral() != null)
            {
                return new Literals.NumberLiteral(GetSourceInfo(context.Start), context.NumberLiteral().GetText());
            }
            /*
             expression:
             NumberLiteral
            | This
            | field_id
            |'(' expression ')'
            | expression '+' expression
            | expression '-' expression
            | expression '*' expression
            | expression '/' expression;
            */
            throw new NotImplementedException();
        }

        //public override IASTNode VisitNumber(BiPaGeParser.NumberContext context)
        //{
        //    return new AST.Literals.NumberLiteral(GetSourceInfo(context.Start), context.NumberLiteral().GetText());
        //}

        //public override AST.IASTNode VisitCollection(BiPaGeParser.CollectionContext context)
        //{
        //    AST.FieldType type = (dynamic)context.singular().Accept(this);
        //    AST.IMultiplier multiplier = (dynamic)context.multiplier().Accept(this);

        //    return new AST.FieldTypes.Collection(GetSourceInfo(context.Start), type, multiplier);
        //}

        //public override AST.IASTNode VisitMultiplier(BiPaGeParser.MultiplierContext context)
        //{
        //    if (context.NumberLiteral() != null)
        //        return new AST.Literals.NumberLiteral(GetSourceInfo(context.Start), context.NumberLiteral().GetText());
        //    else
        //    {
        //        String identifier = "";
        //        foreach (var id in context.Identifier())
        //        {
        //            identifier = identifier + id.GetText() + ".";
        //        }
        //        identifier.Remove(identifier.Length - 1);

        //        return new AST.Identifiers.FieldIdentifier(GetSourceInfo(context.Start), identifier);
        //    }
        //}

        private AST.SourceInfo GetSourceInfo(Antlr4.Runtime.Tree.ITerminalNode node)
        {
            return GetSourceInfo(node.Symbol);
        }

        private AST.SourceInfo GetSourceInfo(Antlr4.Runtime.IToken token)
        {
            return new AST.SourceInfo(token.Line, token.Column);
        }

        private FieldType ParseType(String type, SourceInfo sourceInfo)
        {
            var type_only = type.TrimEnd("0123456789".ToCharArray());
            int size = 0;
            int.TryParse(type.TrimStart("abcdefghijklmnopqrstuvwxyz".ToCharArray()),out size);
            Debug.Assert(new HashSet<String> { "int", "i", "uint", "u", "float", "f", "bool", "ascii_string", "utf8_string" }.Contains(type_only));

            switch (type_only)
            {
                case "int":
                case "i":
                    return new AST.FieldTypes.Signed(sourceInfo, size);
                case "uint":
                case "u":
                    return new AST.FieldTypes.Unsigned(sourceInfo, size);
                case "float":
                case "f":
                    return new AST.FieldTypes.Float(sourceInfo, size);
                case "bool":
                    return new AST.FieldTypes.Boolean(sourceInfo);
                case "ascii_string":
                    return new AST.FieldTypes.AsciiString(sourceInfo);
                case "utf8_string":
                    return new AST.FieldTypes.Utf8String(sourceInfo);
                default:
                    // TODO: solve better
                    throw new ArgumentException();
            }
        }
    }
}

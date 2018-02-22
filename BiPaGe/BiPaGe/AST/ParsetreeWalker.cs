using System;
using System.Collections.Generic;
using System.Diagnostics;
using BiPaGe.AST.Identifiers;
using BiPaGe.AST.Expressions;

// TODO: Parsetree walker should be able to handle errors in the input better. Right now we get all sorts of exceptions if there are real problems with the input because members of context
// can be null in that case. We can simply catch the exceptyion and return an empty program object, but since invalid input is not an exceptional situation for a compiler, I think we
// should just properly handle these situations. I think that means checking all required member of the context for being null before we handle them and returning an default node if they are.

namespace BiPaGe.AST
{
    public class ParsetreeWalker : BiPaGeBaseVisitor<AST.ASTNode>
    {
        public ParsetreeWalker() 
        {
        }

        public override ASTNode VisitProgram(BiPaGeParser.ProgramContext context)
        {
            List<AST.Element> elements = new List<AST.Element>();
            foreach (var element in context.element())
            {
                elements.Add((dynamic)element.Accept(this));
            }


            return new AST.Parser(GetSourceInfo(context.Start), "No name yet", elements);
        }

        public override ASTNode VisitElement(BiPaGeParser.ElementContext context)
        {
            Debug.Assert(context.@object() != null || context.enumeration() != null);
            if (context.@object() != null)
                return context.@object().Accept(this);
            else 
                return context.enumeration().Accept(this);
        }

        public override ASTNode VisitEnumeration(BiPaGeParser.EnumerationContext context)
        {
            var enumerators = new List<AST.Enumerator>();
            foreach(var enumerator in context.enumerator())
            {
                enumerators.Add((dynamic)enumerator.Accept(this));
            }
            var type = ParseType(context.Type().GetText(), GetSourceInfo(context.Start));


            return new AST.Enumeration(GetSourceInfo(context.Start), context.Identifier().GetText(), type, enumerators);
        }

        public override ASTNode VisitEnumerator(BiPaGeParser.EnumeratorContext context)
        {
            string sign = context.negative() != null ? "-" : "";
            return new Enumerator(GetSourceInfo(context.Start), sign + context.Identifier().GetText(), context.NumberLiteral().GetText());
        }

        public override AST.ASTNode VisitObject(BiPaGeParser.ObjectContext context)
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

        public override ASTNode VisitField(BiPaGeParser.FieldContext context)
        {
            String identifier = context.Identifier()?.GetText();
            ASTNode type = context.field_type().Accept(this);
            ASTNode collection_size = context.expression()?.Accept(this);
            ASTNode fixer = context.fixer()?.Accept(this);

            return new Field(GetSourceInfo(context.Start), identifier, (dynamic)type, (dynamic)collection_size, (dynamic)fixer);
        }

        public override ASTNode VisitField_type(BiPaGeParser.Field_typeContext context)
        {
            // (Type | Identifier | inline_enumeration | inline_object);
            if(context.Type() != null)
            {
                return ParseType(context.Type().GetText(), GetSourceInfo(context.Start));
            }
            else if(context.Identifier() != null)
            {
                return new Identifier(GetSourceInfo(context.Start), context.Identifier().GetText());
            }
            else if(context.inline_enumeration() != null)
            {
                return context.inline_enumeration().Accept(this);
            }
            else if(context.inline_object() != null)
            {
                return context.inline_object().Accept(this);
            }

            throw new NotImplementedException();
        }

        public override ASTNode VisitNumber(BiPaGeParser.NumberContext context)
        {
            return new Literals.Integer(GetSourceInfo(context.Start), context.NumberLiteral().GetText());
        }

        public override ASTNode VisitOffset(BiPaGeParser.OffsetContext context)
        {
            return new This(GetSourceInfo(context.Start));
        }

        public override ASTNode VisitFieldId(BiPaGeParser.FieldIdContext context)
        {
            String identifier = "";
            foreach (var id in context.field_id().Identifier())
            {
                identifier = identifier + id.GetText() + ".";
            }
            // Remove trailing dot
            identifier = identifier.Remove(identifier.Length - 1);

            return new FieldIdentifier(GetSourceInfo(context.Start), identifier);
        }

        public override ASTNode VisitBinaryOperation(BiPaGeParser.BinaryOperationContext context)
        {
            var lhs = context.left.Accept(this);
            var rhs = context.right.Accept(this);
            var sourceInfo = GetSourceInfo(context.Start);
            switch(context.op.Text)
            {
                case "+": return new Addition(sourceInfo, (dynamic)lhs, (dynamic)rhs);
                case "-": return new Subtraction(sourceInfo, (dynamic)lhs, (dynamic)rhs);
                case "/": return new Division(sourceInfo, (dynamic)lhs, (dynamic)rhs);
                case "*": return new Multiplication(sourceInfo, (dynamic)lhs, (dynamic)rhs);
                default:
                    // TODO: solve better
                    throw new ArgumentException();
            }
        }

        public override ASTNode VisitParentheses(BiPaGeParser.ParenthesesContext context)
        {
            return context.expression().Accept(this);
        }

        public override ASTNode VisitFixer(BiPaGeParser.FixerContext context)
        {
            if(context.field_constant() != null)
            {
                return context.field_constant().Accept(this);
            }
            return base.VisitFixer(context);
        }

        public override ASTNode VisitField_constant(BiPaGeParser.Field_constantContext context)
        {
            return context.constant().Accept(this);
        }

        public override ASTNode VisitLiteralConstant(BiPaGeParser.LiteralConstantContext context)
        {
            return context.literal().Accept(this);
        }

        public override ASTNode VisitLiteral(BiPaGeParser.LiteralContext context)
        {
            string sign = context.negative()  != null ? "-" : "";

            if (context.NumberLiteral() != null)
                return new Literals.Integer(GetSourceInfo(context.Start), sign + context.NumberLiteral().GetText());
            else if (context.FloatLiteral() != null)
                return new Literals.Float(GetSourceInfo(context.Start), sign + context.FloatLiteral().GetText());
            else if (context.BooleanLiteral() != null)
                return new Literals.Boolean(GetSourceInfo(context.Start), context.BooleanLiteral().GetText());
            else if (context.StringLiteral() != null)
                return new Literals.StringLiteral(GetSourceInfo(context.Start), context.StringLiteral().GetText());

            throw new ArgumentException();
        }

        public override ASTNode VisitNumberCollection(BiPaGeParser.NumberCollectionContext context)
        {
            List<Literals.Literal> literals = new List<Literals.Literal>();
            foreach(var literal in context.literal())
            {
                literals.Add((dynamic)literal.Accept(this));
            }
            return new Constants.LiteralCollection(GetSourceInfo(context.Start), literals);
        }

        public override ASTNode VisitObjectId(BiPaGeParser.ObjectIdContext context)
        {
            return new Identifier(GetSourceInfo(context.Start), context.Identifier().GetText());
        }

        public override ASTNode VisitInline_object(BiPaGeParser.Inline_objectContext context)
        {
            List<Field> fields = new List<Field>();
            foreach (var field in context.field())
            {
                var a = field.Accept(this);
                fields.Add((dynamic)a);
            }

            return new FieldTypes.InlineObject(GetSourceInfo(context.Start), fields);
        }

        public override ASTNode VisitInline_enumeration(BiPaGeParser.Inline_enumerationContext context)
        {
            var enumerators = new List<Enumerator>();
            foreach (var enumerator in context.enumerator())
            {
                enumerators.Add((dynamic)enumerator.Accept(this));
            }
            var type = ParseType(context.Type().GetText(), GetSourceInfo(context.Start));


            return new FieldTypes.InlineEnumeration(GetSourceInfo(context.Start), type, enumerators);
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

    

        //public override ASTNode VisitNumber(BiPaGeParser.NumberContext context)
        //{
        //    return new AST.Literals.NumberLiteral(GetSourceInfo(context.Start), context.NumberLiteral().GetText());
        //}

        //public override AST.ASTNode VisitCollection(BiPaGeParser.CollectionContext context)
        //{
        //    AST.FieldType type = (dynamic)context.singular().Accept(this);
        //    AST.IMultiplier multiplier = (dynamic)context.multiplier().Accept(this);

        //    return new AST.FieldTypes.Collection(GetSourceInfo(context.Start), type, multiplier);
        //}

        //public override AST.ASTNode VisitMultiplier(BiPaGeParser.MultiplierContext context)
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
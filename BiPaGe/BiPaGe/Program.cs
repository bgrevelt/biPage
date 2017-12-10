using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Antlr4.Runtime;

namespace BiPaGe
{
    class Visitor : BiPaGeBaseVisitor<AST.IASTNode>
    {
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
            if(context.Identifier() != null)
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
                switch(type_only)
                {
                    case "int": 
                    case "s":
                        return new AST.FieldTypes.Signed(GetSourceInfo(context.Start), type);
                    case "uint" :
                    case "u": 
                        return new AST.FieldTypes.Unsigned(GetSourceInfo(context.Start), type);
                    case "float" : 
                    case "f":
                        return new AST.FieldTypes.Float(GetSourceInfo(context.Start), type);
                    case "bool" : 
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
                foreach(var id in context.Identifier())
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

    public class ErrorListener : BaseErrorListener, IAntlrErrorListener<int>
    {
        private IList<SemanticAnalysis.Error> errors;
        private IList<SemanticAnalysis.Warning> warnings;

        public ErrorListener(IList<SemanticAnalysis.Error> errors, IList<SemanticAnalysis.Warning> warnings)
        {
            this.errors = errors;
            this.warnings = warnings;
        }
        public override void SyntaxError(System.IO.TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            this.errors.Add(new SemanticAnalysis.Error(new AST.SourceInfo(line, charPositionInLine), msg));
        }

        public void SyntaxError(TextWriter output, IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            this.errors.Add(new SemanticAnalysis.Error(new AST.SourceInfo(line, charPositionInLine), msg));
        }
    }


    class MainClass
    {
        public static void Main(string[] args)
        {
            var input =
@"Object1 
{
field_one: int12;
field_two: u4;
}

Object2
{
field_three: uint18;
field_four: s14;
field_five: float64;
}

Object3
{
field_six: f32;
field_seven: bool;
}

ObjectWithCollections
{
  size_field : uint16;
  embedded : Object2;
  collection_one : int32[5]; // hard coded size
  collection_two : bool[size_field]; // sized from field
  collection_three : float64[embedded.field_three];
}
";
            var invalid_input =
@"Object1 
{
field_one: int1;
field_two: u1;
}

Object2
{
field_three: uint18;
field_four: s0;
field_five: float12;
}

Object3
{
field_six, flaot49;
field_seven: bool5;
}

ObjectWithCollections
{
  size_field : uint16;
  embedded : Object2;
  collection_one : int32[5]; // hard coded size
  collection_two : bool[size_field]; // sized from field
  collection_three : float64[embedded.field_three];
}
";

            var inputStream = new AntlrInputStream(invalid_input);
            var lexer = new BiPaGeLexer(inputStream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new BiPaGeParser(tokens);

            var errors = new List<SemanticAnalysis.Error>();
            var warnings = new List<SemanticAnalysis.Warning>();

            var errorListener = new ErrorListener(errors, warnings);
            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(errorListener);
            parser.RemoveErrorListeners();
            parser.AddErrorListener(errorListener);
            var f = parser.objects();

            var test = new Visitor();
            var AST = test.Visit(f);
            bool valid = AST.CheckSemantics(errors, warnings);
            foreach(var error in errors)
            {
                Console.WriteLine(error.ToString());
            }
            foreach (var warning in warnings)
            {
                Console.WriteLine(warning.ToString());
            }
            if(valid)
                AST.Print(0);
        }


    }
}

using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using System.IO;

namespace BiPaGe.AST
{

    public class ErrorListener : BaseErrorListener, IAntlrErrorListener<int>
    {
        private IList<SemanticAnalysis.Error> Errors;
        private IList<SemanticAnalysis.Warning> Warnings;

        public ErrorListener(IList<SemanticAnalysis.Error> errors, IList<SemanticAnalysis.Warning> warnings)
        {
            this.Errors = errors;
            this.Warnings = warnings;
        }
        public override void SyntaxError(System.IO.TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            this.Errors.Add(new SemanticAnalysis.Error(new AST.SourceInfo(line, charPositionInLine), msg));
        }

        public void SyntaxError(TextWriter output, IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            this.Errors.Add(new SemanticAnalysis.Error(new AST.SourceInfo(line, charPositionInLine), msg));
        }
    }

    public class Builder : BiPaGeBaseVisitor<AST.IASTNode>
    {
        private IList<SemanticAnalysis.Error> errors;
        private IList<SemanticAnalysis.Warning> warnings;

        public Builder(IList<SemanticAnalysis.Error> errors, IList<SemanticAnalysis.Warning> warnings)
        {
            this.errors = errors;
            this.warnings = warnings;
        }

        public Parser Program(String input)
        {
            var inputStream = new AntlrInputStream(input);
            var lexer = new BiPaGeLexer(inputStream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new BiPaGeParser(tokens);

            var errorListener = new ErrorListener(errors, warnings);
            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(errorListener);
            parser.RemoveErrorListeners();
            parser.AddErrorListener(errorListener);
            var program = parser.program();

            var walker = new ParsetreeWalker();
            return walker.Visit(program) as Parser;
        }
    }
}

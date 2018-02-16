using System;
using System.Collections.Generic;

namespace BiPaGe.AST
{
    public class SourceInfo
    {
        public int line { get; }
        public int column { get; }

        public SourceInfo(int line, int column)
        {
            this.line = line;
            this.column = column;
        }
    }

    public abstract class ASTNode : IASTNode
    {
        public abstract void Print(int indentLevel);
        public abstract bool CheckSemantics(IList<SemanticAnalysis.Error> errors, IList<SemanticAnalysis.Warning> warnings);

        public void PrintIndented(string content, int indentLevel)
        {
            var toPrint = "";
            for (var i = 0; i < indentLevel; ++i)
                toPrint += "\t";
            toPrint += content;
            Console.WriteLine(toPrint);
        }

        public abstract void Validate(IASTNode expected);

        public SourceInfo sourceInfo { get; }

        public ASTNode(SourceInfo sourceInfo)
        {
            this.sourceInfo = sourceInfo;
        }
    }

    public interface IASTNode
    {
        void Print(int indentLevel);
        bool CheckSemantics(IList<SemanticAnalysis.Error> errors, IList<SemanticAnalysis.Warning> warnings);
        void PrintIndented(String content, int indentLevel);
        void Validate(IASTNode expected);
    }
}

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

    public abstract class IASTNode
    {
        public abstract void Print(int indentLevel);
        public abstract bool CheckSemantics(IList<SemanticAnalysis.Error> errors, IList<SemanticAnalysis.Warning> warnings);

        public SourceInfo sourceInfo { get; }

        public IASTNode(SourceInfo sourceInfo)
        {
            this.sourceInfo = sourceInfo;
        }

        protected void PrintIndented(String content, int indentLevel)
        {
            var toPrint = "";
            for (var i = 0; i < indentLevel; ++i)
                toPrint += "\t";
            toPrint += content;
            Console.WriteLine(toPrint);
        }
    }
}

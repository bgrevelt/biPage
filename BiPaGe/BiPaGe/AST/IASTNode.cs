using System;
using System.Collections.Generic;

namespace BiPaGe.AST
{
    public abstract class IASTNode
    {
        public abstract void Print(int indentLevel);
        public abstract bool CheckSemantics(IList<String> errors, IList<String> warnings);

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

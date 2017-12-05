using System;
namespace BiPaGe.AST
{
    public abstract class IASTNode
    {
        public abstract void Print(int indentLevel);

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

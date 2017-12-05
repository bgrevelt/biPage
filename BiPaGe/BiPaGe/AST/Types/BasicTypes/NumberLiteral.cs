using System;
namespace BiPaGe.AST.Types.BasicTypes
{
    public class NumberLiteral : IMultiplier
    {
        public String content { get;  }

        public NumberLiteral(String content)
        {
            this.content = content;
        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("Number literal: {0}", content), indentLevel);
        }
    }
}

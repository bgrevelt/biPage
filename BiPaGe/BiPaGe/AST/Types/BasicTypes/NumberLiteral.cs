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
    }
}

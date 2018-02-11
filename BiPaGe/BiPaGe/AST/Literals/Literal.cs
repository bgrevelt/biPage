using System;
using BiPaGe.AST.Constants;

namespace BiPaGe.AST.Literals
{
    public abstract class Literal : ASTNode, Constants.Constant
    {
        protected String value_as_string;
        public Literal(SourceInfo sourceInfo, String value) : base(sourceInfo)
        {
            value_as_string = value;
        }
    }
}

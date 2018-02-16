using System;
using System.Collections.Generic;
using BiPaGe.SemanticAnalysis;
using NUnit.Framework;

namespace BiPaGe.AST.Expressions
{
    public class This : ASTNode, IExpression
    {
        public This(SourceInfo sourceInfo) : base(sourceInfo)
        {
        }

        public override bool CheckSemantics(IList<Error> errors, IList<Warning> warnings)
        {
            throw new NotImplementedException();
        }

        public override void Validate(IASTNode expected)
        {
            Assert.IsInstanceOf<This>(expected);
        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("this"), indentLevel);
        }
    }
}

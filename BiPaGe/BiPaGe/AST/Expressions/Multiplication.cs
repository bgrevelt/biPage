using System;
using System.Collections.Generic;
using BiPaGe.SemanticAnalysis;
using NUnit.Framework;

namespace BiPaGe.AST.Expressions
{
    public class Multiplication : ASTNode, IExpression
    {
        public IExpression Left { get; }
        public IExpression Right { get; }

        public Multiplication(SourceInfo sourceInfo, IExpression lhs, IExpression rhs) : base(sourceInfo)
        {
            this.Left = lhs;
            this.Right = rhs;
        }

        public override bool CheckSemantics(IList<Error> errors, IList<Warning> warnings)
        {
            throw new NotImplementedException();
        }

        public override void Validate(IASTNode expected)
        {
            Assert.IsInstanceOf<Multiplication>(expected);
            this.Left.Validate(((Multiplication)expected).Left);
            this.Right.Validate(((Multiplication)expected).Right);
        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("Multiplication"), indentLevel);
            this.Left.Print(indentLevel + 1);
            this.Right.Print(indentLevel + 1);
        }
    }
}

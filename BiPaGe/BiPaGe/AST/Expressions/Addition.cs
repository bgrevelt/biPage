﻿using System;
using System.Collections.Generic;
using BiPaGe.SemanticAnalysis;
using NUnit.Framework;

namespace BiPaGe.AST.Expressions
{
    public class Addition : ASTNode, IExpression
    {
        public IExpression Left { get; }
        public IExpression Right { get; }

        public Addition(SourceInfo sourceInfo, IExpression lhs, IExpression rhs) : base(sourceInfo)
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
            Assert.IsInstanceOf<Addition>(expected);
            this.Left.Validate(((Addition)expected).Left);
            this.Right.Validate(((Addition)expected).Right);

        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("Addition"), indentLevel);
            this.Left.Print(indentLevel+1);
            this.Right.Print(indentLevel+1);
        }
    }
}

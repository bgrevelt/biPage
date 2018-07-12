using System;
using System.Collections.Generic;
using BiPaGe.SemanticAnalysis;
using NUnit.Framework;

namespace BiPaGe.Model.Expressions
{
    public class Subtraction : Expression
    {
        public Expression Left { get; }
        public Expression Right { get; }

        public Subtraction(Expression lhs, Expression rhs)
        {
            this.Left = lhs;
            this.Right = rhs;
        }
    }
}

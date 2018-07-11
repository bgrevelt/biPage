using System;
using System.Collections.Generic;
using BiPaGe.SemanticAnalysis;
using NUnit.Framework;

namespace BiPaGe.Model.Expressions
{
    public class Subtraction : Expression
    {
        public Expression left { get; }
        public Expression right { get; }

        public Subtraction(Expression lhs, Expression rhs)
        {
            left = lhs;
            right = rhs;
        }
    }
}

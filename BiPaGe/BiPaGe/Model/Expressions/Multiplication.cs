using System;
using System.Collections.Generic;
using BiPaGe.SemanticAnalysis;
using NUnit.Framework;

namespace BiPaGe.Model.Expressions
{
    public class Multiplication : Expression
    {
        public Expression left { get; }
        public Expression right { get; }

        public Multiplication(Expression lhs, Expression rhs)
        {
            left = lhs;
            right = rhs;
        }
    }
}

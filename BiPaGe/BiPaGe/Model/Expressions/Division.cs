using System;
using System.Collections.Generic;
using BiPaGe.SemanticAnalysis;
using NUnit.Framework;

namespace BiPaGe.Model.Expressions
{
    public class Division : Expression
    {
        public Expression Left { get; }
        public Expression Right { get; }

        public Division(Expression lhs, Expression rhs)
        {
            this.Left = lhs;
            this.Right = rhs;
        }
    }
}

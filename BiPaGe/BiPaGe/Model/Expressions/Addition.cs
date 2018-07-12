using System;
using System.Collections.Generic;
using BiPaGe.SemanticAnalysis;
using NUnit.Framework;

namespace BiPaGe.Model.Expressions
{
    public class Addition : Expression
    {
        public Expression Left { get; }
        public Expression Right { get; }

        public Addition(Expression lhs, Expression rhs) 
        {
            this.Left = lhs;
            this.Right = rhs;
        }
    }
}

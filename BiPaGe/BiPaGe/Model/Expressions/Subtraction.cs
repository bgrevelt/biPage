using System;
using System.Collections.Generic;
using BiPaGe.SemanticAnalysis;
using NUnit.Framework;

namespace BiPaGe.Model.Expressions
{
    public class Subtraction : BinaryExpression
    {
        public Subtraction(Expression lhs, Expression rhs)
        {
            this.left = lhs;
            this.right = rhs;
        }
    }
}

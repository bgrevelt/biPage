using System;
using System.Collections.Generic;
using BiPaGe.SemanticAnalysis;
using NUnit.Framework;

namespace BiPaGe.Model.Expressions
{
    public class Division : BinaryExpression
    {
        public Division(Expression lhs, Expression rhs)
        {
            this.left = lhs;
            this.right = rhs;
        }
    }
}

using System;
using System.Collections.Generic;
using BiPaGe.SemanticAnalysis;
using NUnit.Framework;

namespace BiPaGe.Model.Expressions
{
    public class Addition : BinaryExpression
    {
        public Addition(Expression lhs, Expression rhs) 
        {
            this.left = lhs;
            this.right = rhs;
        }
    }
}

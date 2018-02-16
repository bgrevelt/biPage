﻿using System;
using System.Collections.Generic;
using BiPaGe.SemanticAnalysis;

namespace BiPaGe.AST.Expressions
{
    public class Subtraction : ASTNode, IExpression
    {
        public IExpression left { get; }
        public IExpression right { get; }

        public Subtraction(SourceInfo sourceInfo, IExpression lhs, IExpression rhs) : base(sourceInfo)
        {
            left = lhs;
            right = rhs;
        }

        public override bool CheckSemantics(IList<Error> errors, IList<Warning> warnings)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(IASTNode other)
        {
            try
            {
                return left.Equals(((Subtraction)other).left) && right.Equals(((Subtraction)other).right);
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("Subtraction"), indentLevel);
            left.Print(indentLevel + 1);
            right.Print(indentLevel + 1);
        }
    }
}
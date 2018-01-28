﻿using System;
namespace BiPaGe.AST.Literals
{
    public abstract class Literal : IASTNode, Constants.Value
    {
        protected String value_as_string;
        public Literal(SourceInfo sourceInfo, String value) : base(sourceInfo)
        {
            value_as_string = value;
        }

        public abstract bool Equals(Literal other);
    }
}

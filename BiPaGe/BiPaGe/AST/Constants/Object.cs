using System;
using System.Collections.Generic;

namespace BiPaGe.AST.Constants
{
    public abstract class Object : ASTNode
    {
        public List<ObjectField> FieldFixers { get; }
        public Object(SourceInfo sourceInfo, List<ObjectField> field_fixers) : base(sourceInfo)
        {
            this.FieldFixers = field_fixers;
        }
    }
}

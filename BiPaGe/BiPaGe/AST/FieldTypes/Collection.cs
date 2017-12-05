﻿using System;
using System.Collections.Generic;

namespace BiPaGe.AST.FieldTypes
{
    public class Collection : AST.FieldType
    {
        public FieldType Type { get; }
        public IMultiplier Size { get; } 
        
        public Collection(FieldType type, IMultiplier multiplier)
        {
            this.Type = type;
            this.Size = multiplier;
        }

        public override void Print(int indentLevel)
        {
            PrintIndented("collection", indentLevel);
            Type.Print(indentLevel + 1);
            Size.Print(indentLevel + 1);
        }

        public override bool CheckSemantics(IList<String> errors, IList<String> warnings)
        {
            bool type_valid = Type.CheckSemantics(errors, warnings);
            bool size_valid = Size.CheckSemantics(errors, warnings);
            return type_valid && size_valid;
        }
    }
}
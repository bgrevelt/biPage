﻿using System;
using System.Collections.Generic;
using BiPaGe.AST.Expressions;
namespace BiPaGe.AST
{
    public class Field : IASTNode
    {
        public String Name { get; }
        public AST.FieldType Type { get; }
        public Expression CollectionSize { get; }
        public Constants.Fixer Fixer { get; }

        public Field(SourceInfo sourceIfo, String name, AST.FieldType type, Expression collection_size, Constants.Fixer fixer) : base(sourceIfo)
        {
            this.Name = name;
            this.Type = type;
            this.CollectionSize = collection_size;
            this.Fixer = fixer;
        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("Field {0}: ", Name), indentLevel);
            Type.Print(indentLevel + 1);
            CollectionSize?.Print((indentLevel + 1));
            Fixer?.Print(indentLevel + 1);
        }

        public override bool CheckSemantics(IList<SemanticAnalysis.Error> errors, IList<SemanticAnalysis.Warning> warnings)
        {
            return Type.CheckSemantics(errors, warnings);
        }
    }
}

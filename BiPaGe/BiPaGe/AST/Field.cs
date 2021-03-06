﻿using System;
using System.Collections.Generic;
using BiPaGe.AST.Expressions;
using NUnit.Framework;

namespace BiPaGe.AST
{
    public class Field : ASTNode
    {
        public String Name { get; }
        public FieldType Type { get; }
        public IExpression CollectionSize { get; }
        public Constants.IFixer Fixer { get; }

        public Field(SourceInfo sourceIfo, String name, AST.FieldType type, IExpression collection_size, Constants.IFixer fixer) : base(sourceIfo)
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

        public override void Validate(IASTNode expected)
        {
            Assert.IsInstanceOf<Field>(expected);
            var expected_field = expected as Field;
            Assert.AreEqual(expected_field.Name, this.Name);

            this.Type.Validate(expected_field.Type);
            this.CollectionSize?.Validate(expected_field.CollectionSize);
            this.Fixer?.Validate(expected_field.Fixer);
        }
    }
}

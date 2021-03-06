﻿using System;
using System.Collections.Generic;
using BiPaGe.SemanticAnalysis;
using System.Linq;
using NUnit.Framework;

namespace BiPaGe.AST.FieldTypes
{
    public class InlineEnumeration : FieldType
    {
        public IList<Enumerator> Enumerators { get; }
        public FieldType Type { get; } 
        public InlineEnumeration(SourceInfo sourceInfo, FieldType type, IList<Enumerator> enumerators) : base(sourceInfo)
        {
            this.Enumerators = enumerators;
            this.Type = type;
        }

        public override bool CheckSemantics(IList<Error> errors, IList<Warning> warnings)
        {
            throw new NotImplementedException();
        }

        public override void Print(int indentLevel)
        {
            PrintIndented("InlineEnumeration", indentLevel);
            Type.Print(indentLevel + 1);
            foreach (var enumerator in Enumerators)
            {
                enumerator.Print(indentLevel + 1);
            }
        }

        public override void Validate(IASTNode expected)
        {
            Assert.IsInstanceOf<InlineEnumeration>(expected);
            var expected_enum = expected as InlineEnumeration;

            this.Type.Validate(expected_enum.Type);
            Assert.AreEqual(expected_enum.Enumerators.Count, this.Enumerators.Count());
            for (int i = 0; i < this.Enumerators.Count; ++i)
                this.Enumerators[i].Validate(expected_enum.Enumerators[i]);
        }

        public override void Accept(IFieldTypeVisitor v)
        {
            v.Visit(this);
        }
    }
}

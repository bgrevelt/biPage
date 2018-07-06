﻿using System;
using System.Collections.Generic;
using BiPaGe.AST.Constants;
using NUnit.Framework;

namespace BiPaGe.AST.Identifiers
{
    public class Identifier : AST.FieldType, Constants.Constant
    {
        public String Id { get; }
        public Identifier(SourceInfo sourceIfo, String id) : base(sourceIfo)
        {
            this.Id = id;
        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("Object id: {0}", Id), indentLevel);
        }

        public override bool CheckSemantics(IList<SemanticAnalysis.Error> errors, IList<SemanticAnalysis.Warning> warnings)
        {
            // TODO: we should pass in a list of know objects and their fields
            return true;
        }

        public override void Validate(IASTNode expected)
        {
            Assert.IsInstanceOf<Identifier>(expected);
            Assert.AreEqual(((Identifier)expected).Id, this.Id);
        }

        public override uint SizeInBits()
        {
            // TODO: well this makes no sense at all....
            throw new NotImplementedException();
        }
    }
}

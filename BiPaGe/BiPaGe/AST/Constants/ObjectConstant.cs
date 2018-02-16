using System;
using System.Collections.Generic;
using BiPaGe.SemanticAnalysis;
using NUnit.Framework;

namespace BiPaGe.AST.Constants
{
    public class ObjectConstant : ASTNode, IFixer
    {
        public List<ObjectField> FieldFixers { get; }
        public ObjectConstant(SourceInfo sourceInfo, List<ObjectField> field_fixers) : base(sourceInfo)
        {
            this.FieldFixers = field_fixers;
        }

        public override void Print(int indentLevel)
        {
            PrintIndented("ObjectConstant:", indentLevel);
            foreach (var ff in FieldFixers)
                PrintIndented(String.Format("{0}: {1}", ff.FieldId, ff.Value), indentLevel + 1);
        }

        public override bool CheckSemantics(IList<Error> errors, IList<Warning> warnings)
        {
            throw new NotImplementedException();
        }

        public override void Validate(IASTNode expected)
        {
            Assert.IsInstanceOf<ObjectConstant>(expected);
            var expected_oc = expected as ObjectConstant;
            Assert.AreEqual(expected_oc.FieldFixers.Count, this.FieldFixers.Count);

            for (int i = 0; i < this.FieldFixers.Count; ++i)
            {
                var this_field = this.FieldFixers[i];
                var expected_field = expected_oc.FieldFixers[i];
                Assert.AreSame(expected_field.FieldId, this_field.FieldId);
                this_field.Value.Validate(expected_field.Value);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace BiPaGe.AST.FieldTypes
{
    public class InlineObject : FieldType
    {
        public IList<Field> Fields
        {
            get;
        }

        public InlineObject(SourceInfo sourceInfo, IList<Field> fields) : base(sourceInfo)
        {
            this.Fields = fields;
        }

        public override void Print(int indentLevel)
        {
            PrintIndented("InlineObject", indentLevel);

            foreach (var field in Fields)
            {
                field.Print(indentLevel + 1);
            }
        }

        public override bool CheckSemantics(IList<SemanticAnalysis.Error> errors, IList<SemanticAnalysis.Warning> warnings)
        {
            throw new NotImplementedException();
        }

        public override void Validate(IASTNode expected)
        {
            Assert.IsInstanceOf<InlineObject>(expected);
            var expected_object = expected as InlineObject;
            Assert.AreEqual(expected_object.Fields.Count, this.Fields.Count);
            for (int i = 0; i < this.Fields.Count; ++i)
                this.Fields[i].Validate(expected_object.Fields[i]);
        }
    }
}

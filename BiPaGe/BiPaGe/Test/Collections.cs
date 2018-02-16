using NUnit.Framework;
using System;
using System.Collections.Generic;
using BiPaGe.AST;
using BiPaGe.AST.FieldTypes;
using BiPaGe.AST.Identifiers;
using BiPaGe.AST.Expressions;
using BiPaGe.AST.Constants;
using System.Linq;
using Helpers;

namespace BiPaGe.Test.AST.Collections
{
    [TestFixture()]
    public class Creation
    {
        [Test()]
        public void StaticallySizedCollection()
        {
            var input = @"
Object1
{
    field1 : int32[5];
    field2 : ascii_string[32];
    field3 : utf8_string[255];
}";
            var expected = new ProgramBuilder();
            var object1 = expected.AddObject();
            object1.Name = "Object1";
            object1.Fields.Add(("field1", new Signed(null, 32), new BiPaGe.AST.Literals.Integer(null, "5"), null));
            object1.Fields.Add(("field2", new AsciiString(null), new BiPaGe.AST.Literals.Integer(null, "32"), null));
            object1.Fields.Add(("field3", new Utf8String(null), new BiPaGe.AST.Literals.Integer(null, "255"), null));
            expected.Validate(SimpleBuilder.Build(input));
        }

        [Test()]
        public void CollectionSizedByField()
        {
            var input = @"
Object1
{
    size : int32;
    field2 : ascii_string[size];
}";
            var expected = new ProgramBuilder();
            var object1 = expected.AddObject();
            object1.Name = "Object1";
            object1.Fields.Add(("size", new Signed(null, 32), null, null));
            object1.Fields.Add(("field2", new AsciiString(null), new BiPaGe.AST.Identifiers.FieldIdentifier(null, "size"), null));
            expected.Validate(SimpleBuilder.Build(input));
        }

        [Test()]
        public void CollectionSizedByExpression()
        {
            var input = @"
Object1
{
    size : int32;
    field2 : u32[(size - this) / 4];
}";
            var expected = new ProgramBuilder();
            var object1 = expected.AddObject();
            object1.Name = "Object1";
            object1.Fields.Add(("size", new Signed(null, 32), null, null));
            var field2_size = new Division(
                null,
                new Subtraction(
                    null,
                    new FieldIdentifier(null, "size"),
                    new This(null)
                ),
                new BiPaGe.AST.Literals.Integer(null, "4"));
            object1.Fields.Add(("field2", new Unsigned(null, 32), field2_size, null));
            expected.Validate(SimpleBuilder.Build(input));
        }

        [Test()]
        public void CollectionSizedByExpression2()
        {
            var input = @"
Object1
{
    size : int32;
    size2 : int16;
    collection : bool[size + size2 - 10 * 5];
}";

            var expected = new ProgramBuilder();
            var object1 = expected.AddObject();
            object1.Name = "Object1";
            object1.Fields.Add(("size", new Signed(null, 32), null, null));
            object1.Fields.Add(("size2", new Signed(null, 16), null, null));
            var field3_size = new Subtraction(
                null,
                new Addition(
                    null,
                    new FieldIdentifier(null, "size"),
                    new FieldIdentifier(null, "size2")
                ),
                new Multiplication(
                    null,
                    new BiPaGe.AST.Literals.Integer(null, "10"),
                    new BiPaGe.AST.Literals.Integer(null, "5")
                ));
            object1.Fields.Add(("collection", new BiPaGe.AST.FieldTypes.Boolean(null), field3_size, null));
            expected.Validate(SimpleBuilder.Build(input));
        }
    }
}

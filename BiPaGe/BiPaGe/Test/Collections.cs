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

        [Test()]
        public void Reserved()
        {
            var input = @"
Object1
{
    int32[5];
    ascii_string[32];
    utf8_string[255];
    ascii_string[size];
    u32[(size - this) / 4];
    bool[size + size2 - 10 * 5];
}";
            var expected = new ProgramBuilder();
            var object1 = expected.AddObject();
            object1.Name = "Object1";
            object1.Fields.Add((null, new Signed(null, 32), new BiPaGe.AST.Literals.Integer(null, "5"), null));
            object1.Fields.Add((null, new AsciiString(null), new BiPaGe.AST.Literals.Integer(null, "32"), null));
            object1.Fields.Add((null, new Utf8String(null), new BiPaGe.AST.Literals.Integer(null, "255"), null));
            object1.Fields.Add((null, new AsciiString(null), new BiPaGe.AST.Identifiers.FieldIdentifier(null, "size"), null));
            var field5_size = new Division(
                null,
                new Subtraction(
                    null,
                    new FieldIdentifier(null, "size"),
                    new This(null)
                ),
                new BiPaGe.AST.Literals.Integer(null, "4"));
            object1.Fields.Add((null, new Unsigned(null, 32), field5_size, null));
            var field6_size = new Subtraction(
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
            object1.Fields.Add((null, new BiPaGe.AST.FieldTypes.Boolean(null), field6_size, null));
            expected.Validate(SimpleBuilder.Build(input));
        }
    }

    [TestFixture()]
    public class FixedValues
    {
        [Test()]
        public void ListOfPositiveIntegers()
        {
            var input = @"
Object1
{
    field1 : int32[5] = {1,2,3,4,5};
}";
            var expected = new ProgramBuilder();
            var object1 = expected.AddObject();
            object1.Name = "Object1";
            object1.Fields.Add(("field1", new Signed(null, 32), new BiPaGe.AST.Literals.Integer(null, "5"), 
                                new LiteralCollection(null,
                                    new List<BiPaGe.AST.Literals.Literal>() {
                                        new BiPaGe.AST.Literals.Integer(null, "1"),
                                        new BiPaGe.AST.Literals.Integer(null, "2"),
                                        new BiPaGe.AST.Literals.Integer(null, "3"),
                                        new BiPaGe.AST.Literals.Integer(null, "4"),
                                        new BiPaGe.AST.Literals.Integer(null, "5")
            })));
            expected.Validate(SimpleBuilder.Build(input));
        }

        [Test()]
        public void ListOfNegativeIntegers()
        {
            var input = @"
Object1
{
    field1 : int32[5] = {-1,-2,-3,-4,-5};
}";
            var expected = new ProgramBuilder();
            var object1 = expected.AddObject();
            object1.Name = "Object1";
            object1.Fields.Add(("field1", new Signed(null, 32), new BiPaGe.AST.Literals.Integer(null, "5"),
                                new LiteralCollection(null,
                                    new List<BiPaGe.AST.Literals.Literal>() {
                                        new BiPaGe.AST.Literals.Integer(null, "-1"),
                                        new BiPaGe.AST.Literals.Integer(null, "-2"),
                                        new BiPaGe.AST.Literals.Integer(null, "-3"),
                                        new BiPaGe.AST.Literals.Integer(null, "-4"),
                                        new BiPaGe.AST.Literals.Integer(null, "-5")
            })));
            expected.Validate(SimpleBuilder.Build(input));
        }

        [Test()]
        public void ListOfMixedSignIntegers()
        {
            var input = @"
Object1
{
    field1 : int32[5] = {1,-2,3,-4,5};
}";
            var expected = new ProgramBuilder();
            var object1 = expected.AddObject();
            object1.Name = "Object1";
            object1.Fields.Add(("field1", new Signed(null, 32), new BiPaGe.AST.Literals.Integer(null, "5"),
                                new LiteralCollection(null,
                                    new List<BiPaGe.AST.Literals.Literal>() {
                                        new BiPaGe.AST.Literals.Integer(null, "1"),
                                        new BiPaGe.AST.Literals.Integer(null, "-2"),
                                        new BiPaGe.AST.Literals.Integer(null, "3"),
                                        new BiPaGe.AST.Literals.Integer(null, "-4"),
                                        new BiPaGe.AST.Literals.Integer(null, "5")
            })));
            expected.Validate(SimpleBuilder.Build(input));
        }

        [Test()]
        public void ListOfIntegersSizedByExpression2()
        {
            var input = @"
Object1
{
    size : int32;
    size2 : int16;
    collection : i32[size + size2 - 10 * 5] = {1,1,2,3,5,8,13,21,34,55};
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
            object1.Fields.Add(("collection", new BiPaGe.AST.FieldTypes.Signed(null,32), field3_size, 
                                new LiteralCollection(null,
                                    new List<BiPaGe.AST.Literals.Literal>() {
                                        new BiPaGe.AST.Literals.Integer(null, "1"),
                                        new BiPaGe.AST.Literals.Integer(null, "1"),
                                        new BiPaGe.AST.Literals.Integer(null, "2"),
                                        new BiPaGe.AST.Literals.Integer(null, "3"),
                                        new BiPaGe.AST.Literals.Integer(null, "5"),
                                        new BiPaGe.AST.Literals.Integer(null, "8"),
                                        new BiPaGe.AST.Literals.Integer(null, "13"),
                                        new BiPaGe.AST.Literals.Integer(null, "21"),
                                        new BiPaGe.AST.Literals.Integer(null, "34"),
                                        new BiPaGe.AST.Literals.Integer(null, "55")
            })));
            expected.Validate(SimpleBuilder.Build(input));
        }

        [Test()]
        public void ListOfBooleans()
        {
            var input = @"
Object1
{
    field1 : bool[3] = {true, false, true};
}";
            var expected = new ProgramBuilder();
            var object1 = expected.AddObject();
            object1.Name = "Object1";
            object1.Fields.Add(("field1", new BiPaGe.AST.FieldTypes.Boolean(null), new BiPaGe.AST.Literals.Integer(null, "3"),
                                new LiteralCollection(null,
                                    new List<BiPaGe.AST.Literals.Literal>() {
                                        new BiPaGe.AST.Literals.Boolean(null, "true"),
                                        new BiPaGe.AST.Literals.Boolean(null, "false"),
                                        new BiPaGe.AST.Literals.Boolean(null, "true")
            })));
            expected.Validate(SimpleBuilder.Build(input));
        }

        [Test()]
        public void AsciiString()
        {
            var input = @"
Object1
{
    field1 : ascii_string[64] = ""This is a string initializer"";
}";
            var expected = new ProgramBuilder();
            var object1 = expected.AddObject();
            object1.Name = "Object1";
            object1.Fields.Add(("field1", new AsciiString(null), new BiPaGe.AST.Literals.Integer(null, "64"), new BiPaGe.AST.Literals.StringLiteral(null, "This is a string initializer")));
            expected.Validate(SimpleBuilder.Build(input));
        }

        // TODO: add test for utf-8 string once we have a little more of a grasp on what we want to do with that.


    }
}

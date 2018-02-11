using NUnit.Framework;
using System;
using System.Collections.Generic;
using BiPaGe.AST;
using BiPaGe.AST.FieldTypes;
using BiPaGe.AST.Identifiers;
using BiPaGe.AST.Expressions;
using BiPaGe.AST.Constants;
using System.Linq;

namespace BiPaGe.Test.AST
{
    [TestFixture()]
    public class Building
    {
        public void CheckField(BiPaGe.AST.Field field, String name, FieldType type, IExpression collection_size = null, IFixer fixer = null)
        {
            Assert.AreEqual(field.Name, name);
            Assert.IsTrue(field.Type.Equals(type));
            if (collection_size != null)
            {
                Assert.IsTrue(field.CollectionSize.Equals(collection_size));
            }
            if(fixer != null)
            {
                //Assert.
            }
        }

        private BiPaGe.AST.Object CreateObject(String name, (String, FieldType, IExpression, IFixer)[] fields)
        {
            return new BiPaGe.AST.Object(null, name, fields.Select(f => new Field(null, f.Item1, f.Item2, f.Item3, f.Item4)).ToList());
        }

        //private void CheckObject(String name, (String, FieldType, IExpression, IFixer)[] fields, BiPaGe.AST.Object obj)
        //{
        //    var expected_fields = new List<Field>();
        //    foreach(var field in fields)
        //        expected_fields.Add(new Field(null, field.Item1, field.Item2, field.Item3, field.Item4));
        //    Assert.IsTrue(obj.Equals(new BiPaGe.AST.Object(null, name, expected_fields)));
        //}

        private Parser Build(String input)
        {
            var errors = new List<SemanticAnalysis.Error>();
            var warnings = new List<SemanticAnalysis.Warning>();
            var builder = new BiPaGe.AST.Builder(errors, warnings);
            return builder.Program(input);
        }



        [Test()]
        public void BasicTypes()
        {
            var input = @"
Object1
{
    field1 : int16;
    field2 : u32;
    field3 : float32;
    field4 : f64;
    field5 : bool;
}";
            
            var AST = Build(input);
            // Assert.AreEqual(AST.Name, ""); TODO: the program should have a name. Maybe the file name. Otherwise add a field?
            Assert.AreEqual(AST.Elements.Count, 1);
            Assert.IsTrue(AST.Elements[0].GetType() == typeof(BiPaGe.AST.Object));

            Assert.IsTrue(CreateObject("Object1", new(String, FieldType, IExpression, IFixer)[]
            {
                ("field1", new Signed(null, 16), null, null),
                ("field2", new Unsigned(null, 32), null, null),
                ("field3", new Float(null, 32), null, null),
                ("field4", new Float(null, 64), null, null),
                ("field5", new BiPaGe.AST.FieldTypes.Boolean(null),null, null)
            }).Equals((BiPaGe.AST.Object)AST.Elements[0]));
        }

        [Test()]
        public void OddWidths()
        {
            var input = @"
Object1
{
    field1 : int2;
    field2 : u6;
    field3 : i11;
    field4 : u12;
    field5 : bool;
}";
            var AST = Build(input);
            // Assert.AreEqual(AST.Name, ""); TODO: the program should have a name. Maybe the file name. Otherwise add a field?
            Assert.AreEqual(AST.Elements.Count, 1);
            Assert.IsTrue(AST.Elements[0].GetType() == typeof(BiPaGe.AST.Object));

            Assert.IsTrue(CreateObject("Object1", new(String, FieldType, IExpression, IFixer)[]
            {
                ("field1", new Signed(null, 2), null, null),
                ("field2", new Unsigned(null, 6), null, null),
                ("field3", new Signed(null, 11), null, null),
                ("field4", new Unsigned(null, 12), null, null),
                ("field5", new BiPaGe.AST.FieldTypes.Boolean(null),null, null)
            }).Equals((BiPaGe.AST.Object)AST.Elements[0]));
        }

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
            var AST = Build(input);
            // Assert.AreEqual(AST.Name, ""); TODO: the program should have a name. Maybe the file name. Otherwise add a field?
            Assert.AreEqual(AST.Elements.Count, 1);
            Assert.IsTrue(AST.Elements[0].GetType() == typeof(BiPaGe.AST.Object));

            Assert.IsTrue(CreateObject("Object1", new(String, FieldType, IExpression, IFixer)[]
            {
                ("field1", new Signed(null, 32), new BiPaGe.AST.Literals.Integer(null, "5"), null),
                ("field2", new AsciiString(null), new BiPaGe.AST.Literals.Integer(null, "32"), null),
                ("field3", new Utf8String(null), new BiPaGe.AST.Literals.Integer(null, "255"), null)
            }).Equals((BiPaGe.AST.Object)AST.Elements[0]));
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
            var AST = Build(input);
            // Assert.AreEqual(AST.Name, ""); TODO: the program should have a name. Maybe the file name. Otherwise add a field?
            Assert.AreEqual(AST.Elements.Count, 1);
            Assert.IsTrue(AST.Elements[0].GetType() == typeof(BiPaGe.AST.Object));

            Assert.IsTrue(CreateObject("Object1", new(String, FieldType, IExpression, IFixer)[]
            {
                ("size", new Signed(null, 32), null, null),
                ("field2", new AsciiString(null), new BiPaGe.AST.Identifiers.FieldIdentifier(null, "size"), null)
            }).Equals((BiPaGe.AST.Object)AST.Elements[0]));
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
            var AST = Build(input);
            // Assert.AreEqual(AST.Name, ""); TODO: the program should have a name. Maybe the file name. Otherwise add a field?
            Assert.AreEqual(AST.Elements.Count, 1);
            Assert.IsTrue(AST.Elements[0].GetType() == typeof(BiPaGe.AST.Object));

            var expected_size = new Division(
                null,
                new Subtraction(
                    null,
                    new FieldIdentifier(null, "size"),
                    new This(null)
                ),
                new BiPaGe.AST.Literals.Integer(null, "4"));

            Assert.IsTrue(CreateObject("Object1", new(String, FieldType, IExpression, IFixer)[]
            {
                ("size", new Signed(null, 32), null, null),
                ("field2", new Unsigned(null, 32), expected_size, null)
            }).Equals((BiPaGe.AST.Object)AST.Elements[0]));
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
            var AST = Build(input);
            // Assert.AreEqual(AST.Name, ""); TODO: the program should have a name. Maybe the file name. Otherwise add a field?
            Assert.AreEqual(AST.Elements.Count, 1);
            Assert.IsTrue(AST.Elements[0].GetType() == typeof(BiPaGe.AST.Object));

            var expected_size = new Subtraction(
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

            Assert.IsTrue(CreateObject("Object1", new(String, FieldType, IExpression, IFixer)[]
            {
                ("size", new Signed(null, 32), null, null),
                ("size2", new Signed(null, 16), null, null),
                ("collection", new BiPaGe.AST.FieldTypes.Boolean(null), expected_size, null)
            }).Equals((BiPaGe.AST.Object)AST.Elements[0]));
        }

        [Test()]
        public void Enumeration()
        {
            var input = @"
SomeEnumeration : u8
{
    value1 = 1,
    value2 = 2,
    value3 = 0
}
Object1
{
    enum_field : SomeEnumeration;
}";
            var AST = Build(input);
            // Assert.AreEqual(AST.Name, ""); TODO: the program should have a name. Maybe the file name. Otherwise add a field?
            Assert.AreEqual(AST.Elements.Count, 2);
            Assert.IsTrue(AST.Elements[0].GetType() == typeof(BiPaGe.AST.Enumeration));
            Assert.IsTrue(AST.Elements[1].GetType() == typeof(BiPaGe.AST.Object));
            var enumeration = (Enumeration)AST.Elements[0];
            Assert.AreEqual(3, enumeration.Enumerators.Count);

            Assert.AreEqual("value1", enumeration.Enumerators[0].Name);
            Assert.AreEqual("1", enumeration.Enumerators[0].Value);

            Assert.AreEqual("value2", enumeration.Enumerators[1].Name);
            Assert.AreEqual("2", enumeration.Enumerators[1].Value);

            Assert.AreEqual("value3", enumeration.Enumerators[2].Name);
            Assert.AreEqual("0", enumeration.Enumerators[2].Value);

            var obj = (BiPaGe.AST.Object)AST.Elements[1];
            Assert.AreEqual(obj.identifier, "Object1");
            Assert.AreEqual(obj.fields.Count, 1);
            CheckField(obj.fields[0], "enum_field", new Identifier(null, "SomeEnumeration"), null);
        }
    }
}
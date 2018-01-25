using NUnit.Framework;
using System;
using System.Collections.Generic;
using BiPaGe.AST;
using BiPaGe.AST.FieldTypes;
using BiPaGe.AST.Identifiers;
using BiPaGe.AST.Expressions;

namespace BiPaGe.Test.AST
{
    [TestFixture()]
    public class Building
    {
        public void CheckField(BiPaGe.AST.Field field, String name, FieldType type, Expression collection_size = null)
        {
            Assert.AreEqual(field.Name, name);
            Assert.IsTrue(field.Type.Equals(type));
            if (collection_size != null)
            {
                Assert.IsTrue(field.CollectionSize.Equals(collection_size));
            }
        }

        private void CheckObject(String name, (String, FieldType, Expression)[] fields, BiPaGe.AST.Object obj)
        {
            Assert.AreEqual(name, obj.identifier);
            Assert.AreEqual(fields.Length, obj.fields.Count);
            for (int i = 0; i < fields.Length; ++i)
                CheckField(obj.fields[i], fields[i].Item1, fields[i].Item2, fields[i].Item3);
        }

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

            CheckObject("Object1", new(String, FieldType, Expression)[]
            {
                ("field1", new Signed(null, 16), null),
                ("field2", new Unsigned(null, 32), null),
                ("field3", new Float(null, 32), null),
                ("field4", new Float(null, 64), null),
                ("field5", new BiPaGe.AST.FieldTypes.Boolean(null),null)
            }, (BiPaGe.AST.Object)AST.Elements[0]);
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

            CheckObject("Object1", new(String, FieldType, Expression)[]
            {
                ("field1", new Signed(null, 2), null),
                ("field2", new Unsigned(null, 6), null),
                ("field3", new Signed(null, 11), null),
                ("field4", new Unsigned(null, 12), null),
                ("field5", new BiPaGe.AST.FieldTypes.Boolean(null),null)
            }, (BiPaGe.AST.Object)AST.Elements[0]);
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

            CheckObject("Object1", new(String, FieldType, Expression)[]
            {
                ("field1", new Signed(null, 32), new Number(null, "5")),
                ("field2", new AsciiString(null), new Number(null, "32")),
                ("field3", new Utf8String(null), new Number(null, "255"))
            }, (BiPaGe.AST.Object)AST.Elements[0]);
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

            CheckObject("Object1", new(String, FieldType, Expression)[]
            {
                ("size", new Signed(null, 32), null),
                ("field2", new AsciiString(null), new BiPaGe.AST.Identifiers.FieldIdentifier(null, "size"))
            }, (BiPaGe.AST.Object)AST.Elements[0]);


        }

        // TODO: collection sized by expression

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
            CheckField(obj.fields[0], "enum_field", new ObjectIdentifier(null, "SomeEnumeration"), null);
        }
    }
}
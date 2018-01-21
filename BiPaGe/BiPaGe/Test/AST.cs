using NUnit.Framework;
using System;
using System.Collections.Generic;
using BiPaGe.AST;
using BiPaGe.AST.FieldTypes;

namespace BiPaGe.Test.AST
{
    [TestFixture()]
    public class Building
    {
        public void CheckField(BiPaGe.AST.Field field, String name, FieldType type, IMultiplier collection_size = null)
        {
            Assert.AreEqual(field.Name, name);
            Assert.IsTrue(field.Type.Equals(type));
            if (collection_size != null)
            {
                Assert.IsTrue(field.CollectionSize.Equals(collection_size));
            }
        }

     

        [Test()]
        public void BasicTypes()
        {
            var errors = new List<SemanticAnalysis.Error>();
            var warnings = new List<SemanticAnalysis.Warning>();
            var builder = new BiPaGe.AST.Builder(errors, warnings);
            var input = @"
Object1
{
    field1 : int16;
    field2 : u32;
    field3 : float32;
    field4 : f64;
    field5 : bool;
}";
            var AST = builder.Program(input);
            // Assert.AreEqual(AST.Name, ""); TODO: the program should have a name. Maybe the file name. Otherwise add a field?
            Assert.AreEqual(AST.Elements.Count, 1);
            Assert.IsTrue(AST.Elements[0].GetType() == typeof(BiPaGe.AST.Object));
            var obj = (BiPaGe.AST.Object)AST.Elements[0];
            Assert.AreEqual(obj.identifier, "Object1");
            Assert.AreEqual(obj.fields.Count, 5);

            CheckField(obj.fields[0], "field1", new Signed(null, 16), null);
            CheckField(obj.fields[1], "field2", new Unsigned(null, 32), null);
            CheckField(obj.fields[2], "field3", new Float(null, 32), null);
            CheckField(obj.fields[3], "field4", new Float(null, 64), null);
            CheckField(obj.fields[4], "field5", new BiPaGe.AST.FieldTypes.Boolean(null));
        }

        [Test()]
        public void OddWidths()
        {
            var errors = new List<SemanticAnalysis.Error>();
            var warnings = new List<SemanticAnalysis.Warning>();
            var builder = new BiPaGe.AST.Builder(errors, warnings);
            var input = @"
Object1
{
    field1 : int2;
    field2 : u6;
    field3 : i11;
    field4 : u12;
    field5 : bool;
}";
            var AST = builder.Program(input);
            // Assert.AreEqual(AST.Name, ""); TODO: the program should have a name. Maybe the file name. Otherwise add a field?
            Assert.AreEqual(AST.Elements.Count, 1);
            Assert.IsTrue(AST.Elements[0].GetType() == typeof(BiPaGe.AST.Object));
            var obj = (BiPaGe.AST.Object)AST.Elements[0];
            Assert.AreEqual(obj.identifier, "Object1");
            Assert.AreEqual(obj.fields.Count, 5);

            CheckField(obj.fields[0], "field1", new Signed(null, 2), null);
            CheckField(obj.fields[1], "field2", new Unsigned(null, 6), null);
            CheckField(obj.fields[2], "field3", new Signed(null, 11), null);
            CheckField(obj.fields[3], "field4", new Unsigned(null, 12), null);
            CheckField(obj.fields[4], "field5", new BiPaGe.AST.FieldTypes.Boolean(null));
        }

        [Test()]
        public void StaticallySizedCollection()
        {
            var errors = new List<SemanticAnalysis.Error>();
            var warnings = new List<SemanticAnalysis.Warning>();
            var builder = new BiPaGe.AST.Builder(errors, warnings);
            var input = @"
Object1
{
    field1 : int32[5];
    field2 : ascii_string[32];
    field3 : utf8_string[255];
}";
            var AST = builder.Program(input);
            // Assert.AreEqual(AST.Name, ""); TODO: the program should have a name. Maybe the file name. Otherwise add a field?
            Assert.AreEqual(AST.Elements.Count, 1);
            Assert.IsTrue(AST.Elements[0].GetType() == typeof(BiPaGe.AST.Object));
            var obj = (BiPaGe.AST.Object)AST.Elements[0];
            Assert.AreEqual(obj.identifier, "Object1");
            Assert.AreEqual(obj.fields.Count, 3);

            CheckField(obj.fields[0], "field1", new Signed(null, 32), new BiPaGe.AST.Literals.NumberLiteral(null, "5"));
            CheckField(obj.fields[1], "field2", new AsciiString(null), new BiPaGe.AST.Literals.NumberLiteral(null, "32"));
            CheckField(obj.fields[2], "field3", new Utf8String(null), new BiPaGe.AST.Literals.NumberLiteral(null, "255"));
        }

        [Test()]
        public void CollectionSizedByField()
        {
            var errors = new List<SemanticAnalysis.Error>();
            var warnings = new List<SemanticAnalysis.Warning>();
            var builder = new BiPaGe.AST.Builder(errors, warnings);
            var input = @"
Object1
{
    size : int32;
    field2 : ascii_string[size];
}";
            var AST = builder.Program(input);
            // Assert.AreEqual(AST.Name, ""); TODO: the program should have a name. Maybe the file name. Otherwise add a field?
            Assert.AreEqual(AST.Elements.Count, 1);
            Assert.IsTrue(AST.Elements[0].GetType() == typeof(BiPaGe.AST.Object));
            var obj = (BiPaGe.AST.Object)AST.Elements[0];
            Assert.AreEqual(obj.identifier, "Object1");
            Assert.AreEqual(obj.fields.Count, 2);

            CheckField(obj.fields[0], "size", new Signed(null, 32), new BiPaGe.AST.Literals.NumberLiteral(null, "5"));
            CheckField(obj.fields[1], "field2", new AsciiString(null), new BiPaGe.AST.Identifiers.FieldIdentifier(null, "size"));
        }

//        [Test()]
//        public void Enumeration()
//        {
//            var errors = new List<SemanticAnalysis.Error>();
//            var warnings = new List<SemanticAnalysis.Warning>();
//            var builder = new BiPaGe.AST.Builder(errors, warnings);
//            var input = @"
//SomeEnumeration
//{
//    value1 = 1,
//    value2 = 2,
//    value3 = 0
//}

//Object1
//{
//    enum_field : SomeEnumeration;
//}";
        //    var AST = builder.Program(input);
        //    // Assert.AreEqual(AST.Name, ""); TODO: the program should have a name. Maybe the file name. Otherwise add a field?
        //    Assert.AreEqual(AST.Objects.Count, 2);
        //    var obj = AST.Objects[0];
        //    Assert.AreEqual(obj.identifier, "Object1");
        //    Assert.AreEqual(obj.fields.Count, 2);

        //    CheckField(obj.fields[0], "size", new Signed(null, 32), new BiPaGe.AST.Literals.NumberLiteral(null, "5"));
        //    CheckField(obj.fields[1], "field2", new AsciiString(null), new BiPaGe.AST.Identifiers.FieldIdentifier(null, "size"));
        //}
    }
}
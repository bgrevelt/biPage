using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace BiPaGe.Test
{
    [TestFixture()]
    public class Creation
    {
        public void CheckField<T>(AST.Field field, String name, int size = -1)
        {
            Assert.AreEqual(field.Name, name);
            Assert.IsInstanceOf<T>(field.Type);

            if (size != -1)
            {
                var actual_size = field.Type.GetType().GetProperty("Size");
                Assert.NotNull(actual_size);
                Assert.AreEqual(actual_size.GetValue(field.Type), size);
            }
        }

        public void CheckCollection<Type>(AST.Field field, String name, int collection_size, int size = -1)
        {
            Assert.AreEqual(field.Name, name);
            Assert.IsInstanceOf<AST.FieldTypes.Collection>(field.Type);
            var collection = field.Type as AST.FieldTypes.Collection;
            Assert.IsInstanceOf<Type>(collection.Type);
            Assert.IsInstanceOf<AST.Literals.NumberLiteral>(collection.Size);
            Assert.AreEqual((collection.Size as AST.Literals.NumberLiteral).Number, collection_size);

            if (size != -1)
            {
                var actual_size = collection.Type.GetType().GetProperty("Size");
                Assert.NotNull(actual_size);
                Assert.AreEqual(actual_size.GetValue(collection.Type), size);
            }
        }

        [Test()]
        public void SingleEmptyObject()
        {
            var empy_object = "Object1{}";
            var errors = new List<SemanticAnalysis.Error>();
            var warnings = new List<SemanticAnalysis.Warning>();
            var builder = new BiPaGe.AST.Builder(errors, warnings);
            var AST = builder.Objects(empy_object);

            Assert.AreEqual(AST.Objects.Count, 1);

            var o = AST.Objects[0];
            Assert.AreEqual(o.identifier, "Object1");
            Assert.IsEmpty(o.fields);
        }

        [Test()]
        public void SingleObjectSingleIntegerField()
        {
            var empy_object = "Object1{field1 : int16;}";
            var errors = new List<SemanticAnalysis.Error>();
            var warnings = new List<SemanticAnalysis.Warning>();
            var builder = new BiPaGe.AST.Builder(errors, warnings);
            var AST = builder.Objects(empy_object);

            Assert.AreEqual(AST.Objects.Count, 1);

            var o = AST.Objects[0];
            Assert.AreEqual(o.identifier, "Object1");
            Assert.AreEqual(o.fields.Count, 1);

            CheckField<AST.FieldTypes.Signed>(o.fields[0], "field1", 16);                                
        }

        [Test()]
        public void SingleObjectSingleUnsignedField()
        {
            var empy_object = "Object1{field1 : uint24;}";
            var errors = new List<SemanticAnalysis.Error>();
            var warnings = new List<SemanticAnalysis.Warning>();
            var builder = new BiPaGe.AST.Builder(errors, warnings);
            var AST = builder.Objects(empy_object);

            Assert.AreEqual(AST.Objects.Count, 1);

            var o = AST.Objects[0];
            Assert.AreEqual(o.identifier, "Object1");
            Assert.AreEqual(o.fields.Count, 1);

            CheckField<AST.FieldTypes.Unsigned>(o.fields[0], "field1", 24); 
        }

        [Test()]
        public void SingleObjectSingleFloatField()
        {
            var empy_object = "Object1{field1 : float11;}";
            var errors = new List<SemanticAnalysis.Error>();
            var warnings = new List<SemanticAnalysis.Warning>();
            var builder = new BiPaGe.AST.Builder(errors, warnings);
            var AST = builder.Objects(empy_object);

            Assert.AreEqual(AST.Objects.Count, 1);

            var o = AST.Objects[0];
            Assert.AreEqual(o.identifier, "Object1");
            Assert.AreEqual(o.fields.Count, 1);

            CheckField<AST.FieldTypes.Float>(o.fields[0], "field1", 11);
        }

        [Test()]
        public void SingleObjectSingleBoolField()
        {
            var empy_object = "Object1{field1 : bool;}";
            var errors = new List<SemanticAnalysis.Error>();
            var warnings = new List<SemanticAnalysis.Warning>();
            var builder = new BiPaGe.AST.Builder(errors, warnings);
            var AST = builder.Objects(empy_object);

            Assert.AreEqual(AST.Objects.Count, 1);

            var o = AST.Objects[0];
            Assert.AreEqual(o.identifier, "Object1");
            Assert.AreEqual(o.fields.Count, 1);

            CheckField<AST.FieldTypes.Boolean>(o.fields[0], "field1");
        }

        [Test()]
        public void SingleObjectFixedCollectionFields()
        {
            var empy_object = @"
Object1
{
    field1 : int23[5];
    field2 : s16[8];
    field3 : uint8[22];
    field4 : u33[325];
    field5 : float64[3];
    field6 : bool[22];
}";
            var errors = new List<SemanticAnalysis.Error>();
            var warnings = new List<SemanticAnalysis.Warning>();
            var builder = new BiPaGe.AST.Builder(errors, warnings);
            var AST = builder.Objects(empy_object);

            Assert.AreEqual(AST.Objects.Count, 1);

            var o = AST.Objects[0];
            Assert.AreEqual(o.identifier, "Object1");
            Assert.AreEqual(o.fields.Count, 6);


            CheckCollection<AST.FieldTypes.Signed>(o.fields[0], "field1", 5, 23);
            CheckCollection<AST.FieldTypes.Signed>(o.fields[1], "field2", 8, 16);
            CheckCollection<AST.FieldTypes.Unsigned>(o.fields[2], "field3", 22, 8);
            CheckCollection<AST.FieldTypes.Unsigned>(o.fields[3], "field4", 325, 33);
            CheckCollection<AST.FieldTypes.Float>(o.fields[4], "field5", 3, 64);
            CheckCollection<AST.FieldTypes.Boolean>(o.fields[5], "field6", 22);
        }

        [Test()]
        public void SingleObjectMultipleFields()
        {
            var empy_object = @"Object1
{
    field1 : s5;
    field2 : u12;
    field3 : f15;
    field4 : bool;
}";
            var errors = new List<SemanticAnalysis.Error>();
            var warnings = new List<SemanticAnalysis.Warning>();
            var builder = new BiPaGe.AST.Builder(errors, warnings);
            var AST = builder.Objects(empy_object);

            Assert.AreEqual(AST.Objects.Count, 1);

            var o = AST.Objects[0];
            Assert.AreEqual(o.identifier, "Object1");
            Assert.AreEqual(o.fields.Count, 4);

            CheckField<AST.FieldTypes.Signed>(o.fields[0], "field1", 5);
            CheckField<AST.FieldTypes.Unsigned>(o.fields[1], "field2", 12);
            CheckField<AST.FieldTypes.Float>(o.fields[2], "field3", 15);
            CheckField<AST.FieldTypes.Boolean>(o.fields[3], "field4");
        }

        [Test()]
        public void MultipleObjectMultipleFields()
        {
            var empy_object = @"Object1
{
    field1 : s5;
    field2 : u12;
}

Object2
{

    field3 : f15;
    field4 : bool;
}

Object3
{
    field5 : bool[8];
    field6 : int8[32];
}
";
            var errors = new List<SemanticAnalysis.Error>();
            var warnings = new List<SemanticAnalysis.Warning>();
            var builder = new BiPaGe.AST.Builder(errors, warnings);
            var AST = builder.Objects(empy_object);

            Assert.AreEqual(AST.Objects.Count, 3);

            var o1 = AST.Objects[0];
            Assert.AreEqual(o1.identifier, "Object1");
            Assert.AreEqual(o1.fields.Count, 2);

            CheckField<AST.FieldTypes.Signed>(o1.fields[0], "field1", 5);
            CheckField<AST.FieldTypes.Unsigned>(o1.fields[1], "field2", 12);

            var o2 = AST.Objects[1];
            Assert.AreEqual(o2.identifier, "Object2");
            Assert.AreEqual(o2.fields.Count, 2);
            CheckField<AST.FieldTypes.Float>(o2.fields[0], "field3", 15);
            CheckField<AST.FieldTypes.Boolean>(o2.fields[1], "field4");

            var o3 = AST.Objects[2];
            Assert.AreEqual(o3.identifier, "Object3");
            Assert.AreEqual(o3.fields.Count, 2);
            CheckCollection<AST.FieldTypes.Boolean>(o3.fields[0], "field5", 8);
            CheckCollection<AST.FieldTypes.Signed>(o3.fields[1], "field6", 32, 8);
        }

        // TODO: Test dynamically sized collections
    }
}
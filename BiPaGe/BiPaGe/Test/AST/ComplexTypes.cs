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

namespace BiPaGe.Test.AST.ComplexTypes
{
    [TestFixture()]
    public class Creation
    {
        [Test()]
        public void SingleEmbedded()
        {
            var input = @"
Object1
{
    field1 : i32;
    field2 : SomeOtherObject;
}
";
            var expected = new Helpers.ProgramBuilder();
            var object1 = expected.AddObject();
            object1.Name = "Object1";
            object1.Fields.Add(("field1", new Signed(null, 32), null, null));
            object1.Fields.Add(("field2", new Identifier(null, "SomeOtherObject"), null, null));           
            expected.Validate(SimpleBuilder.Build(input));
        }
    }

    [TestFixture()]
    public class FixedValues
    {
        [Test()]
        public void SingleLevelSingleValue()
        {
            var input = @"
Object1
{
    field1 : i32;
    field2 : SomeOtherObject(field1 = 5);
}";
            var expected = new Helpers.ProgramBuilder();
            var object1 = expected.AddObject();
            object1.Name = "Object1";
            object1.Fields.Add(("field1", new Signed(null, 32), null, null));
            object1.Fields.Add(("field2", new Identifier(null, "SomeOtherObject"), null, 
                new BiPaGe.AST.Constants.ObjectConstant(null, 
                    new List<ObjectField> {
                        new ObjectField("field1", new BiPaGe.AST.Literals.Integer(null, "5"))
                    })));
            expected.Validate(SimpleBuilder.Build(input));
        }

        [Test()]
        public void SingleLevelMultipleValue()
        {
            var input = @"
Object1
{
    field1 : i32;
    field2 : SomeOtherObject(field1 = 5, field2 = 1.23, field3 = ""Some string"", field4 = {1,2,3,4,5}, field5 = false);
}";
            var expected = new Helpers.ProgramBuilder();
            var object1 = expected.AddObject();
            object1.Name = "Object1";
            object1.Fields.Add(("field1", new Signed(null, 32), null, null));
            object1.Fields.Add(("field2", new Identifier(null, "SomeOtherObject"), null,
                new BiPaGe.AST.Constants.ObjectConstant(null,
                    new List<ObjectField> {
                        new ObjectField("field1", new BiPaGe.AST.Literals.Integer(null, "5")),
                        new ObjectField("field2", new BiPaGe.AST.Literals.Float(null, "1.23")),
                        new ObjectField("field3", new BiPaGe.AST.Literals.StringLiteral(null, "Some string")),
                        new ObjectField("field4", new LiteralCollection(null, new List<BiPaGe.AST.Literals.Literal> {
                               new BiPaGe.AST.Literals.Integer(null, "1"),
                               new BiPaGe.AST.Literals.Integer(null, "2"),
                               new BiPaGe.AST.Literals.Integer(null, "3"),
                               new BiPaGe.AST.Literals.Integer(null, "4"),
                               new BiPaGe.AST.Literals.Integer(null, "5")})),
                        new ObjectField("field5", new BiPaGe.AST.Literals.Boolean(null, "false"))
                    })));
            expected.Validate(SimpleBuilder.Build(input));
        }

        [Test()]
        public void TrippleLevelSingleValue()
        {
            var input = @"
Object1
{
    field1 : i32;
    field2 : SomeOtherObject(parent.parent.field1 = 5);
}";
            var expected = new Helpers.ProgramBuilder();
            var object1 = expected.AddObject();
            object1.Name = "Object1";
            object1.Fields.Add(("field1", new Signed(null, 32), null, null));
            object1.Fields.Add(("field2", new Identifier(null, "SomeOtherObject"), null,
                new BiPaGe.AST.Constants.ObjectConstant(null,
                    new List<ObjectField> {
                        new ObjectField("parent.paernt.field1", new BiPaGe.AST.Literals.Integer(null, "5"))
                    })));
            expected.Validate(SimpleBuilder.Build(input));
        }

        [Test()]
        public void TrippleLevelMultipleValue()
        {
            var input = @"
Object1
{
    field1 : i32;
    field2 : SomeOtherObject(parent.paernt.field1 = 5, parent.paernt.field2 = 1.23, parent.paernt.field3 = ""Some string"", parent.paernt.field4 = {1,2,3,4,5}, parent.paernt.field5 = false);
}";
            var expected = new Helpers.ProgramBuilder();
            var object1 = expected.AddObject();
            object1.Name = "Object1";
            object1.Fields.Add(("field1", new Signed(null, 32), null, null));
            object1.Fields.Add(("field2", new Identifier(null, "SomeOtherObject"), null,
                new BiPaGe.AST.Constants.ObjectConstant(null,
                    new List<ObjectField> {
                        new ObjectField("field1", new BiPaGe.AST.Literals.Integer(null, "5")),
                        new ObjectField("field2", new BiPaGe.AST.Literals.Float(null, "1.23")),
                        new ObjectField("field3", new BiPaGe.AST.Literals.StringLiteral(null, "Some string")),
                        new ObjectField("field4", new LiteralCollection(null, new List<BiPaGe.AST.Literals.Literal> {
                               new BiPaGe.AST.Literals.Integer(null, "1"),
                               new BiPaGe.AST.Literals.Integer(null, "2"),
                               new BiPaGe.AST.Literals.Integer(null, "3"),
                               new BiPaGe.AST.Literals.Integer(null, "4"),
                               new BiPaGe.AST.Literals.Integer(null, "5")})),
                        new ObjectField("field5", new BiPaGe.AST.Literals.Boolean(null, "false"))
                    })));
            expected.Validate(SimpleBuilder.Build(input));
        }
    }
}

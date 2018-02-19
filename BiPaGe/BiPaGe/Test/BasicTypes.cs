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

namespace BiPaGe.Test.AST.BasicTypes
{
    [TestFixture()]
    public class Creation
    {
        [Test()]
        public void StandardSize()
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
            var expected = new Helpers.ProgramBuilder();
            var object1 = expected.AddObject();
            object1.Name = "Object1";
            object1.Fields.Add(("field1", new Signed(null, 16), null, null));
            object1.Fields.Add(("field2", new Unsigned(null, 32), null, null));
            object1.Fields.Add(("field3", new Float(null, 32), null, null));
            object1.Fields.Add(("field4", new Float(null, 64), null, null));
            object1.Fields.Add(("field5", new BiPaGe.AST.FieldTypes.Boolean(null), null, null));
            expected.Validate(SimpleBuilder.Build(input));
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
            var expected = new ProgramBuilder();
            var object1 = expected.AddObject();
            object1.Name = "Object1";
            object1.Fields.Add(("field1", new Signed(null, 2), null, null));
            object1.Fields.Add(("field2", new Unsigned(null, 6), null, null));
            object1.Fields.Add(("field3", new Signed(null, 11), null, null));
            object1.Fields.Add(("field4", new Unsigned(null, 12), null, null));
            object1.Fields.Add(("field5", new BiPaGe.AST.FieldTypes.Boolean(null), null, null));
            expected.Validate(SimpleBuilder.Build(input));   
        }

        [Test()]
        public void Reserved()
        {
            var input = @"
Object1
{
    int2;
    u6;
    i11;
    u12;
    bool;
}";
            var expected = new ProgramBuilder();
            var object1 = expected.AddObject();
            object1.Name = "Object1";
            object1.Fields.Add((null, new Signed(null, 2), null, null));
            object1.Fields.Add((null, new Unsigned(null, 6), null, null));
            object1.Fields.Add((null, new Signed(null, 11), null, null));
            object1.Fields.Add((null, new Unsigned(null, 12), null, null));
            object1.Fields.Add((null, new BiPaGe.AST.FieldTypes.Boolean(null), null, null));
            expected.Validate(SimpleBuilder.Build(input));
        }
    }

    [TestFixture()]
    public class FixedValues
    {
        [Test()]
        public void SignedPositive()
        {
            var input = @"
Object1
{
    field1 : int16 = 5;
}";
            var expected = new Helpers.ProgramBuilder();
            var object1 = expected.AddObject();
            object1.Name = "Object1";
            object1.Fields.Add(("field1", new Signed(null, 16), null, new BiPaGe.AST.Literals.Integer(null, "5")));
            expected.Validate(SimpleBuilder.Build(input));
        }  

        [Test()]
        public void SignedNegative()
        {
            var input = @"
Object1
{
    field1 : int16 = -5;
}";
            var expected = new Helpers.ProgramBuilder();
            var object1 = expected.AddObject();
            object1.Name = "Object1";
            object1.Fields.Add(("field1", new Signed(null, 16), null, new BiPaGe.AST.Literals.Integer(null, "-5")));
            expected.Validate(SimpleBuilder.Build(input));
        } 

        [Test()]
        public void Unsigned()
        {
            var input = @"
Object1
{
    field1 : uint32 = 500;
}";
            var expected = new Helpers.ProgramBuilder();
            var object1 = expected.AddObject();
            object1.Name = "Object1";
            object1.Fields.Add(("field1", new Unsigned(null, 32), null, new BiPaGe.AST.Literals.Integer(null, "500")));
            expected.Validate(SimpleBuilder.Build(input));
        }
    
        [Test()]
        public void FloatPositive()
        {
            var input = @"
Object1
{
    field1 : float32 = 123.456;
}";
            var expected = new Helpers.ProgramBuilder();
            var object1 = expected.AddObject();
            object1.Name = "Object1";
            object1.Fields.Add(("field1", new Float(null, 32), null, new BiPaGe.AST.Literals.Float(null, "123.456")));
            expected.Validate(SimpleBuilder.Build(input));
        }

        [Test()]
        public void FloatNegative()
        {
            var input = @"
Object1
{
    field1 : float64 = -3.14;
}";
            var expected = new Helpers.ProgramBuilder();
            var object1 = expected.AddObject();
            object1.Name = "Object1";
            object1.Fields.Add(("field1", new Float(null, 64), null, new BiPaGe.AST.Literals.Float(null, "-3.14")));
            expected.Validate(SimpleBuilder.Build(input));
        }

        [Test()]
        public void Bool()
        {
            var input = @"
Object1
{
    field1 : bool = true;
    field2 : bool = false;
}";
            var expected = new Helpers.ProgramBuilder();
            var object1 = expected.AddObject();
            object1.Name = "Object1";
            object1.Fields.Add(("field1", new BiPaGe.AST.FieldTypes.Boolean(null), null, new BiPaGe.AST.Literals.Boolean(null,"true")));
            object1.Fields.Add(("field2", new BiPaGe.AST.FieldTypes.Boolean(null), null, new BiPaGe.AST.Literals.Boolean(null,"false")));
            expected.Validate(SimpleBuilder.Build(input));
        }

    }
}

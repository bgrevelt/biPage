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
    namespace Fakes
    {
        public class Enumeration
        {
            public String Name;
            public FieldType Type;
            public List<(String, int)> Enumerators = new List<(string, int)>();
        }

        public class Object
        {
            public String Name;
            public List<(String, FieldType, IExpression, IFixer)> Fields = new List<(string, FieldType, IExpression, IFixer)>();
        }

        public class ProgramBuilder
        {
            private List<Enumeration> Enumerations = new List<Enumeration>();
            private List<Object> Objects = new List<Object>();

            public Enumeration AddEnumeration()
            {
                var new_enum = new Enumeration();
                Enumerations.Add(new_enum);
                return new_enum;
            }

            public Object AddObject()
            {
                var new_object = new Object();
                Objects.Add(new_object);
                return new_object;
            }

            public void Validate(IASTNode program)
            {
                Assert.IsTrue(ToAst().Equals(program));
            }

            private IASTNode ToAst()
            {
                List<BiPaGe.AST.Element> elements = new List<Element>();
                foreach (var e in this.Enumerations)
                    elements.Add(new BiPaGe.AST.Enumeration(null, e.Name, e.Type, e.Enumerators.Select(f => new BiPaGe.AST.Enumerator(null, f.Item1, f.Item2.ToString())).ToList()));
                foreach (var o in this.Objects)
                    elements.Add(new BiPaGe.AST.Object(null, o.Name, o.Fields.Select(f => new Field(null, f.Item1, f.Item2, f.Item3, f.Item4)).ToList()));

                return new BiPaGe.AST.Parser(null, "No name yet", elements);
            }

        }
    }


    [TestFixture()]
    public class Building
    {
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
            var expected = new Fakes.ProgramBuilder();
            var object1 = expected.AddObject();
            object1.Name = "Object1";
            object1.Fields.Add(("field1", new Signed(null, 16), null, null));
            object1.Fields.Add(("field2", new Unsigned(null, 32), null, null));
            object1.Fields.Add(("field3", new Float(null, 32), null, null));
            object1.Fields.Add(("field4", new Float(null, 64), null, null));
            object1.Fields.Add(("field5", new BiPaGe.AST.FieldTypes.Boolean(null), null, null));
            expected.Validate(Build(input));
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
            var expected = new Fakes.ProgramBuilder();
            var object1 = expected.AddObject();
            object1.Name = "Object1";
            object1.Fields.Add(("field1", new Signed(null, 2), null, null));
            object1.Fields.Add(("field2", new Unsigned(null, 6), null, null));
            object1.Fields.Add(("field3", new Signed(null, 11), null, null));
            object1.Fields.Add(("field4", new Unsigned(null, 12), null, null));
            object1.Fields.Add(("field5", new BiPaGe.AST.FieldTypes.Boolean(null), null, null));
            expected.Validate(Build(input));
            var AST = Build(input);            
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
            var expected = new Fakes.ProgramBuilder();
            var object1 = expected.AddObject();
            object1.Name = "Object1";
            object1.Fields.Add(("field1", new Signed(null, 32), new BiPaGe.AST.Literals.Integer(null, "5"), null));
            object1.Fields.Add(("field2", new AsciiString(null), new BiPaGe.AST.Literals.Integer(null, "32"), null));
            object1.Fields.Add(("field3", new Utf8String(null), new BiPaGe.AST.Literals.Integer(null, "255"), null));
            expected.Validate(Build(input));
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
            var expected = new Fakes.ProgramBuilder();
            var object1 = expected.AddObject();
            object1.Name = "Object1";
            object1.Fields.Add(("size", new Signed(null, 32), null, null));
            object1.Fields.Add(("field2", new AsciiString(null), new BiPaGe.AST.Identifiers.FieldIdentifier(null, "size"), null));
            expected.Validate(Build(input));
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
            var expected = new Fakes.ProgramBuilder();
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
            expected.Validate(Build(input));
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

            var expected = new Fakes.ProgramBuilder();
            var object1= expected.AddObject();
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
            expected.Validate(Build(input));
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

            var expected = new Fakes.ProgramBuilder();

            var enum1 = expected.AddEnumeration();
            enum1.Name = "SomeEnumeration";
            enum1.Type = new Unsigned(null, 8);
            enum1.Enumerators.Add(("value1", 1));
            enum1.Enumerators.Add(("value2", 2));
            enum1.Enumerators.Add(("value3", 0));

            var object1 = expected.AddObject();
            object1.Name = "Object1";
            object1.Fields.Add(("enum_field", new Identifier(null, "SomeEnumeration"), null, null));

            expected.Validate(AST);

      
        }
    }
}

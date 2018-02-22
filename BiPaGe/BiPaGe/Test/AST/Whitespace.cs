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

namespace BiPaGe.Test.AST.Whitespace
{
    [TestFixture()]
    public class Creation
    {
        [Test()]
        public void Basic()
        {
            var expected = new Helpers.ProgramBuilder();
            var object1 = expected.AddObject();
            object1.Name = "Object1";
            object1.Fields.Add(("field1", new Signed(null, 16), null, null));
            object1.Fields.Add(("field2", new Unsigned(null, 32), null, null));
            object1.Fields.Add(("field3", new Float(null, 32), null, null));
            object1.Fields.Add(("field4", new Float(null, 64), null, null));
            object1.Fields.Add(("field5", new BiPaGe.AST.FieldTypes.Boolean(null), null, null));
            expected.Validate(SimpleBuilder.Build("Object1{field1:int16;field2:u32;field3:float32;field4:f64;field5:bool;}"));
        }

        [Test()]
        public void Collection()
        {
            var expected = new Helpers.ProgramBuilder();
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
            expected.Validate(SimpleBuilder.Build("Object1{int32[5];ascii_string[32];utf8_string[255];ascii_string[size];u32[(size - this) / 4];bool[size + size2 - 10 * 5];}"));
        }

        [Test()]
        public void Enumeration()
        {
            var expected = new ProgramBuilder();

            var enum1 = expected.AddEnumeration();
            enum1.Name = "SomeEnumeration";
            enum1.Type = new Unsigned(null, 8);
            enum1.Enumerators.Add(("value1", 1));
            enum1.Enumerators.Add(("value2", 2));
            enum1.Enumerators.Add(("value3", 0));

            var object1 = expected.AddObject();
            object1.Name = "Object1";
            object1.Fields.Add(("enum_field", new Identifier(null, "SomeEnumeration"), null, null));

            expected.Validate(SimpleBuilder.Build("SomeEnumeration : u8{value1=1,value2=2,value3=0}Object1{enum_field:SomeEnumeration;}"));
        }

        [Test()]
        public void InlineEnumeration()
        {
            var expected = new ProgramBuilder();

            var inline_enum = new BiPaGe.AST.FieldTypes.InlineEnumeration(null, new Unsigned(null, 8),
                                                                         new List<BiPaGe.AST.Enumerator>
            {
                new BiPaGe.AST.Enumerator(null, "value1", "1"),
                new BiPaGe.AST.Enumerator(null, "value2", "2"),
                new BiPaGe.AST.Enumerator(null, "value3", "0")
            });

            var object1 = expected.AddObject();
            object1.Name = "Object1";
            object1.Fields.Add(("enum_field", inline_enum, null, null));

            expected.Validate(SimpleBuilder.Build("Object1{enum_field:u8{value1=1,value2=2,value3=0};}"));
        }

        [Test()]
        public void DoubleEmbedded()
        {
            ;
            var expected = new Helpers.ProgramBuilder();
            var object1 = expected.AddObject();
            object1.Name = "Object1";
            object1.Fields.Add(("field1", new Signed(null, 16), null, null));
            object1.Fields.Add(("field2", new Unsigned(null, 32), null, null));

            var superembedded = new BiPaGe.AST.FieldTypes.InlineObject(null, new List<BiPaGe.AST.Field> {
                new Field(null, "nestnest1", new Utf8String(null), new BiPaGe.AST.Literals.Integer(null, "64"), null),
                new Field(null, "nestnest2", new Identifier(null, "SomeOtherObject"), null, null),
                new Field(null, "nestnest3", new Unsigned(null, 12), null, null),
                          });

            var embedded = new BiPaGe.AST.FieldTypes.InlineObject(null, new List<BiPaGe.AST.Field> {
                new Field(null, "embedded1", new Float(null, 32), null, null),
                new Field(null, "embedded2", new BiPaGe.AST.FieldTypes.Boolean(null), new BiPaGe.AST.Literals.Integer(null, "3"), null),
                new Field(null, "embedded_embedded", superembedded, null, null)
                          });

            object1.Fields.Add(("embedded_object", embedded, null, null));
            expected.Validate(SimpleBuilder.Build("Object1{field1:int16;field2:u32;embedded_object:{embedded1:f32;embedded2:bool[3];embedded_embedded:{nestnest1:utf8_string[64];nestnest2:SomeOtherObject;nestnest3:u12;}};}"));
        }
    }

    [TestFixture()]
    public class Fixer
    {
        [Test()]
        public void BasicTypes()
        {
            var expected = new Helpers.ProgramBuilder();
            var object1 = expected.AddObject();
            object1.Name = "Object1";
            object1.Fields.Add(("field1", new Unsigned(null, 32), null, new BiPaGe.AST.Literals.Integer(null, "500")));
            object1.Fields.Add(("field2", new Signed(null, 16), null, new BiPaGe.AST.Literals.Integer(null, "-5")));
            object1.Fields.Add(("field3", new Float(null, 32), null, new BiPaGe.AST.Literals.Float(null, "123.456")));
            object1.Fields.Add(("field4", new Float(null, 64), null, new BiPaGe.AST.Literals.Float(null, "-3.14")));
            object1.Fields.Add(("field5", new BiPaGe.AST.FieldTypes.Boolean(null), null, new BiPaGe.AST.Literals.Boolean(null, "true")));
            object1.Fields.Add(("field6", new BiPaGe.AST.FieldTypes.Boolean(null), null, new BiPaGe.AST.Literals.Boolean(null, "false")));
            expected.Validate(SimpleBuilder.Build("Object1{field1:uint32=500;field2:int16=-5;field3:float32=123.456;field4:float64=-3.14;field5:bool=true;field6:bool=false;}"));
        }

        [Test()]
        public void Collections()
        {            
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
            object1.Fields.Add(("field2", new Signed(null, 32), new BiPaGe.AST.Literals.Integer(null, "5"),
                                new LiteralCollection(null,
                                    new List<BiPaGe.AST.Literals.Literal>() {
                                        new BiPaGe.AST.Literals.Integer(null, "-1"),
                                        new BiPaGe.AST.Literals.Integer(null, "-2"),
                                        new BiPaGe.AST.Literals.Integer(null, "-3"),
                                        new BiPaGe.AST.Literals.Integer(null, "-4"),
                                        new BiPaGe.AST.Literals.Integer(null, "-5")
            })));
            object1.Fields.Add(("field3", new Signed(null, 32), new BiPaGe.AST.Literals.Integer(null, "5"),
                                new LiteralCollection(null,
                                    new List<BiPaGe.AST.Literals.Literal>() {
                                        new BiPaGe.AST.Literals.Integer(null, "1"),
                                        new BiPaGe.AST.Literals.Integer(null, "-2"),
                                        new BiPaGe.AST.Literals.Integer(null, "3"),
                                        new BiPaGe.AST.Literals.Integer(null, "-4"),
                                        new BiPaGe.AST.Literals.Integer(null, "5")
            })));
            var field4_size = new Subtraction(
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
            object1.Fields.Add(("collection", new BiPaGe.AST.FieldTypes.Signed(null, 32), field4_size,
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
            object1.Fields.Add(("field4", new BiPaGe.AST.FieldTypes.Boolean(null), new BiPaGe.AST.Literals.Integer(null, "3"),
                               new LiteralCollection(null,
                                   new List<BiPaGe.AST.Literals.Literal>() {
                                        new BiPaGe.AST.Literals.Boolean(null, "true"),
                                        new BiPaGe.AST.Literals.Boolean(null, "false"),
                                        new BiPaGe.AST.Literals.Boolean(null, "true")
           })));
            object1.Fields.Add(("field5", new AsciiString(null), new BiPaGe.AST.Literals.Integer(null, "64"), new BiPaGe.AST.Literals.StringLiteral(null, "This is a string initializer")));
            expected.Validate(SimpleBuilder.Build("Object1{field1:int32[5]={1,2,3,4,5};field2:int32[5]={-1,-2,-3,-4,-5};field3:int32[5]={1,-2,3,-4,5};collection:i32[size+size2-10*5]={1,1,2,3,5,8,13,21,34,55};field4:bool[3]={true, false, true};field5:ascii_string[64]=\"This is a string initializer\";}"));
        }
    }
      
}
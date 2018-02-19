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

namespace BiPaGe.Test.AST.InlineObjects
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
    field1 : int16;
    field2 : u32;
    embedded_object : { embedded1 : f32 ; embedded2 : bool[3] ; embedded3 : ascii_string[8]; };
}";
            var expected = new Helpers.ProgramBuilder();
            var object1 = expected.AddObject();
            object1.Name = "Object1";
            object1.Fields.Add(("field1", new Signed(null, 16), null, null));
            object1.Fields.Add(("field2", new Unsigned(null, 32), null, null));

            var embedded = new BiPaGe.AST.FieldTypes.InlineObject(null, new List<BiPaGe.AST.Field> {
                new Field(null, "embedded1", new Float(null, 32), null, null),
                new Field(null, "embedded2", new BiPaGe.AST.FieldTypes.Boolean(null), new BiPaGe.AST.Literals.Integer(null, "3"), null),
                new Field(null, "embedded3", new AsciiString(null), new BiPaGe.AST.Literals.Integer(null, "8"), null),
                          });

            object1.Fields.Add(("embedded_object", embedded, null, null));
            expected.Validate(SimpleBuilder.Build(input));
        }

        [Test()]
        public void DoubleEmbedded()
        {
            var input = @"
Object1
{
    field1 : int16;
    field2 : u32;
    embedded_object : 
    { 
        embedded1 : f32; 
        embedded2 : bool[3]; 
        embedded_embedded :
        {
            nestnest1 : utf8_string[64];
            nestnest2 : SomeOtherObject;
            nestnest3 : u12; 
        } 
    };
}";
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
            expected.Validate(SimpleBuilder.Build(input));
        }
    }
}

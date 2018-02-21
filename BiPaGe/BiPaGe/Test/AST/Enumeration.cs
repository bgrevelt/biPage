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

namespace BiPaGe.Test.AST.Enumeration
{
    [TestFixture()]
    public class Creation
    {
        [Test()]
        public void Standard()
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

            expected.Validate(SimpleBuilder.Build(input));
        }


        // TODO add a test for a signed integer enum type. Values may be negative there

    }
}

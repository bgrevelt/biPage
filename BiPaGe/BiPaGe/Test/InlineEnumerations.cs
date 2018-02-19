using NUnit.Framework;
using BiPaGe.AST.FieldTypes;
using BiPaGe.AST.Identifiers;
using Helpers;
using System.Collections.Generic;

namespace BiPaGe.Test.AST.InlineEnumeration
{
    [TestFixture()]
    public class Creation
    {
        [Test()]
        public void Standard()
        {
            var input = @"
Object1
{
    enum_field : u8
    {
        value1 = 1,
        value2 = 2,
        value3 = 0
    };
}";
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

            expected.Validate(SimpleBuilder.Build(input));
        }


    }
}

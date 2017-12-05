using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace BiPaGe.Test
{
    [TestFixture()]
    public class NUnitTestClass
    {
        [Test()]
        public void TestCase()
        {
            var float_type = new AST.FieldTypes.Float("float23");
            List<String> warnings = new List<string>();
            List<String> errors = new List<string>();
            Assert.IsFalse(float_type.CheckSemantics(errors, warnings));
        }
    }
}

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

namespace BiPaGe.Test.SemanticAnalysis.Enumerations
{
    [TestFixture()]
    public class Uniqueness
    {
        [Test()]
        public void Labels()
        {
            var input = @"
Enumeration1 : u8
{
    label1 = 1,
    label2 = 2,
    label2 = 3,
    label3 = 4
}";

            var AST = SimpleBuilder.Build(input);
            BiPaGe.SemanticAnalysis.SemanticAnalyzer analyzer = new BiPaGe.SemanticAnalysis.SemanticAnalyzer();
            var events = analyzer.CheckSemantics(AST);

            Assert.AreEqual(1, events.Count);
            Assert.IsInstanceOf<BiPaGe.SemanticAnalysis.Error>(events[0]);            
        }

        [Test()]
        public void Values()
        {
            var input = @"
Enumeration1 : u8
{
    label1 = 1,
    label2 = 2,
    label3 = 2,
    label4 = 3
}";

            var AST = SimpleBuilder.Build(input);
            BiPaGe.SemanticAnalysis.SemanticAnalyzer analyzer = new BiPaGe.SemanticAnalysis.SemanticAnalyzer();
            var events = analyzer.CheckSemantics(AST);

            Assert.AreEqual(1, events.Count);
            Assert.IsInstanceOf<BiPaGe.SemanticAnalysis.Error>(events[0]);
        }
    }

    [TestFixture()]
    public class InvalidEnumerationTypes
    {
        [Test()]
        public void AsciiString()
        {
            var input = @"
Enumeration1 : ascii_string
{
    label1 = ""1"",
    label2 = ""2"",
    label3 = ""3"",
    label4 = ""4""
}";

            var AST = SimpleBuilder.Build(input);
            BiPaGe.SemanticAnalysis.SemanticAnalyzer analyzer = new BiPaGe.SemanticAnalysis.SemanticAnalyzer();
            var events = analyzer.CheckSemantics(AST);

            Assert.AreEqual(1, events.Count);
            Assert.IsInstanceOf<BiPaGe.SemanticAnalysis.Error>(events[0]);
        }

        [Test()]
        public void Utf8String()
        {
            var input = @"
Enumeration1 : utf8_string
{
    label1 = ""1"",
    label2 = ""2"",
    label3 = ""3"",
    label4 = ""4""
}";

            var AST = SimpleBuilder.Build(input);
            BiPaGe.SemanticAnalysis.SemanticAnalyzer analyzer = new BiPaGe.SemanticAnalysis.SemanticAnalyzer();
            var events = analyzer.CheckSemantics(AST);

            Assert.AreEqual(1, events.Count);
            Assert.IsInstanceOf<BiPaGe.SemanticAnalysis.Error>(events[0]);
        }

        [Test()]
        public void Float()
        {
            var input = @"
Enumeration1 : f64
{
    label1 = 1.0,
    label2 = 2.0,
    label3 = 3.0,
    label4 = 4.0
}";

            var AST = SimpleBuilder.Build(input);
            BiPaGe.SemanticAnalysis.SemanticAnalyzer analyzer = new BiPaGe.SemanticAnalysis.SemanticAnalyzer();
            var events = analyzer.CheckSemantics(AST);

            Assert.AreEqual(1, events.Count);
            Assert.IsInstanceOf<BiPaGe.SemanticAnalysis.Error>(events[0]);
        }

        [Test()]
        public void Boolean()
        {
            var input = @"
Enumeration1 : bool
{
    label1 = true,
    label2 = false
}";

            var AST = SimpleBuilder.Build(input);
            BiPaGe.SemanticAnalysis.SemanticAnalyzer analyzer = new BiPaGe.SemanticAnalysis.SemanticAnalyzer();
            var events = analyzer.CheckSemantics(AST);

            Assert.AreEqual(1, events.Count);
            Assert.IsInstanceOf<BiPaGe.SemanticAnalysis.Error>(events[0]);
        }
    }

    public class EnumeratorTypes
    {
        [Test()]
        public void ValidUnsigned()
        {
            var input = @"
Enumeration1 : u8
{
    label1 = 1,
    label2 = 2,
    label3 = 3,
    label4 = 4
}
";

            var AST = SimpleBuilder.Build(input);
            BiPaGe.SemanticAnalysis.SemanticAnalyzer analyzer = new BiPaGe.SemanticAnalysis.SemanticAnalyzer();
            var events = analyzer.CheckSemantics(AST);

            Assert.AreEqual(0, events.Count);
        }

        [Test()]
        public void ValidSigned()
        {
            var input = @"
Enumeration1 : i8
{
    label1 = -1,
    label2 = -2,
    label3 = -3,
    label4 = -4
}";

            var AST = SimpleBuilder.Build(input);
            BiPaGe.SemanticAnalysis.SemanticAnalyzer analyzer = new BiPaGe.SemanticAnalysis.SemanticAnalyzer();
            var events = analyzer.CheckSemantics(AST);

            Assert.AreEqual(0, events.Count);
        }

        [Test()]
        public void NegativeValuesInUnsigned()
        {
            var input = @"
Enumeration1 : u8
{
    label1 = -1,
    label2 = -2,
    label3 = -3,
    label4 = -4
}";

            var AST = SimpleBuilder.Build(input);
            BiPaGe.SemanticAnalysis.SemanticAnalyzer analyzer = new BiPaGe.SemanticAnalysis.SemanticAnalyzer();
            var events = analyzer.CheckSemantics(AST);

            Assert.AreEqual(4, events.Count);
            Assert.IsInstanceOf<BiPaGe.SemanticAnalysis.Error>(events[0]);
            Assert.IsInstanceOf<BiPaGe.SemanticAnalysis.Error>(events[1]);
            Assert.IsInstanceOf<BiPaGe.SemanticAnalysis.Error>(events[2]);
            Assert.IsInstanceOf<BiPaGe.SemanticAnalysis.Error>(events[3]);
        }

        [Test()]
        public void ValidMaximumStandardUnSignedRanges()
        {
            var input = @"
Enumu8 : u8
{
    label1 = 0,
    label2 = 255,
}

Enumu8 : u16
{
    label1 = 0,
    label2 = 65535,
}

Enumu8 : u32
{
    label1 = 0,
    label2 = 4294967295,
}

Enumu8 : u64
{
    label1 = 0,
    label2 = 18446744073709551615,
}
";

            var AST = SimpleBuilder.Build(input);
            BiPaGe.SemanticAnalysis.SemanticAnalyzer analyzer = new BiPaGe.SemanticAnalysis.SemanticAnalyzer();
            var events = analyzer.CheckSemantics(AST);

            Assert.AreEqual(0, events.Count);
        }

        [Test()]
        public void ValidMaximumStandardSignedRanges()
        {
            var input = @"
Enumu8 : i8
{
    label1 = -128,
    label2 = 127,
}

Enumu8 : i16
{
    label1 = -32768,
    label2 = 32767,
}

Enumu8 : i32
{
    label1 = -2147483648,
    label2 = -2147483647,
}

Enumu8 : i64
{
    label1 = -9223372036854775808,
    label2 = 9223372036854775807,
}
";

            var AST = SimpleBuilder.Build(input);
            BiPaGe.SemanticAnalysis.SemanticAnalyzer analyzer = new BiPaGe.SemanticAnalysis.SemanticAnalyzer();
            var events = analyzer.CheckSemantics(AST);

            Assert.AreEqual(0, events.Count);
        }

        [Test()]
        public void ValidMaximumNonStandardUnsignedRanges()
        {
            var input = @"
Enumu8 : u4
{
    label1 = 0,
    label2 = 7,
}

Enumu8 : u6
{
    label1 = 0,
    label2 = 31,
}

Enumu8 : u12
{
    label1 = 0,
    label2 = 4095,
}

Enumu8 : u22
{
    label1 = 0,
    label2 = 4194303,
}
";

            var AST = SimpleBuilder.Build(input);
            BiPaGe.SemanticAnalysis.SemanticAnalyzer analyzer = new BiPaGe.SemanticAnalysis.SemanticAnalyzer();
            var events = analyzer.CheckSemantics(AST);

            Assert.AreEqual(0, events.Count);
        }

        [Test()]
        public void ValidMaximumNonStandardSignedRanges()
        {
            var input = @"
Enumu8 : i4
{
    label1 = -4,
    label2 = 3,
}

Enumu8 : i6
{
    label1 = -16,
    label2 = 15,
}

Enumu8 : i12
{
    label1 = -2048,
    label2 = 2047,
}

Enumu8 : i22
{
    label1 = -2097152,
    label2 = 2097151,
}
";

            var AST = SimpleBuilder.Build(input);
            BiPaGe.SemanticAnalysis.SemanticAnalyzer analyzer = new BiPaGe.SemanticAnalysis.SemanticAnalyzer();
            var events = analyzer.CheckSemantics(AST);

            Assert.AreEqual(0, events.Count);
        }

        [Test()]
        public void InvalidStandarSignedValues()
        {
            var input = @"
Enumu8 : i8
{
    label1 = -129,
    label2 = 128,
}

Enumu8 : i16
{
    label1 = -32769,
    label2 = 32768,
}

Enumu8 : i32
{
    label1 = -2147483649,
    label2 = -2147483648,
}

Enumu8 : i64
{
    label1 = -9223372036854775809,
    label2 = 9223372036854775808,
}
";
            var AST = SimpleBuilder.Build(input);
            BiPaGe.SemanticAnalysis.SemanticAnalyzer analyzer = new BiPaGe.SemanticAnalysis.SemanticAnalyzer();
            var events = analyzer.CheckSemantics(AST);

            Assert.AreEqual(8, events.Count);
            Assert.IsInstanceOf<BiPaGe.SemanticAnalysis.Error>(events[0]);
            Assert.IsInstanceOf<BiPaGe.SemanticAnalysis.Error>(events[1]);
            Assert.IsInstanceOf<BiPaGe.SemanticAnalysis.Error>(events[2]);
            Assert.IsInstanceOf<BiPaGe.SemanticAnalysis.Error>(events[3]);
            Assert.IsInstanceOf<BiPaGe.SemanticAnalysis.Error>(events[4]);
            Assert.IsInstanceOf<BiPaGe.SemanticAnalysis.Error>(events[5]);
            Assert.IsInstanceOf<BiPaGe.SemanticAnalysis.Error>(events[6]);
            Assert.IsInstanceOf<BiPaGe.SemanticAnalysis.Error>(events[7]);
        }

        [Test()]
        public void InvalidStandarUnsignedValues()
        {
            var input = @"
Enumu8 : u8
{
    label1 = 0,
    label2 = 256,
}

Enumu8 : u16
{
    label1 = 0,
    label2 = 65536,
}

Enumu8 : u32
{
    label1 = 0,
    label2 = 4294967296,
}

Enumu8 : u64
{
    label1 = 0,
    label2 = 18446744073709551616,
}
";
            var AST = SimpleBuilder.Build(input);
            BiPaGe.SemanticAnalysis.SemanticAnalyzer analyzer = new BiPaGe.SemanticAnalysis.SemanticAnalyzer();
            var events = analyzer.CheckSemantics(AST);

            Assert.AreEqual(4, events.Count);
            Assert.IsInstanceOf<BiPaGe.SemanticAnalysis.Error>(events[0]);
            Assert.IsInstanceOf<BiPaGe.SemanticAnalysis.Error>(events[1]);
            Assert.IsInstanceOf<BiPaGe.SemanticAnalysis.Error>(events[2]);
            Assert.IsInstanceOf<BiPaGe.SemanticAnalysis.Error>(events[3]);
        }

        [Test()]
        public void InvalidNonStandardUnsignedRanges()
        {
            var input = @"
Enumu8 : i4
{
    label1 = -5,
    label2 = 4,
}

Enumu8 : i6
{
    label1 = -17,
    label2 = 16,
}

Enumu8 : i12
{
    label1 = -2049,
    label2 = 2048,
}

Enumu8 : i22
{
    label1 = -2097153,
    label2 = 2097153,
}
";

            var AST = SimpleBuilder.Build(input);
            BiPaGe.SemanticAnalysis.SemanticAnalyzer analyzer = new BiPaGe.SemanticAnalysis.SemanticAnalyzer();
            var events = analyzer.CheckSemantics(AST);

            Assert.AreEqual(0, events.Count);
        }

        [Test()]
        public void InvalidNonStandardSignedRanges()
        {
            var input = @"
Enumu8 : u4
{
    label1 = 0,
    label2 = 8,
}

Enumu8 : u6
{
    label1 = 0,
    label2 = 32,
}

Enumu8 : u12
{
    label1 = 0,
    label2 = 4096,
}

Enumu8 : u22
{
    label1 = 0,
    label2 = 4194304,
}
";
            var AST = SimpleBuilder.Build(input);
            BiPaGe.SemanticAnalysis.SemanticAnalyzer analyzer = new BiPaGe.SemanticAnalysis.SemanticAnalyzer();
            var events = analyzer.CheckSemantics(AST);

            Assert.AreEqual(4, events.Count);
            Assert.IsInstanceOf<BiPaGe.SemanticAnalysis.Error>(events[0]);
            Assert.IsInstanceOf<BiPaGe.SemanticAnalysis.Error>(events[1]);
            Assert.IsInstanceOf<BiPaGe.SemanticAnalysis.Error>(events[2]);
            Assert.IsInstanceOf<BiPaGe.SemanticAnalysis.Error>(events[3]);
        }
    }


    }

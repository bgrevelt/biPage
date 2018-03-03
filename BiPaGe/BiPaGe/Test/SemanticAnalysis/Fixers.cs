using NUnit.Framework;
using Helpers;
using System.Collections.Generic;

namespace BiPaGe.Test.SemanticAnalysis.Fixers
{
    [TestFixture()]
    public class TypeChecking
    {
        [Test()]
        public void Unsigned()
        {
            var input_simple = @"
            Object1
            {
                field1 : u8 = VALUE;
            }
            ";

            var input_complex = @"
            Object1
            {
                field1 : u8;
            }
            Object2
            {
                field1 : Object1(field1 = VALUE);
            }
            ";

            foreach (var input in new string[] { input_simple, input_complex })
            {
                TestRunner.Run(input, new List<(string, int, int, string)>
                {
                    ("0", 0, 0, "minimum value for u8"),
                    ("255", 0, 0, "maximum value for u8"),
                    ("-1", 0, 1, "should not be able to fix with a value that is too small for the type"),
                    ("256", 0, 1, "should not be able to fix with a value that is too large for the type"),
                    ("1.2345", 0, 1, "should not be able to fix an unsigned value with a floating point value"),
                    ("\"this is a string\"", 0, 1, "should not be able to fix an unsigned value with a string value"),
                    ("true", 0, 1, "should not be able to fix an unsigned value with a boolean value"),
                    ("false", 0, 1, "should not be able to fix an unsigned value with a boolean value"),
                    ("{1,2,3}", 0, 1, "should not be able to fix a single unsigned value with a collection of unsigneds"),
                    ("{-1,-2,-3}", 0, 1, "should not be able to fix a single unsigned value with a collection of signeds"),
                    ("{-1.12,-2.34,-3.45}", 0, 1, "should not be able to fix a single unsigned value with a collection of floats"),
                    ("{true, false, true}", 0, 1, "should not be able to fix a single unsigned value with a collection of booleans"),
                });
            }
        }

        [Test()]
        public void Signed()
        {
            var input_simple = @"
            Object1
            {
                field1 : i16 = VALUE;
            }
            ";

            var input_complex = @"
            Object1
            {
                field1 : i16;
            }
            Object2
            {
                field1 : Object1(field1 = VALUE);
            }
            ";

            foreach (var input in new string[] { input_simple, input_complex })
            {
                TestRunner.Run(input, new List<(string, int, int, string)>
            {
                ("-32768", 0, 0, "minimum value for i16"),
                ("32767", 0, 0, "maximum value for i16"),
                ("-32769", 0, 1, "Should not be able to fix on a value that is too small for the type"),
                ("32768", 0, 1, "Should not be able to fix on a value that is too large for the type"),
                ("1.2345", 0, 1, "should not be able to fix an integer with a floating point value"),
                ("\"this is a string\"", 0, 1, "should not be able to fix an integer value with a string value"),
                ("true", 0, 1, "should not be able to fix an integer value with a boolean value"),
                ("false", 0, 1, "should not be able to fix an integer value with a boolean value"),
                ("{1,2,3}", 0, 1, "should not be able to fix a single integer value with a collection of unsigneds"),
                ("{-1,-2,-3}", 0, 1, "should not be able to fix a single integer value with a collection of signeds"),
                ("{-1.12,-2.34,-3.45}", 0, 1, "should not be able to fix a single integer value with a collection of floats"),
                ("{true, false, true}", 0, 1, "should not be able to fix a single integer value with a collection of booleans"),
            });
            }
        }

        [Test()]
        public void Bool()
        {
            var input_simple = @"
            Object1
            {
                field1 : bool = VALUE;
            }
            ";

            var input_complex = @"
            Object1
            {
                field1 : bool;
            }
            Object2
            {
                field1 : Object1(field1 = VALUE);
            }
            ";

            foreach (var input in new string[] { input_simple, input_complex })
            {
                TestRunner.Run(input, new List<(string, int, int, string)>
            {
                ("false", 0, 0, "minimum value for i16"),
                ("true", 0, 0, "maximum value for i16"),
                ("1", 0, 1, "should not be able to fix a boolean with a positive integer value"),
                ("-1", 0, 1, "should not be able to fix a boolean with a negative integer value"),
                ("1.2345", 0, 1, "should not be able to fix a boolean with a floating point value"),
                ("\"this is a string\"", 0, 1, "should not be able to fix a boolean value with a string value"),
                ("true", 0, 1, "should not be able to fix a boolean value with a boolean value"),
                ("false", 0, 1, "should not be able to fix a boolean value with a boolean value"),
                ("{1,2,3}", 0, 1, "should not be able to fix a single boolean value with a collection of unsigneds"),
                ("{-1,-2,-3}", 0, 1, "should not be able to fix a single boolean value with a collection of signeds"),
                ("{-1.12,-2.34,-3.45}", 0, 1, "should not be able to fix a boolean integer value with a collection of floats"),
                ("{true, false, true}", 0, 1, "should not be able to fix a boolean integer value with a collection of booleans"),
            });
            }
        }

        [Test()]
        public void Float()
        {
            var input_simple = @"
            Object1
            {
                field1 : f64 = VALUE;
            }
            ";

            var input_complex = @"
            Object1
            {
                field1 : f64;
            }
            Object2
            {
                field1 : Object1(field1 = VALUE);
            }
            ";

            foreach (var input in new string[] { input_simple, input_complex })
            {
                TestRunner.Run(input, new List<(string, int, int, string)>
            {
                ("0.123456", 0, 0, "Valid float value"),
                ("1.2345", 0, 1, "should not be able to fix a float with a floating point value"),
                ("1", 0, 1, "should not be able to fix a float with a positive integer value"),   // We don't support automatic... 
                ("-1", 0, 1, "should not be able to fix a float with a negative integer value"),  // ...type conversion
                ("\"this is a string\"", 0, 1, "should not be able to fix a float value with a string value"),
                ("true", 0, 1, "should not be able to fix a float value with a boolean value"),
                ("false", 0, 1, "should not be able to fix a float value with a boolean value"),
                ("{1,2,3}", 0, 1, "should not be able to fix a single float value with a collection of unsigneds"),
                ("{-1,-2,-3}", 0, 1, "should not be able to fix a single float value with a collection of signeds"),
                ("{-1.12,-2.34,-3.45}", 0, 1, "should not be able to fix a float integer value with a collection of floats"),
                ("{true, false, true}", 0, 1, "should not be able to fix a float integer value with a collection of booleans"),
            });
            }
        }

        [Test()]
        public void UnsignedCollection()
        {
            var input_simple = @"
            Object1
            {
                field1 : u8[5] = VALUE;
            }
            ";

            var input_complex = @"
            Object1
            {
                field1 : u8[5];
            }
            Object2
            {
                field1 : Object1(field1 = VALUE);
            }
            ";

            foreach (var input in new string[] { input_simple, input_complex })
            {
                TestRunner.Run(input, new List<(string, int, int, string)>
            {
                ("{0,0,0,0,0}", 0, 0, "minimum value for u8"),
                ("{255,255,255,255,255}", 0, 0, "maximum value for u8"),
                ("-1", 0, 1, "should not be able to fix a collection with a single value"),
                ("1", 0, 1, "should not be able to fix a collection with a single value"),
                ("1.2345", 0, 1, "should not be able to fix a collection with a single value"),
                ("true", 0, 1, "should not be able to fix a collection with a single value"),
                ("false", 0, 1, "should not be able to fix a collection with a single value"),
                ("\"this is a string\"", 0, 1, "should not be able to fix a collection of unsigneds with a string value"),
                ("{0,0,-1,0,0}", 0, 1, "Should not be able to fix a collection of unsigneds with a collection for whic a value is too small for the type"),
                ("{0,0,256,0,0}", 0, 1, "Should not be able to fix a collection of unsigneds with a collection for whic a value is too large for the type"),
                ("{0.0,0.0,0.0,0.0,0.0}", 0, 1, "Should not be able to fix a collection of unsigneds with a collection of floats"),
                ("{true,false,true,false,true}", 0, 1, "Should not be able to fix a collection of unsigneds with a collection of floats"),
                ("{0,0,0,0}", 0, 1, "Should not be able to fix a collection with a collection that is too small"),
                ("{0,0,0,0,0,0}", 0, 1, "Should not be able to fix a collection with a collection that is too large")
            });
            }
        }

        [Test()]
        public void SignedCollection()
        {
            var input_simple = @"
            Object1
            {
                field1 : i16[5] = VALUE;
            }
            ";

            var input_complex = @"
            Object1
            {
                field1 : i16[5];
            }
            Object2
            {
                field1 : Object1(field1 = VALUE);
            }
            ";

            foreach (var input in new string[] { input_simple, input_complex })
            {
                TestRunner.Run(input, new List<(string, int, int, string)>
            {
                ("{-32768,-32768,-32768,-32768,-32768}", 0, 0, "minimum value for u8"),
                ("{32767,32767,32767,32767,32767}", 0, 0, "maximum value for u8"),
                ("-1", 0, 1, "should not be able to fix a collection with a single value"),
                ("1", 0, 1, "should not be able to fix a collection with a single value"),
                ("1.2345", 0, 1, "should not be able to fix a collection with a single value"),
                ("true", 0, 1, "should not be able to fix a collection with a single value"),
                ("false", 0, 1, "should not be able to fix a collection with a single value"),
                ("\"this is a string\"", 0, 1, "should not be able to fix a collection of signeds with a string value"),
                ("{0,0,-32769,0,0}", 0, 1, "Should not be able to fix a collection of unsigneds with a collection for whic a value is too small for the type"),
                ("{0,0,32768,0,0}", 0, 1, "Should not be able to fix a collection of unsigneds with a collection for whic a value is too large for the type"),
                ("{0.0,0.0,0.0,0.0,0.0}", 0, 1, "Should not be able to fix a collection of signeds with a collection of floats"),
                ("{true,false,true,false,true}", 0, 1, "Should not be able to fix a collection of signeds with a collection of booleans"),
                ("{0,0,0,0}", 0, 1, "Should not be able to fix a collection with a collection that is too small"),
                ("{0,0,0,0,0,0}", 0, 1, "Should not be able to fix a collection with a collection that is too large")
            });
            }
        }

        [Test()]
        public void BoolCollection()
        {
            var input_simple = @"
            Object1
            {
                field1 : bool[5] = VALUE;
            }
            ";

            var input_complex = @"
            Object1
            {
                field1 : bool[5];
            }
            Object2
            {
                field1 : Object1(field1 = VALUE);
            }
            ";

            foreach (var input in new string[] { input_simple, input_complex })
            {
                TestRunner.Run(input, new List<(string, int, int, string)>
            {
                ("{false,false,false,false,false}", 0, 0, "minimum value for u8"),
                ("{true,true,true,true,true}", 0, 0, "maximum value for u8"),
                ("-1", 0, 1, "should not be able to fix a collection with a single value"),
                ("1", 0, 1, "should not be able to fix a collection with a single value"),
                ("1.2345", 0, 1, "should not be able to fix a collection with a single value"),
                ("true", 0, 1, "should not be able to fix a collection with a single value"),
                ("false", 0, 1, "should not be able to fix a collection with a single value"),
                ("\"this is a string\"", 0, 1, "should not be able to fix a collection of booleans with a string value"),
                ("{0.0,0.0,0.0,0.0,0.0}", 0, 1, "Should not be able to fix a collection of booleans with a collection of floats"),
                ("{1,-1,5,6,-7}", 0, 1, "Should not be able to fix a collection of booleans with a collection of integers"),
                ("{false,false,false,false}", 0, 1, "Should not be able to fix a collection with a collection that is too small"),
                ("{true,true,true,true,true,true}", 0, 1, "Should not be able to fix a collection with a collection that is too large")
            });
            }
        }

        [Test()]
        public void FloatCollection()
        {
            var input_simple = @"
            Object1
            {
                field1 : f64[5] = VALUE;
            }
            ";

            var input_complex = @"
            Object1
            {
                field1 : f64[5];
            }
            Object2
            {
                field1 : Object1(field1 = VALUE);
            }
            ";

            foreach (var input in new string[] { input_simple, input_complex })
            {
                TestRunner.Run(input, new List<(string, int, int, string)>
            {
                ("{0.123456,0.123456,0.123456,0.123456,0.123456}", 0, 0, "Valid float value"),
                ("-1", 0, 1, "should not be able to fix a collection with a single value"),
                ("1", 0, 1, "should not be able to fix a collection with a single value"),
                ("1.2345", 0, 1, "should not be able to fix a collection with a single value"),
                ("true", 0, 1, "should not be able to fix a collection with a single value"),
                ("false", 0, 1, "should not be able to fix a collection with a single value"),
                ("\"this is a string\"", 0, 1, "should not be able to fix a collection of floats with a string value"),
                ("{0,1,2,3,4}", 0, 1, "Should not be able to fix a collection of floats with a collection of signeds"),
                ("{-1,-2,-3,-4,-5}", 0, 1, "Should not be able to fix a collection of floats with a collection of unsigneds"),
                ("{true,false,true,false,true}", 0, 1, "Should not be able to fix a collection of floats with a collection of booleans"),
                ("{0.123456,0.123456,0.123456,0.123456}", 0, 1, "Should not be able to fix a collection with a collection that is too small"),
                ("{0.123456,0.123456,0.123456,0.123456,0.123456, 0.123456}", 0, 1, "Should not be able to fix a collection with a collection that is too large")
            });
            }
        }

        [Test()]
        public void AsciiString()
        {
            var input_simple = @"
            Object1
            {
                field1 : ascii_string[6] = VALUE;
            }
            ";

            var input_complex = @"
            Object1
            {
                field1 : ascii_string[6];
            }
            Object2
            {
                field1 : Object1(field1 = VALUE);
            }
            ";

            foreach (var input in new string[] { input_simple, input_complex })
            {
                TestRunner.Run(input, new List<(string, int, int, string)>
            {
                ("\"string\"", 0, 0, "Valid string value"),
                ("-1", 0, 1, "should not be able to fix a string with a signed value"),
                ("1", 0, 1, "should not be able to fix a string with an unsigned value"),
                ("1.2345", 0, 1, "should not be able to fix a string with a float value"),
                ("true", 0, 1, "should not be able to fix a string with a boolean value"),
                ("false", 0, 1, "should not be able to fix a string with a boolean value"),
                ("{0,1,2,3,4}", 0, 1, "Should not be able to fix a string with a collection of signeds"),
                ("{-1,-2,-3,-4,-5}", 0, 1, "Should not be able to fix a string with a collection of unsigneds"),
                ("{true,false,true,false,true}", 0, 1, "Should not be able to fix a string with a collection of booleans"),
                ("\"strin\"", 0, 1, "Should not be able to fix a string with a string that is too short"),
                ("\"string1\"", 0, 1, "Should not be able to fix a string with a string that is too long")
            });
            }
        }

        // TODO: utf8_string once we figure out how that works
    }

    [TestFixture()]
    /* Test that we get an error when we try to fix a field that does not exist in the type */
    public class Existence
    {
        [Test()]
        public void KnownType()
        {
            var input = @"
Object1
{
    field1 : u8;
    field2 : f64
}

Object2 
{
    field3 : u16; // Let's name this one field3 to also test that we don't somehow accept the fixer because there is a field in the wrong object with the right field name
    field2 : Object2(field2 = 4);
}";

            TestRunner.Run(input, 0, 0);
        }

        [Test()]
        public void UknownType()
        {
            var input = @"
Object1
{
    field1 : u8;
    field2 : f64
}

Object2 
{
    field3 : u16; // Let's name this one field3 to also test that we don't somehow accept the fixer because there is a field in the wrong object with the right field name
    field2 : Object2(field3 = 4);
}";

            TestRunner.Run(input, 0, 1);
        }

        [Test()]
        public void KnownTypeEmbedded()
        {
            var input = @"
Object1
{
    field1 : u8;
    field2 : f64
}

Object2
{
    field1 : s32;
    embedded_field : Object1;
}

Object3 
{
    field : Object2(embedded_field.field2 = 4);
}";

            TestRunner.Run(input, 0, 0);
        }

        [Test()]
        public void UknownTypeEmbedded()
        {
            var input = @"
Object1
{
    field1 : u8;
    field2 : f64
}

Object2
{
    field1 : s32;
    embedded_field : Object1;
}

Object3 
{
    field3 : u16; // Let's name this one field3 to also test that we don't somehow accept the fixer because there is a field in the wrong object with the right field name
    field : Object2(embedded_field.field3 = 4);
}";

            TestRunner.Run(input, 0, 1);
        }
    }

    [TestFixture()]
    /* we don't support implicit sizing of collections. Although it is easier, I prefer the compile time double check we get from having an explicit collection size and an initializer with a certain size */
    public class ImplicitSizing
    {
        [Test()]
        public void CollectionOfFloats()
        {
            var input = @"
Object1
{
    field1 : f32[] = { 1.23, 2.34, 3.45 };
}";
          TestRunner.Run(input, 0, 1);
        }

        [Test()]
        public void CollectionOfIntegers()
        {
            var input = @"
Object1
{
    field1 : i64[] = { 1,2,3,4,5 };
}";
            TestRunner.Run(input, 0, 1);
        }

        [Test()]
        public void CollectionOfBooleans()
        {
            var input = @"
Object1
{
    field1 : bool[] = { true, false, true, true };
}";
            TestRunner.Run(input, 0, 1);
        }

        [Test()]
        public void AsciiString()
        {
            var input = @"
Object1
{
    field1 : ascii+string = ""SomeString"";
}";
            TestRunner.Run(input, 0, 1);
        }
    }
}

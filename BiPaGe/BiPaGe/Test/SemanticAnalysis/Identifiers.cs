using NUnit.Framework;
using Helpers;

namespace BiPaGe.Test.SemanticAnalysis.Identifiers
{
    /* Objects and enumeration do not need to be declared in the order in which they are
     * used. In these test cases we verify that no semantic errors/warning will be thrown
     * when a type is used before it is declared */
    [TestFixture()]
    public class Order
    {
        [Test()]
        public void InOrderObject()
        {
            var input = @"
Object1
{
    field1 : u8;
    field2 : f64
}

Object2 
{
    field1 : u16;
    field2 : Object1;
}";

            TestRunner.Run(input, 0, 0);
        }

        [Test()]
        public void OutOfOrderObject()
        {
            var input = @"
Object2 
{
    field1 : u16;
    field2 : Object1;
}

Object1
{
    field1 : u8;
    field2 : f64
}
";

            TestRunner.Run(input, 0, 0);
        }

        [Test()]
        public void InOrderEnum()
        {
            var input = @"
Enum : u32
{
    value1 = 1,
    value2 = 2
}

Object2 
{
    field1 : u16;
    field2 : Eunm;
}";
            TestRunner.Run(input, 0, 0);
        }

        [Test()]
        public void OutOfOrderEnum()
        {
            var input = @"
Object2 
{
    field1 : u16;
    field2 : Eunm;
}

Eunm : u32
{
    value1 = 1,
    value2 = 2
}";
            TestRunner.Run(input, 0, 0);
        }
    }

    [TestFixture()]
    /* Test that we get errors when we try to use type identifiers of types that are not defined */
    public class Existence
    {
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
    field1 : u16;
    field2 : Object3;
}";

            TestRunner.Run(input, 0, 1);
        }
    }

    [TestFixture()]    
    public class Uniqueness
    {
        [Test()]
        // All fields in an object should be unique
        public void FieldNameRepeated()
        {
            var input = @"
Object1
{
    field1 : u8;
    field1 : f64
}";

            TestRunner.Run(input, 0, 1);
        }

        [Test()]
        // Different objects may have fields with the same name
        public void DifferentOjectsSameFieldName()
        {
            var input = @"
Object1
{
    field1 : u8;
    field2 : f64
}

Object2
{
    field1: ascii_string[15];
    field2: bool;
}";
            TestRunner.Run(input, 0, 0);
        }

        [Test()]
        // Different objects may not have the same name
        public void UniqueOBjectNames()
        {
            var input = @"
Object1
{
    field1 : u8;
    field2 : f64
}

Object1
{
    field1: ascii_string[15];
    field2: bool;
}";
            TestRunner.Run(input, 0, 1);
        }
    }

    public class CyclicDependency
    {
        /* Object cannot have itself as a field */
        [Test()]
        public void Direct()
        {
            var input = @"
Object1
{
    field1 : u16;
    field2 : Object1;
}";

            TestRunner.Run(input, 0, 0);
        }

        /* Object cannot have a fieldtype that has itself as a field */
        [Test()]
        public void Indirect()
        {
            var input = @"
Object1
{
    field1 : u16;
    field2 : Object2;
}

Object2
{
    field1 : bool[5];
    u3;
    field2 : Object1;
}";

            TestRunner.Run(input, 0, 1);
        }
    }
}
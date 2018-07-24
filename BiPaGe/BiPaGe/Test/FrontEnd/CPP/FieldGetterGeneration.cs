using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace BiPaGe.Test.FrontEnd.CPP
{
    [TestFixture()]
    public class StandardWidthIntegersNoOffset
    {
        [Test()]
        public void Int8()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(8), 0, null);
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            Assert.AreEqual("const std::int8_t& TEST() const", declaration);
            Assert.AreEqual("return *reinterpret_cast<const std::int8_t*>(reinterpret_cast<const std::uint8_t*>(this));", body);
        }

        [Test()]
        public void Int16()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(16), 0, null);
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            Assert.AreEqual("const std::int16_t& TEST() const", declaration);
            Assert.AreEqual("return *reinterpret_cast<const std::int16_t*>(reinterpret_cast<const std::uint8_t*>(this));", body);
        }

        [Test()]
        public void Int32()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(32), 0, null);
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            Assert.AreEqual("const std::int32_t& TEST() const", declaration);
            Assert.AreEqual("return *reinterpret_cast<const std::int32_t*>(reinterpret_cast<const std::uint8_t*>(this));", body);
        }

        [Test()]
        public void Int64()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(64), 0, null);
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            Assert.AreEqual("const std::int64_t& TEST() const", declaration);
            Assert.AreEqual("return *reinterpret_cast<const std::int64_t*>(reinterpret_cast<const std::uint8_t*>(this));", body);
        }

        [Test()]
        public void Uint8()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(8), 0, null);
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            Assert.AreEqual("const std::uint8_t& TEST() const", declaration);
            Assert.AreEqual("return *reinterpret_cast<const std::uint8_t*>(reinterpret_cast<const std::uint8_t*>(this));", body);
        }

        [Test()]
        public void Uint16()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(16), 0, null);
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            Assert.AreEqual("const std::uint16_t& TEST() const", declaration);
            Assert.AreEqual("return *reinterpret_cast<const std::uint16_t*>(reinterpret_cast<const std::uint8_t*>(this));", body);
        }

        [Test()]
        public void Uint32()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(32), 0, null);
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            Assert.AreEqual("const std::uint32_t& TEST() const", declaration);
            Assert.AreEqual("return *reinterpret_cast<const std::uint32_t*>(reinterpret_cast<const std::uint8_t*>(this));", body);
        }

        [Test()]
        public void Uint64()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(64), 0, null);
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            Assert.AreEqual("const std::uint64_t& TEST() const", declaration);
            Assert.AreEqual("return *reinterpret_cast<const std::uint64_t*>(reinterpret_cast<const std::uint8_t*>(this));", body);
        }
    }

    public class StandardWidthIntegersStaticMultiByteOffset
    {
        [Test()]
        public void Int8()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(8), 16, null);
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            Assert.AreEqual("const std::int8_t& TEST() const", declaration);
            Assert.AreEqual("return *reinterpret_cast<const std::int8_t*>(reinterpret_cast<const std::uint8_t*>(this) + 2);", body);
        }

        [Test()]
        public void Int16()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(16), 8, null);
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            Assert.AreEqual("const std::int16_t& TEST() const", declaration);
            Assert.AreEqual("return *reinterpret_cast<const std::int16_t*>(reinterpret_cast<const std::uint8_t*>(this) + 1);", body);
        }

        [Test()]
        public void Int32()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(32), 24, null);
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            Assert.AreEqual("const std::int32_t& TEST() const", declaration);
            Assert.AreEqual("return *reinterpret_cast<const std::int32_t*>(reinterpret_cast<const std::uint8_t*>(this) + 3);", body);
        }

        [Test()]
        public void Int64()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(64), 128, null);
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            Assert.AreEqual("const std::int64_t& TEST() const", declaration);
            Assert.AreEqual("return *reinterpret_cast<const std::int64_t*>(reinterpret_cast<const std::uint8_t*>(this) + 16);", body);
        }

        [Test()]
        public void Uint8()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(8), 256, null);
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            Assert.AreEqual("const std::uint8_t& TEST() const", declaration);
            Assert.AreEqual("return *reinterpret_cast<const std::uint8_t*>(reinterpret_cast<const std::uint8_t*>(this) + 32);", body);
        }

        [Test()]
        public void Uint16()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(16), 40, null);
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            Assert.AreEqual("const std::uint16_t& TEST() const", declaration);
            Assert.AreEqual("return *reinterpret_cast<const std::uint16_t*>(reinterpret_cast<const std::uint8_t*>(this) + 5);", body);
        }

        [Test()]
        public void Uint32()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(32), 72, null);
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            Assert.AreEqual("const std::uint32_t& TEST() const", declaration);
            Assert.AreEqual("return *reinterpret_cast<const std::uint32_t*>(reinterpret_cast<const std::uint8_t*>(this) + 9);", body);
        }

        [Test()]
        public void Uint64()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(64), 80, null);
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            Assert.AreEqual("const std::uint64_t& TEST() const", declaration);
            Assert.AreEqual("return *reinterpret_cast<const std::uint64_t*>(reinterpret_cast<const std::uint8_t*>(this) + 10);", body);
        }
    }

    public class StandardWidthIntegersStaticOffset
    {
        /* 
         * Tests getter generation for standard sized integers that are not located on a byte offset. For example:
         * Object { a : bool, b: int8 }
         * In this example <b> is an 8 bit signed integer located on bits 1-7 of byte zero and bit 1 of byte 2:
         * [-------*][*******-]
         * The interesting things to test here are:
         * - The function should return by value (because we have to mask)
         * - The right encapsulating type (the standard type that encompasses all bits of the type) is used
         * - Masking and shifting is done right
         */
        [Test()]
        public void Int8()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(8), 4, null);
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            // Return by value
            Assert.AreEqual("std::int8_t TEST() const", declaration);
            // The encapsulating type should be a 16 bit integer. We should then mask out the most and least significant nibble and shift four places right. The resulting value needs to 
            // be cast back to the expected type: int8
            Assert.AreEqual("return static_cast<std::int8_t>(((*reinterpret_cast<const std::int16_t*>(reinterpret_cast<const std::uint8_t*>(this)) & 0xff0) >> 4));", body);
        }

        [Test()]
        public void Int16()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(16), 22, null);
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            // Return by value
            Assert.AreEqual("std::int16_t TEST() const", declaration);
            // The encapsulating type should be a 32 bit integer which has a byte offset of 2 (the first 16 bits of the offset).
            // We should mask out the least significant 6 bits and the most significant 10 bits and shift six places right. The resulting value needs to 
            // be cast back to the expected type: int16            
            Assert.AreEqual("return static_cast<std::int16_t>(((*reinterpret_cast<const std::int32_t*>(reinterpret_cast<const std::uint8_t*>(this) + 2) & 0x3fffc0) >> 6));", body);
        }

        [Test()]
        public void Int32()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(32), 523, null);
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            // Return by value
            Assert.AreEqual("std::int32_t TEST() const", declaration);
            // The encapsulating type should be a 64 bit integer which has a byte offset of 65 (the first 520 bits of the offset).
            // We should mask out the least significant 3 bits and the most significant 5 bits and shift 3 places right. The resulting value needs to 
            // be cast back to the expected type: int32
            ;
            Assert.AreEqual("return static_cast<std::int32_t>(((*reinterpret_cast<const std::int64_t*>(reinterpret_cast<const std::uint8_t*>(this) + 65) & 0x7fffffff8) >> 3));", body);
        }

        [Test()]
        public void Uint8()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(8), 4, null);
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            // Return by value
            Assert.AreEqual("std::uint8_t TEST() const", declaration);
            // The encapsulating type should be a 16 bit integer. We should then mask out the most and least significant nibble and shift four places right. The resulting value needs to 
            // be cast back to the expected type: int8
            Assert.AreEqual("return static_cast<std::uint8_t>(((*reinterpret_cast<const std::uint16_t*>(reinterpret_cast<const std::uint8_t*>(this)) & 0xff0) >> 4));", body);
        }

        [Test()]
        public void Uint16()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(16), 22, null);
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            // Return by value
            Assert.AreEqual("std::uint16_t TEST() const", declaration);
            // The encapsulating type should be a 32 bit integer which has a byte offset of 2 (the first 16 bits of the offset).
            // We should mask out the least significant 6 bits and the most significant 10 bits and shift six places right. The resulting value needs to 
            // be cast back to the expected type: int16            
            Assert.AreEqual("return static_cast<std::uint16_t>(((*reinterpret_cast<const std::uint32_t*>(reinterpret_cast<const std::uint8_t*>(this) + 2) & 0x3fffc0) >> 6));", body);
        }

        [Test()]
        public void Uint32()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(32), 523, null);
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            // Return by value
            Assert.AreEqual("std::uint32_t TEST() const", declaration);
            // The encapsulating type should be a 64 bit integer which has a byte offset of 65 (the first 520 bits of the offset).
            // We should mask out the least significant 3 bits and the most significant 5 bits and shift 3 places right. The resulting value needs to 
            // be cast back to the expected type: int32
            ;
            Assert.AreEqual("return static_cast<std::uint32_t>(((*reinterpret_cast<const std::uint64_t*>(reinterpret_cast<const std::uint8_t*>(this) + 65) & 0x7fffffff8) >> 3));", body);
        }

        /*
        Note that we don't have a test for 64 bit integral types with a non-byte algined offset, because we don't support that yet. Our current methodology of using an encapsulating type 
        and masking out what we don't need won't work for 64 bit types as there is no standard 128 bit type. We could of course solve this by casting to two 64 bit type (the upper and lower halve)
        masking each one and constructing the required 64 bit type from that, but as I don't expect this to be required, we have omitted this for now
        */
    }

    public class NonStandardWidthIntegersNoOffset
    {
        /* Here we test getters for non standard sized integers without an offset.
         * As we always need to mask these integers, we should always return by value 
         */

        [Test()]
        public void Int3()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(3), 0, null);
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            Assert.AreEqual("std::int8_t TEST() const", declaration);
            Assert.AreEqual("return (*reinterpret_cast<const std::int8_t*>(reinterpret_cast<const std::uint8_t*>(this)) & 0x7);", body);
        }

        [Test()]
        public void Int12()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(12), 0, null);
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            Assert.AreEqual("std::int16_t TEST() const", declaration);
            Assert.AreEqual("return (*reinterpret_cast<const std::int16_t*>(reinterpret_cast<const std::uint8_t*>(this)) & 0xfff);", body);
        }

        [Test()]
        public void Int17()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(17), 0, null);
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            Assert.AreEqual("std::int32_t TEST() const", declaration);
            Assert.AreEqual("return (*reinterpret_cast<const std::int32_t*>(reinterpret_cast<const std::uint8_t*>(this)) & 0x1ffff);", body);
        }

        [Test()]
        public void Int39()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(39), 0, null);
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            Assert.AreEqual("std::int64_t TEST() const", declaration);
            Assert.AreEqual("return (*reinterpret_cast<const std::int64_t*>(reinterpret_cast<const std::uint8_t*>(this)) & 0x7fffffffff);", body);
        }

        [Test()]
        public void Uint5()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(5), 0, null);
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            Assert.AreEqual("std::uint8_t TEST() const", declaration);
            Assert.AreEqual("return (*reinterpret_cast<const std::uint8_t*>(reinterpret_cast<const std::uint8_t*>(this)) & 0x1f);", body);
        }

        [Test()]
        public void Uint15()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(15), 0, null);
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            Assert.AreEqual("std::uint16_t TEST() const", declaration);
            Assert.AreEqual("return (*reinterpret_cast<const std::uint16_t*>(reinterpret_cast<const std::uint8_t*>(this)) & 0x7fff);", body);
        }

        [Test()]
        public void Uint24()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(24), 0, null);
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            Assert.AreEqual("std::uint32_t TEST() const", declaration);
            Assert.AreEqual("return (*reinterpret_cast<const std::uint32_t*>(reinterpret_cast<const std::uint8_t*>(this)) & 0xffffff);", body);
        }

        [Test()]
        public void Uint57()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(57), 0, null);
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            Assert.AreEqual("std::uint64_t TEST() const", declaration);
            Assert.AreEqual("return (*reinterpret_cast<const std::uint64_t*>(reinterpret_cast<const std::uint8_t*>(this)) & 0x1ffffffffffffff);", body);
        }
    }

    public class StandardWidthIntegersDynamicOffset
    {
        [Test()]
        public void Int8()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(8), 0, "SomeOtherField");
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            Assert.AreEqual("const std::int8_t& TEST() const", declaration);
            Assert.AreEqual("return *reinterpret_cast<const std::int8_t*>(SomeOtherField().end());", body);
        }

        [Test()]
        public void Int16()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(16), 0, "SomeOtherField");
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            Assert.AreEqual("const std::int16_t& TEST() const", declaration);
            Assert.AreEqual("return *reinterpret_cast<const std::int16_t*>(SomeOtherField().end());", body);
        }

        [Test()]
        public void Int32()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(32), 0, "SomeOtherField");
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            Assert.AreEqual("const std::int32_t& TEST() const", declaration);
            Assert.AreEqual("return *reinterpret_cast<const std::int32_t*>(SomeOtherField().end());", body);
        }

        [Test()]
        public void Int64()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(64), 0, "SomeOtherField");
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            Assert.AreEqual("const std::int64_t& TEST() const", declaration);
            Assert.AreEqual("return *reinterpret_cast<const std::int64_t*>(SomeOtherField().end());", body);
        }

        [Test()]
        public void Uint8()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(8), 0, "SomeOtherField");
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            Assert.AreEqual("const std::uint8_t& TEST() const", declaration);
            Assert.AreEqual("return *reinterpret_cast<const std::uint8_t*>(SomeOtherField().end());", body);
        }

        [Test()]
        public void Uint16()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(16), 0, "SomeOtherField");
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            Assert.AreEqual("const std::uint16_t& TEST() const", declaration);
            Assert.AreEqual("return *reinterpret_cast<const std::uint16_t*>(SomeOtherField().end());", body);
        }

        [Test()]
        public void Uint32()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(32), 0, "SomeOtherField");
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            Assert.AreEqual("const std::uint32_t& TEST() const", declaration);
            Assert.AreEqual("return *reinterpret_cast<const std::uint32_t*>(SomeOtherField().end());", body);
        }

        [Test()]
        public void Uint64()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(64), 0, "SomeOtherField");
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            Assert.AreEqual("const std::uint64_t& TEST() const", declaration);
            Assert.AreEqual("return *reinterpret_cast<const std::uint64_t*>(SomeOtherField().end());", body);
        }
    }

    public class StandardWidthIntegersStaticAndDynamicOffset
    {
        [Test()]
        public void Int8()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(8), 4, "SomeOtherField");
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            // Return by value
            Assert.AreEqual("std::int8_t TEST() const", declaration);
            // The encapsulating type should be a 16 bit integer. We should then mask out the most and least significant nibble and shift four places right. The resulting value needs to 
            // be cast back to the expected type: int8
            Assert.AreEqual("return static_cast<std::int8_t>(((*reinterpret_cast<const std::int16_t*>(SomeOtherField().end()) & 0xff0) >> 4));", body);
        }

        [Test()]
        public void Int16()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(16), 22, "SomeOtherField");
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            // Return by value
            Assert.AreEqual("std::int16_t TEST() const", declaration);
            // The encapsulating type should be a 32 bit integer which has a byte offset of 2 (the first 16 bits of the offset).
            // We should mask out the least significant 6 bits and the most significant 10 bits and shift six places right. The resulting value needs to 
            // be cast back to the expected type: int16            
            Assert.AreEqual("return static_cast<std::int16_t>(((*reinterpret_cast<const std::int32_t*>(SomeOtherField().end() + 2) & 0x3fffc0) >> 6));", body);
        }

        [Test()]
        public void Int32()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(32), 523, "SomeOtherField");
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            // Return by value
            Assert.AreEqual("std::int32_t TEST() const", declaration);
            // The encapsulating type should be a 64 bit integer which has a byte offset of 65 (the first 520 bits of the offset).
            // We should mask out the least significant 3 bits and the most significant 5 bits and shift 3 places right. The resulting value needs to 
            // be cast back to the expected type: int32
            ;
            Assert.AreEqual("return static_cast<std::int32_t>(((*reinterpret_cast<const std::int64_t*>(SomeOtherField().end() + 65) & 0x7fffffff8) >> 3));", body);
        }

        [Test()]
        public void Uint8()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(8), 4, "SomeOtherField");
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            // Return by value
            Assert.AreEqual("std::uint8_t TEST() const", declaration);
            // The encapsulating type should be a 16 bit integer. We should then mask out the most and least significant nibble and shift four places right. The resulting value needs to 
            // be cast back to the expected type: int8
            Assert.AreEqual("return static_cast<std::uint8_t>(((*reinterpret_cast<const std::uint16_t*>(SomeOtherField().end()) & 0xff0) >> 4));", body);
        }

        [Test()]
        public void Uint16()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(16), 22, "SomeOtherField");
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            // Return by value
            Assert.AreEqual("std::uint16_t TEST() const", declaration);
            // The encapsulating type should be a 32 bit integer which has a byte offset of 2 (the first 16 bits of the offset).
            // We should mask out the least significant 6 bits and the most significant 10 bits and shift six places right. The resulting value needs to 
            // be cast back to the expected type: int16            
            Assert.AreEqual("return static_cast<std::uint16_t>(((*reinterpret_cast<const std::uint32_t*>(SomeOtherField().end() + 2) & 0x3fffc0) >> 6));", body);
        }

        [Test()]
        public void Uint32()
        {
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator();
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(32), 523, "SomeOtherField");
            var declaration = gen.GetDeclaration(field);
            var body = gen.GetBody(field);

            // Return by value
            Assert.AreEqual("std::uint32_t TEST() const", declaration);
            // The encapsulating type should be a 64 bit integer which has a byte offset of 65 (the first 520 bits of the offset).
            // We should mask out the least significant 3 bits and the most significant 5 bits and shift 3 places right. The resulting value needs to 
            // be cast back to the expected type: int32
            ;
            Assert.AreEqual("return static_cast<std::uint32_t>(((*reinterpret_cast<const std::uint64_t*>(SomeOtherField().end() + 65) & 0x7fffffff8) >> 3));", body);
        }
    }
}

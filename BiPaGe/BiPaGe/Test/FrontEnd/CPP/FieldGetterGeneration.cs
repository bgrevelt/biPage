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
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(8), 0, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("const std::int8_t& TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this);",
                "const std::int8_t* captured_data = reinterpret_cast<const std::int8_t*>(data_offset);",
                "return *captured_data;"}, body);
        }

        [Test()]
        public void Int16()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(16), 0, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("const std::int16_t& TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this);",
                "const std::int16_t* captured_data = reinterpret_cast<const std::int16_t*>(data_offset);",
                "return *captured_data;"}, body);
        }

        [Test()]
        public void Int32()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(32), 0, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("const std::int32_t& TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this);",
                "const std::int32_t* captured_data = reinterpret_cast<const std::int32_t*>(data_offset);",
                "return *captured_data;"}, body);
        }

        [Test()]
        public void Int64()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(64), 0, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("const std::int64_t& TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this);",
                "const std::int64_t* captured_data = reinterpret_cast<const std::int64_t*>(data_offset);",
                "return *captured_data;"}, body);
        }

        [Test()]
        public void Uint8()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(8), 0, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("const std::uint8_t& TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this);",
                "return *data_offset;"}, body);
        }

        [Test()]
        public void Uint16()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(16), 0, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("const std::uint16_t& TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this);",
                "const std::uint16_t* captured_data = reinterpret_cast<const std::uint16_t*>(data_offset);",
                "return *captured_data;"}, body);
        }

        [Test()]
        public void Uint32()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(32), 0, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("const std::uint32_t& TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this);",
                "const std::uint32_t* captured_data = reinterpret_cast<const std::uint32_t*>(data_offset);",
                "return *captured_data;"}, body);
        }

        [Test()]
        public void Uint64()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(64), 0, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("const std::uint64_t& TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this);",
                "const std::uint64_t* captured_data = reinterpret_cast<const std::uint64_t*>(data_offset);",
                "return *captured_data;"}, body);
        }
    }

    public class StandardWidthIntegersStaticMultiByteOffset
    {
        [Test()]
        public void Int8()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(8), 16, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("const std::int8_t& TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this) + 2;",
                "const std::int8_t* captured_data = reinterpret_cast<const std::int8_t*>(data_offset);",
                "return *captured_data;"}, body);
        }

        [Test()]
        public void Int16()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(16), 8, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("const std::int16_t& TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this) + 1;",
                "const std::int16_t* captured_data = reinterpret_cast<const std::int16_t*>(data_offset);",
                "return *captured_data;"}, body);
        }

        [Test()]
        public void Int32()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(32), 24, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("const std::int32_t& TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this) + 3;",
                "const std::int32_t* captured_data = reinterpret_cast<const std::int32_t*>(data_offset);",
                "return *captured_data;"}, body);
        }

        [Test()]
        public void Int64()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(64), 128, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("const std::int64_t& TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this) + 16;",
                "const std::int64_t* captured_data = reinterpret_cast<const std::int64_t*>(data_offset);",
                "return *captured_data;"}, body);
        }

        [Test()]
        public void Uint8()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(8), 256, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("const std::uint8_t& TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this) + 32;",
                "return *data_offset;"}, body);
        }

        [Test()]
        public void Uint16()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(16), 40, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("const std::uint16_t& TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this) + 5;",
                "const std::uint16_t* captured_data = reinterpret_cast<const std::uint16_t*>(data_offset);",
                "return *captured_data;"}, body);
        }

        [Test()]
        public void Uint32()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(32), 72, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("const std::uint32_t& TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this) + 9;",
                "const std::uint32_t* captured_data = reinterpret_cast<const std::uint32_t*>(data_offset);",
                "return *captured_data;"}, body);
        }

        [Test()]
        public void Uint64()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(64), 80, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("const std::uint64_t& TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this) + 10;",
                "const std::uint64_t* captured_data = reinterpret_cast<const std::uint64_t*>(data_offset);",
                "return *captured_data;"}, body);
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
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(8), 4, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            // Return by value
            Assert.AreEqual("std::int8_t TEST() const", declaration);
            // The encapsulating type should be a 16 bit integer. We should then mask out the most and least significant nibble and shift four places right. The resulting value needs to 
            // be cast back to the expected type: int8
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this);",
                "const std::int16_t* captured_data = reinterpret_cast<const std::int16_t*>(data_offset);",
                "std::int16_t masked_data = (*captured_data & 0xff0) >> 4;",
                "std::int8_t typed_data = static_cast<std::int8_t>(masked_data);",
                "return typed_data;"}, body);
        }

        [Test()]
        public void Int16()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(16), 22, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            // Return by value
            Assert.AreEqual("std::int16_t TEST() const", declaration);
            // The encapsulating type should be a 32 bit integer which has a byte offset of 2 (the first 16 bits of the offset).
            // We should mask out the least significant 6 bits and the most significant 10 bits and shift six places right. The resulting value needs to 
            // be cast back to the expected type: int16   
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this) + 2;",
                "const std::int32_t* captured_data = reinterpret_cast<const std::int32_t*>(data_offset);",
                "std::int32_t masked_data = (*captured_data & 0x3fffc0) >> 6;",
                "std::int16_t typed_data = static_cast<std::int16_t>(masked_data);",
                "return typed_data;"}, body);
        }

        [Test()]
        public void Int32()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(32), 523, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            // Return by value
            Assert.AreEqual("std::int32_t TEST() const", declaration);
            // The encapsulating type should be a 64 bit integer which has a byte offset of 65 (the first 520 bits of the offset).
            // We should mask out the least significant 3 bits and the most significant 5 bits and shift 3 places right. The resulting value needs to 
            // be cast back to the expected type: int32
            ;
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this) + 65;",
                "const std::int64_t* captured_data = reinterpret_cast<const std::int64_t*>(data_offset);",
                "std::int64_t masked_data = (*captured_data & 0x7fffffff8) >> 3;",
                "std::int32_t typed_data = static_cast<std::int32_t>(masked_data);",
                "return typed_data;"}, body);
        }

        [Test()]
        public void Uint8()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(8), 4, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            // Return by value
            Assert.AreEqual("std::uint8_t TEST() const", declaration);
            // The encapsulating type should be a 16 bit integer. We should then mask out the most and least significant nibble and shift four places right. The resulting value needs to 
            // be cast back to the expected type: int8
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this);",
                "const std::uint16_t* captured_data = reinterpret_cast<const std::uint16_t*>(data_offset);",
                "std::uint16_t masked_data = (*captured_data & 0xff0) >> 4;",
                "std::uint8_t typed_data = static_cast<std::uint8_t>(masked_data);",
                "return typed_data;"}, body);
        }

        [Test()]
        public void Uint16()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(16), 22, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            // Return by value
            Assert.AreEqual("std::uint16_t TEST() const", declaration);
            // The encapsulating type should be a 32 bit integer which has a byte offset of 2 (the first 16 bits of the offset).
            // We should mask out the least significant 6 bits and the most significant 10 bits and shift six places right. The resulting value needs to 
            // be cast back to the expected type: int16            
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this) + 2;",
                "const std::uint32_t* captured_data = reinterpret_cast<const std::uint32_t*>(data_offset);",
                "std::uint32_t masked_data = (*captured_data & 0x3fffc0) >> 6;",
                "std::uint16_t typed_data = static_cast<std::uint16_t>(masked_data);",
                "return typed_data;"}, body);
        }

        [Test()]
        public void Uint32()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(32), 523, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            // Return by value
            Assert.AreEqual("std::uint32_t TEST() const", declaration);
            // The encapsulating type should be a 64 bit integer which has a byte offset of 65 (the first 520 bits of the offset).
            // We should mask out the least significant 3 bits and the most significant 5 bits and shift 3 places right. The resulting value needs to 
            // be cast back to the expected type: int32
            ;
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this) + 65;",
                "const std::uint64_t* captured_data = reinterpret_cast<const std::uint64_t*>(data_offset);",
                "std::uint64_t masked_data = (*captured_data & 0x7fffffff8) >> 3;",
                "std::uint32_t typed_data = static_cast<std::uint32_t>(masked_data);",
                "return typed_data;"}, body);
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
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(3), 0, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("std::int8_t TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this);",
                "const std::int8_t* captured_data = reinterpret_cast<const std::int8_t*>(data_offset);",
                "bool sign_bit = (*captured_data & 0x4) == 0x4;",
                "std::int8_t masked_data = (*captured_data & 0x3);",
                "std::int8_t signed_data = masked_data | (sign_bit ? 0xfc : 0);",
                "return signed_data;"}, body);
        }

        [Test()]
        public void Int12()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(12), 0, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("std::int16_t TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this);",
                "const std::int16_t* captured_data = reinterpret_cast<const std::int16_t*>(data_offset);",
                "bool sign_bit = (*captured_data & 0x800) == 0x800;",
                "std::int16_t masked_data = (*captured_data & 0x7ff);",
                "std::int16_t signed_data = masked_data | (sign_bit ? 0xf800 : 0);",
                "return signed_data;"}, body);
        }

        [Test()]
        public void Int17()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(17), 0, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("std::int32_t TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this);",
                "const std::int32_t* captured_data = reinterpret_cast<const std::int32_t*>(data_offset);",
                "bool sign_bit = (*captured_data & 0x10000) == 0x10000;",
                "std::int32_t masked_data = (*captured_data & 0xffff);",
                "std::int32_t signed_data = masked_data | (sign_bit ? 0xffff0000 : 0);",
                "return signed_data;"}, body);
        }

        [Test()]
        public void Int39()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(39), 0, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("std::int64_t TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this);",
                "const std::int64_t* captured_data = reinterpret_cast<const std::int64_t*>(data_offset);",
                "bool sign_bit = (*captured_data & 0x4000000000) == 0x4000000000;",
                "std::int64_t masked_data = (*captured_data & 0x3fffffffff);",
                "std::int64_t signed_data = masked_data | (sign_bit ? 0xffffffc000000000 : 0);",
                "return signed_data;"}, body);
        }

        [Test()]
        public void Uint5()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(5), 0, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("std::uint8_t TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this);",
                "std::uint8_t masked_data = (*data_offset & 0x1f);",
                "return masked_data;"}, body);
        }

        [Test()]
        public void Uint15()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(15), 0, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("std::uint16_t TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this);",
                "const std::uint16_t* captured_data = reinterpret_cast<const std::uint16_t*>(data_offset);",
                "std::uint16_t masked_data = (*captured_data & 0x7fff);",
                "return masked_data;"}, body);
        }

        [Test()]
        public void Uint24()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(24), 0, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("std::uint32_t TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this);",
                "const std::uint32_t* captured_data = reinterpret_cast<const std::uint32_t*>(data_offset);",
                "std::uint32_t masked_data = (*captured_data & 0xffffff);",
                "return masked_data;"}, body);
        }

        [Test()]
        public void Uint57()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(57), 0, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("std::uint64_t TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this);",
                "const std::uint64_t* captured_data = reinterpret_cast<const std::uint64_t*>(data_offset);",
                "std::uint64_t masked_data = (*captured_data & 0x1ffffffffffffff);",
                "return masked_data;"}, body);
        }
    }

    public class StandardWidthIntegersDynamicOffset
    {
        [Test()]
        public void Int8()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(8), 0, "SomeOtherField");
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("const std::int8_t& TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = SomeOtherField().end();",
                "const std::int8_t* captured_data = reinterpret_cast<const std::int8_t*>(data_offset);",
                "return *captured_data;"}, body);
        }

        [Test()]
        public void Int16()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(16), 0, "SomeOtherField");
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("const std::int16_t& TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = SomeOtherField().end();",
                "const std::int16_t* captured_data = reinterpret_cast<const std::int16_t*>(data_offset);",
                "return *captured_data;"}, body);
        }

        [Test()]
        public void Int32()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(32), 0, "SomeOtherField");
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("const std::int32_t& TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = SomeOtherField().end();",
                "const std::int32_t* captured_data = reinterpret_cast<const std::int32_t*>(data_offset);",
                "return *captured_data;"}, body);
        }

        [Test()]
        public void Int64()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(64), 0, "SomeOtherField");
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("const std::int64_t& TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = SomeOtherField().end();",
                "const std::int64_t* captured_data = reinterpret_cast<const std::int64_t*>(data_offset);",
                "return *captured_data;"}, body);
        }

        [Test()]
        public void Uint8()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(8), 0, "SomeOtherField");
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("const std::uint8_t& TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = SomeOtherField().end();",
                "return *data_offset;"}, body);
        }

        [Test()]
        public void Uint16()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(16), 0, "SomeOtherField");
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("const std::uint16_t& TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = SomeOtherField().end();",
                "const std::uint16_t* captured_data = reinterpret_cast<const std::uint16_t*>(data_offset);",
                "return *captured_data;"}, body);
        }

        [Test()]
        public void Uint32()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(32), 0, "SomeOtherField");
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("const std::uint32_t& TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = SomeOtherField().end();",
                "const std::uint32_t* captured_data = reinterpret_cast<const std::uint32_t*>(data_offset);",
                "return *captured_data;"}, body);
        }

        [Test()]
        public void Uint64()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(64), 0, "SomeOtherField");
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("const std::uint64_t& TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = SomeOtherField().end();",
                "const std::uint64_t* captured_data = reinterpret_cast<const std::uint64_t*>(data_offset);",
                "return *captured_data;"}, body);
        }
    }

    public class NonStandardWidthIntegersStaticOffset
    {
        /* What we want to test here
         * Do we select the rigt capture type
            The capture type depends on the size of the type AND the bit offset from a byte boundary. I don't know how to word that better, so here's an example for a 5 bit integer
            With an offset of 10 bits, it will be algined in memory like this:
            [-*****--][--------]
            meaning we can capture all the data in a single byte. 
            If, on the other hand, the offset is 5 bits, the data will be algined in memory like this
            [------**][***-----]
            and we need two bytes to capture the data
         * Do we apply the right masking and shifting
         * For signed types: Do we properly mask out the sign bit
         */

        [Test()]
        public void Int5()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(5), 4, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            // Return by value
            Assert.AreEqual("std::int8_t TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this);",
                "const std::int16_t* captured_data = reinterpret_cast<const std::int16_t*>(data_offset);",                
                "bool sign_bit = (*captured_data & 0x100) == 0x100;",
                "std::int16_t masked_data = (*captured_data & 0xf0) >> 4;",
                "std::int16_t signed_data = masked_data | (sign_bit ? 0xfff0 : 0);",
                "std::int8_t typed_data = static_cast<std::int8_t>(signed_data);",
                "return typed_data;"}, body);
        }

        [Test()]
        public void Int9()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(9), 22, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            // Return by value
            Assert.AreEqual("std::int16_t TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this) + 2;",
                "const std::int16_t* captured_data = reinterpret_cast<const std::int16_t*>(data_offset);",
                "bool sign_bit = (*captured_data & 0x4000) == 0x4000;",
                "std::int16_t masked_data = (*captured_data & 0x3fc0) >> 6;",
                "std::int16_t signed_data = masked_data | (sign_bit ? 0xff00 : 0);",
                "return signed_data;"}, body);
        }

        [Test()]
        public void Int35()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(35), 523, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            // Return by value
            Assert.AreEqual("std::int64_t TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this) + 65;",
                "const std::int64_t* captured_data = reinterpret_cast<const std::int64_t*>(data_offset);",
                "bool sign_bit = (*captured_data & 0x2000000000) == 0x2000000000;",
                "std::int64_t masked_data = (*captured_data & 0x1ffffffff8) >> 3;",
                "std::int64_t signed_data = masked_data | (sign_bit ? 0xfffffffc00000000 : 0);",
                "return signed_data;"}, body);
        }

        [Test()]
        public void Uint4()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(4), 4, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            // Return by value
            Assert.AreEqual("std::uint8_t TEST() const", declaration);
            // The encapsulating type should be a 16 bit integer. We should then mask out the most and least significant nibble and shift four places right. The resulting value needs to 
            // be cast back to the expected type: int8
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this);",
                "std::uint8_t masked_data = (*data_offset & 0xf0) >> 4;",
                "return masked_data;"}, body);
        }

        [Test()]
        public void Uint12()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(12), 22, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            // Return by value
            Assert.AreEqual("std::uint16_t TEST() const", declaration);     
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this) + 2;",
                "const std::uint32_t* captured_data = reinterpret_cast<const std::uint32_t*>(data_offset);",
                "std::uint32_t masked_data = (*captured_data & 0x3ffc0) >> 6;",
                "std::uint16_t typed_data = static_cast<std::uint16_t>(masked_data);",
                "return typed_data;"}, body);
        }

        [Test()]
        public void Uint19()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(19), 523, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            // Return by value
            Assert.AreEqual("std::uint32_t TEST() const", declaration);
            ;
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this) + 65;",
                "const std::uint32_t* captured_data = reinterpret_cast<const std::uint32_t*>(data_offset);",
                "std::uint32_t masked_data = (*captured_data & 0x3ffff8) >> 3;",
                "return masked_data;"}, body);
        }

        /*
        Note that we don't have a test for 64 bit integral types with a non-byte algined offset, because we don't support that yet. Our current methodology of using an encapsulating type 
        and masking out what we don't need won't work for 64 bit types as there is no standard 128 bit type. We could of course solve this by casting to two 64 bit type (the upper and lower halve)
        masking each one and constructing the required 64 bit type from that, but as I don't expect this to be required, we have omitted this for now
        */
    }

    public class NonStandardWidthIntegersMixedOffset
    {
        /* What we want to test here
         * Do we select the rigt capture type
            The capture type depends on the size of the type AND the bit offset from a byte boundary. I don't know how to word that better, so here's an example for a 5 bit integer
            With an offset of 10 bits, it will be algined in memory like this:
            [-*****--][--------]
            meaning we can capture all the data in a single byte. 
            If, on the other hand, the offset is 5 bits, the data will be algined in memory like this
            [------**][***-----]
            and we need two bytes to capture the data
         * Do we apply the right masking and shifting
         * For signed types: Do we properly mask out the sign bit
         */

        [Test()]
        public void Int5()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(5), 4, "SomeOtherField");
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            // Return by value
            Assert.AreEqual("std::int8_t TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = SomeOtherField().end();",
                "const std::int16_t* captured_data = reinterpret_cast<const std::int16_t*>(data_offset);",
                "bool sign_bit = (*captured_data & 0x100) == 0x100;",
                "std::int16_t masked_data = (*captured_data & 0xf0) >> 4;",
                "std::int16_t signed_data = masked_data | (sign_bit ? 0xfff0 : 0);",
                "std::int8_t typed_data = static_cast<std::int8_t>(signed_data);",
                "return typed_data;"}, body);
        }

        [Test()]
        public void Int9()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(9), 22, "SomeOtherField");
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            // Return by value
            Assert.AreEqual("std::int16_t TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = SomeOtherField().end() + 2;",
                "const std::int16_t* captured_data = reinterpret_cast<const std::int16_t*>(data_offset);",
                "bool sign_bit = (*captured_data & 0x4000) == 0x4000;",
                "std::int16_t masked_data = (*captured_data & 0x3fc0) >> 6;",
                "std::int16_t signed_data = masked_data | (sign_bit ? 0xff00 : 0);",
                "return signed_data;"}, body);
        }

        [Test()]
        public void Int35()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(35), 523, "SomeOtherField");
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            // Return by value
            Assert.AreEqual("std::int64_t TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = SomeOtherField().end() + 65;",
                "const std::int64_t* captured_data = reinterpret_cast<const std::int64_t*>(data_offset);",
                "bool sign_bit = (*captured_data & 0x2000000000) == 0x2000000000;",
                "std::int64_t masked_data = (*captured_data & 0x1ffffffff8) >> 3;",
                "std::int64_t signed_data = masked_data | (sign_bit ? 0xfffffffc00000000 : 0);",
                "return signed_data;"}, body);
        }

        [Test()]
        public void Uint4()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(4), 4, "SomeOtherField");
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            // Return by value
            Assert.AreEqual("std::uint8_t TEST() const", declaration);
            // The encapsulating type should be a 16 bit integer. We should then mask out the most and least significant nibble and shift four places right. The resulting value needs to 
            // be cast back to the expected type: int8
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = SomeOtherField().end();",
                "std::uint8_t masked_data = (*data_offset & 0xf0) >> 4;",
                "return masked_data;"}, body);
        }

        [Test()]
        public void Uint12()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(12), 22, "SomeOtherField");
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            // Return by value
            Assert.AreEqual("std::uint16_t TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = SomeOtherField().end() + 2;",
                "const std::uint32_t* captured_data = reinterpret_cast<const std::uint32_t*>(data_offset);",
                "std::uint32_t masked_data = (*captured_data & 0x3ffc0) >> 6;",
                "std::uint16_t typed_data = static_cast<std::uint16_t>(masked_data);",
                "return typed_data;"}, body);
        }

        [Test()]
        public void Uint19()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(19), 523, "SomeOtherField");
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            // Return by value
            Assert.AreEqual("std::uint32_t TEST() const", declaration);
            ;
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = SomeOtherField().end() + 65;",
                "const std::uint32_t* captured_data = reinterpret_cast<const std::uint32_t*>(data_offset);",
                "std::uint32_t masked_data = (*captured_data & 0x3ffff8) >> 3;",
                "return masked_data;"}, body);
        }

        /*
        Note that we don't have a test for 64 bit integral types with a non-byte algined offset, because we don't support that yet. Our current methodology of using an encapsulating type 
        and masking out what we don't need won't work for 64 bit types as there is no standard 128 bit type. We could of course solve this by casting to two 64 bit type (the upper and lower halve)
        masking each one and constructing the required 64 bit type from that, but as I don't expect this to be required, we have omitted this for now
        */
    }

    // TODO: non-standard width with offsets

    public class StandardWidthIntegersStaticAndDynamicOffset
    {
        [Test()]
        public void Int8()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(8), 4, "SomeOtherField");
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            // Return by value
            Assert.AreEqual("std::int8_t TEST() const", declaration);
            // The encapsulating type should be a 16 bit integer. We should then mask out the most and least significant nibble and shift four places right. The resulting value needs to 
            // be cast back to the expected type: int8
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = SomeOtherField().end();",
                "const std::int16_t* captured_data = reinterpret_cast<const std::int16_t*>(data_offset);",
                "std::int16_t masked_data = (*captured_data & 0xff0) >> 4;",
                "std::int8_t typed_data = static_cast<std::int8_t>(masked_data);",
                "return typed_data;"}, body);
        }

        [Test()]
        public void Int16()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(16), 22, "SomeOtherField");
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            // Return by value
            Assert.AreEqual("std::int16_t TEST() const", declaration);
            // The encapsulating type should be a 32 bit integer which has a byte offset of 2 (the first 16 bits of the offset).
            // We should mask out the least significant 6 bits and the most significant 10 bits and shift six places right. The resulting value needs to 
            // be cast back to the expected type: int16    
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = SomeOtherField().end() + 2;",
                "const std::int32_t* captured_data = reinterpret_cast<const std::int32_t*>(data_offset);",
                "std::int32_t masked_data = (*captured_data & 0x3fffc0) >> 6;",
                "std::int16_t typed_data = static_cast<std::int16_t>(masked_data);",
                "return typed_data;"}, body);
            //Assert.AreEqual("return static_cast<std::int16_t>(((*reinterpret_cast<const std::int32_t*>(SomeOtherField().end() + 2) & 0x3fffc0) >> 6));", body);
        }

        [Test()]
        public void Int32()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.SignedIntegral(32), 523, "SomeOtherField");
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            // Return by value
            Assert.AreEqual("std::int32_t TEST() const", declaration);
            // The encapsulating type should be a 64 bit integer which has a byte offset of 65 (the first 520 bits of the offset).
            // We should mask out the least significant 3 bits and the most significant 5 bits and shift 3 places right. The resulting value needs to 
            // be cast back to the expected type: int32
            ;
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = SomeOtherField().end() + 65;",
                "const std::int64_t* captured_data = reinterpret_cast<const std::int64_t*>(data_offset);",
                "std::int64_t masked_data = (*captured_data & 0x7fffffff8) >> 3;",
                "std::int32_t typed_data = static_cast<std::int32_t>(masked_data);",
                "return typed_data;"}, body);
            //Assert.AreEqual("return static_cast<std::int32_t>(((*reinterpret_cast<const std::int64_t*>(SomeOtherField().end() + 65) & 0x7fffffff8) >> 3));", body);
        }

        [Test()]
        public void Uint8()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(8), 4, "SomeOtherField");
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            // Return by value
            Assert.AreEqual("std::uint8_t TEST() const", declaration);
            // The encapsulating type should be a 16 bit integer. We should then mask out the most and least significant nibble and shift four places right. The resulting value needs to 
            // be cast back to the expected type: int8
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = SomeOtherField().end();",
                "const std::uint16_t* captured_data = reinterpret_cast<const std::uint16_t*>(data_offset);",
                "std::uint16_t masked_data = (*captured_data & 0xff0) >> 4;",
                "std::uint8_t typed_data = static_cast<std::uint8_t>(masked_data);",
                "return typed_data;"}, body);
            //Assert.AreEqual("return static_cast<std::uint8_t>(((*reinterpret_cast<const std::uint16_t*>(SomeOtherField().end()) & 0xff0) >> 4));", body);
        }

        [Test()]
        public void Uint16()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(16), 22, "SomeOtherField");
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            // Return by value
            Assert.AreEqual("std::uint16_t TEST() const", declaration);
            // The encapsulating type should be a 32 bit integer which has a byte offset of 2 (the first 16 bits of the offset).
            // We should mask out the least significant 6 bits and the most significant 10 bits and shift six places right. The resulting value needs to 
            // be cast back to the expected type: int16     
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = SomeOtherField().end() + 2;",
                "const std::uint32_t* captured_data = reinterpret_cast<const std::uint32_t*>(data_offset);",
                "std::uint32_t masked_data = (*captured_data & 0x3fffc0) >> 6;",
                "std::uint16_t typed_data = static_cast<std::uint16_t>(masked_data);",
                "return typed_data;"}, body);
            //Assert.AreEqual("return static_cast<std::uint16_t>(((*reinterpret_cast<const std::uint32_t*>(SomeOtherField().end() + 2) & 0x3fffc0) >> 6));", body);
        }

        [Test()]
        public void Uint32()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.UnsignedIntegral(32), 523, "SomeOtherField");
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            // Return by value
            Assert.AreEqual("std::uint32_t TEST() const", declaration);
            // The encapsulating type should be a 64 bit integer which has a byte offset of 65 (the first 520 bits of the offset).
            // We should mask out the least significant 3 bits and the most significant 5 bits and shift 3 places right. The resulting value needs to 
            // be cast back to the expected type: int32
            ;
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = SomeOtherField().end() + 65;",
                "const std::uint64_t* captured_data = reinterpret_cast<const std::uint64_t*>(data_offset);",
                "std::uint64_t masked_data = (*captured_data & 0x7fffffff8) >> 3;",
                "std::uint32_t typed_data = static_cast<std::uint32_t>(masked_data);",
                "return typed_data;"}, body);
            //Assert.AreEqual("return static_cast<std::uint32_t>(((*reinterpret_cast<const std::uint64_t*>(SomeOtherField().end() + 65) & 0x7fffffff8) >> 3));", body);
        }
    }

    [TestFixture()]
    public class Enumeration
    {
        [Test()]
        public void SignedStandardNoOffset()
        {
            var field = new Model.Field("TEST", new Model.Enumeration("Enum", new Model.FieldTypes.SignedIntegral(8)), 0, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("const Enum& TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this);",
                "const Enum* captured_data = reinterpret_cast<const Enum*>(data_offset);",
                "return *captured_data;"}, body);
        }

        [Test()]
        public void SignedStandardMultiByteOffset()
        {
            var field = new Model.Field("TEST", new Model.Enumeration("Enum", new Model.FieldTypes.SignedIntegral(8)), 16, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("const Enum& TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this) + 2;",
                "const Enum* captured_data = reinterpret_cast<const Enum*>(data_offset);",
                "return *captured_data;"}, body);
        }

        [Test()]
        public void SignedStandardNonByteAlignedOffset()
        {
            var field = new Model.Field("TEST", new Model.Enumeration("Enum", new Model.FieldTypes.SignedIntegral(32)), 523, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("Enum TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this) + 65;",
                "const std::int64_t* captured_data = reinterpret_cast<const std::int64_t*>(data_offset);",
                "std::int64_t masked_data = (*captured_data & 0x7fffffff8) >> 3;",
                "Enum typed_data = static_cast<Enum>(masked_data);",
                "return typed_data;"}, body);
        }

        [Test()]
        public void NonStandardWidth()
        {
            var field = new Model.Field("TEST", new Model.Enumeration("Enum", new Model.FieldTypes.SignedIntegral(17)), 0, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("Enum TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this);",
                "const std::int32_t* captured_data = reinterpret_cast<const std::int32_t*>(data_offset);",
                "bool sign_bit = (*captured_data & 0x10000) == 0x10000;",                
                "std::int32_t masked_data = (*captured_data & 0xffff);",
                "std::int32_t signed_data = masked_data | (sign_bit ? 0xffff0000 : 0);",
                "Enum typed_data = static_cast<Enum>(signed_data);",
                "return typed_data;"}, body);
        }

        // TODO: brackets around ternary operator and look into (1<<x) construct. This is an int by default which may overflow.
        [Test()]
        public void NonStandardWidthNonByteAlignedOffset()
        {
            var field = new Model.Field("TEST", new Model.Enumeration("Enum", new Model.FieldTypes.SignedIntegral(17)), 523, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            //[.........][....****][********][*****...]
            Assert.AreEqual("Enum TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this) + 65;",
                "const std::int32_t* captured_data = reinterpret_cast<const std::int32_t*>(data_offset);",
                "bool sign_bit = (*captured_data & 0x80000) == 0x80000;",
                "std::int32_t masked_data = (*captured_data & 0x7fff8) >> 3;",
                "std::int32_t signed_data = masked_data | (sign_bit ? 0xffff0000 : 0);",
                "Enum typed_data = static_cast<Enum>(signed_data);",
                "return typed_data;"}, body);
        }
    }

    [TestFixture()]
    public class FloatingPoint
    {
        [Test()]
        public void NoOffset()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.FloatingPoint(32), 0, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("const float& TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this);",
                "const float* captured_data = reinterpret_cast<const float*>(data_offset);",
                "return *captured_data;"}, body);
        }

        [Test()]
        public void StaticMultiByteOffset()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.FloatingPoint(64), 40, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("const double& TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this) + 5;",
                "const double* captured_data = reinterpret_cast<const double*>(data_offset);",
                "return *captured_data;"}, body);
        }

        [Test()]
        public void StaticNonMultiByteOffset()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.FloatingPoint(32), 43, null);
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("float TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = reinterpret_cast<const std::uint8_t*>(this) + 5;",
                "const std::uint64_t* captured_data = reinterpret_cast<const std::uint64_t*>(data_offset);",
                "std::uint64_t masked_data = (*captured_data & 0x7fffffff8) >> 3;",
                "float typed_data = static_cast<float>(masked_data);",
                "return typed_data;"}, body);
        }

        [Test()]
        public void MixedOffset()
        {
            var field = new Model.Field("TEST", new Model.FieldTypes.FloatingPoint(64), 24, "SomeOtherField");
            BiPaGe.FrontEnd.CPP.FieldGetterGenerator gen = new BiPaGe.FrontEnd.CPP.FieldGetterGenerator(field);
            var declaration = gen.GetDeclaration();
            var body = gen.GetBody();

            Assert.AreEqual("const double& TEST() const", declaration);
            Assert.AreEqual(new List<String> {
                "const std::uint8_t* data_offset = SomeOtherField().end() + 3;",
                "const double* captured_data = reinterpret_cast<const double*>(data_offset);",
                "return *captured_data;"}, body);
        }
    }
}

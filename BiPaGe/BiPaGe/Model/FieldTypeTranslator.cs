using System;
using System.Collections.Generic;
using System.Diagnostics;
using BiPaGe.Model.FieldTypes;
namespace BiPaGe.Model
{
    class FieldTypeTranslator : AST.FieldTypes.IFieldTypeVisitor
    {
        private Model.FieldType translated_type = null;
        private String FieldName;
        private Dictionary<String, DataElement> Elements;
        public FieldTypeTranslator(Dictionary<String, DataElement> elements)
        {
            this.Elements = elements;
        }
        public FieldType Translate(AST.FieldType original, String fieldName)
        {
            this.FieldName = fieldName;
            translated_type = null;
            original.Accept(this);
            Debug.Assert(translated_type != null);
            return translated_type;
        }

        public void Visit(AST.Identifiers.Identifier i)
        {
            Debug.Assert(Elements.ContainsKey(i.Id));
            translated_type = Elements[i.Id];
        }

        public void Visit(AST.FieldTypes.Boolean b)
        {
            translated_type = new FieldTypes.Boolean();
        }

        public void Visit(AST.FieldTypes.Float f)
        {
            translated_type = new FloatingPoint(f.Size);
        }

        public void Visit(AST.FieldTypes.InlineEnumeration ie)
        {
            EnumerationBuilder builder = new EnumerationBuilder(this.Elements);
            Debug.Assert(!this.Elements.ContainsKey(FieldName), "Crap, we already have a type that has the same name as the field that uses this anonimous type. Typenames should be unique");

            // TODO: For now we just use the field name directly as the type name for anonimous types. This can lead to problems. Do better
            builder.Build(ie, FieldName);
            Debug.Assert(this.Elements.ContainsKey(FieldName));
            translated_type = this.Elements[FieldName];
        }

        public void Visit(AST.FieldTypes.InlineObject io)
        {
            StructureBuilder structureBuilder = new StructureBuilder(this.Elements);
            Debug.Assert(!this.Elements.ContainsKey(FieldName), "Crap, we already have a type that has the same name as the field that uses this anonimous type. Typenames should be unique");

            // TODO: For now we just use the field name directly as the type name for anonimous types. This can lead to problems. Do better            
            structureBuilder.BuildStructure(io, FieldName);
            structureBuilder.PopulateStructure(io, FieldName);
            Debug.Assert(this.Elements.ContainsKey(FieldName));
            translated_type = this.Elements[FieldName];
        }

        public void Visit(AST.FieldTypes.Signed s)
        {
            translated_type = new SignedIntegral(s.Size);
        }

        public void Visit(AST.FieldTypes.Unsigned u)
        {
            translated_type = new UnsignedIntegral(u.Size);
        }

        public void Visit(AST.FieldTypes.AsciiString s)
        {
            translated_type = new FieldTypes.AsciiString(null);
            throw new NotImplementedException();
        }

        public void Visit(AST.FieldTypes.Utf8String s)
        {
            // TODO
            // I'm not really sure how to handle this yet
            throw new NotImplementedException();
        }
    }
}

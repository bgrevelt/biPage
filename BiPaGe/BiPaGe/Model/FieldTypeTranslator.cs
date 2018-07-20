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
        private readonly ExpressionTranslator ExpressionTranslator = new ExpressionTranslator();
        private AST.Expressions.IExpression collectionSize = null;

        public FieldTypeTranslator(Dictionary<String, DataElement> elements)
        {
            this.Elements = elements;
        }
        public FieldType Translate(AST.FieldType original, String fieldName, AST.Expressions.IExpression collectionSize)
        {
            this.FieldName = fieldName;
            translated_type = null;
            original.Accept(this);
            Debug.Assert(translated_type != null);
            return translated_type;
        }

        public Integral TranslateIntegral(AST.FieldType original)
        {
            translated_type = null;
            original.Accept(this);
            Debug.Assert(translated_type != null);
            Debug.Assert(translated_type is Integral);
            return translated_type as Integral;
        }

        public void Visit(AST.Identifiers.Identifier i)
        {
            Debug.Assert(Elements.ContainsKey(i.Id), "We have a reference to an unknown type in the AST. This should have been found in the semantic analysis!");
            translated_type = Elements[i.Id];
            CheckIfCollection();
        }

        public void Visit(AST.FieldTypes.Boolean b)
        {
            translated_type = new FieldTypes.Boolean();
            CheckIfCollection();
        }

        public void Visit(AST.FieldTypes.Float f)
        {
            translated_type = new FloatingPoint(f.Size);
            CheckIfCollection();
        }

        public void Visit(AST.FieldTypes.InlineEnumeration ie)
        {
            EnumerationBuilder builder = new EnumerationBuilder(this.Elements);
            Debug.Assert(!this.Elements.ContainsKey(FieldName), "Crap, we already have a type that has the same name as the field that uses this anonimous type. Typenames should be unique");
            
            var name = FieldName + "_enumeration";
            builder.Build(ie, name);
            Debug.Assert(this.Elements.ContainsKey(name));
            translated_type = this.Elements[name];
            CheckIfCollection();
        }

        public void Visit(AST.FieldTypes.InlineObject io)
        {
            StructureBuilder structureBuilder = new StructureBuilder(this.Elements);
            Debug.Assert(!this.Elements.ContainsKey(FieldName), "Crap, we already have a type that has the same name as the field that uses this anonimous type. Typenames should be unique");

            var name = FieldName + "_structurue";
            structureBuilder.BuildStructure(io, name);
            structureBuilder.PopulateStructure(io, name);
            Debug.Assert(this.Elements.ContainsKey(name));
            translated_type = this.Elements[name];
            CheckIfCollection();
        }

        public void Visit(AST.FieldTypes.Signed s)
        {
            translated_type = new SignedIntegral(s.Size);
            CheckIfCollection();
        }

        public void Visit(AST.FieldTypes.Unsigned u)
        {
            translated_type = new UnsignedIntegral(u.Size);
            CheckIfCollection();
        }

        public void Visit(AST.FieldTypes.AsciiString s)
        {
            translated_type = new FieldTypes.AsciiString(this.ExpressionTranslator.Translate(this.collectionSize));
        }

        public void Visit(AST.FieldTypes.Utf8String s)
        {
            // TODO
            // I'm not really sure how to handle this yet
            throw new NotImplementedException();
        }

        private void CheckIfCollection()
        {
            if (this.collectionSize == null)
                return;

            translated_type = new Collection(translated_type, this.ExpressionTranslator.Translate(this.collectionSize));
        }
    }
}

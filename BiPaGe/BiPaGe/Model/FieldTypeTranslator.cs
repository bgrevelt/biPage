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
            /*  Here's where it gets interesting. We want to do three things here
              (1) Invent a name for this anonimous enumeration
              (2) Add a new enumeration to the enumeration list 
              (3) Return a (named) reference  to this new enumeration
             */

            // (1) For now we just use the field name directly. TODO: this can lead to problems. Do better
            var name = FieldName;

            // (2)
            Debug.Assert(translated_type == null, "I don't think we need to be reentrant, so if there's ever anything here already, I overlooked something!");
            ie.Type.Accept(this);
            var enumeration = new Enumeration(name, (dynamic)translated_type); // TODO: we still have a dynamic here because we can't implicitly upcast. I'm not sure how to handle this without using typeofs. I want to restrict the enum type to only accept integer types (the language doesn't support other types of enumerations) but the translator return the base type. Should I have a translate specific for integral types?
            foreach (var enumerator in ie.Enumerators)
            {
                enumeration.AddEnumerator(new Enumerator(enumerator.Name, enumerator.Value));
            }

            Debug.Assert(Elements.ContainsKey(name), "We should construct a unique type name for the anonimous enumerations");
            Elements[name] = enumeration;

            // (3)
            translated_type = enumeration;
        }

        public void Visit(AST.FieldTypes.InlineObject io)
        {
            var fieldTranslator = new FieldTranslator(Elements);
            // Similar to the enumeration
            // (1) Invent a name for this anonimous structure
            // (2) Add a new structure to the structure list 
            // (3) Return a (named) reference  to this new structure

            // (1) For now we just use the field name directly. TODO: this can lead to problems. Do better
            var name = FieldName;
            // (2)
            var structure = new Structure(name);
            uint field_offset = 0;
            String last_dynmic_field = null;
            foreach (var field in io.Fields)
            {
                var model_field = fieldTranslator.Translate(field, field_offset, last_dynmic_field);
                structure.AddField(model_field);
                if (model_field.HasStaticSize())
                {
                    field_offset += model_field.SizeInBits();
                }
                else
                {
                    field_offset = 0;
                    last_dynmic_field = model_field.Name;
                }
            }

            Debug.Assert(!Elements.ContainsKey(name), "We should construct a unique type name for the anonimous structures");
            Elements[name] = structure;
            // (3)
            translated_type = structure;
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

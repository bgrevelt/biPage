using System;
using System.Collections.Generic;
using System.Diagnostics;
using BiPaGe.Model.FieldTypes;

namespace BiPaGe.Model
{
    public class FieldTranslator 
    {
        private FieldTypeTranslator fieldTypeTranslator;

        private Dictionary<String, DataElement> Elements;
        private readonly ExpressionTranslator ExpressionTranslator = new ExpressionTranslator();
        public FieldTranslator(Dictionary<String, DataElement> elements)
        {
            this.Elements = elements;
            this.fieldTypeTranslator = new FieldTypeTranslator(elements);
        }

        public Model.Field Translate(AST.Field original, uint field_offset, String offset_from)
        {

            var translated_type = fieldTypeTranslator.Translate(original.Type, original.Name);

            if (translated_type is AsciiString) // Todo: this is horrible!
            {
                translated_type = new AsciiString(ExpressionTranslator.Translate(original.CollectionSize));
            }
            if (original.CollectionSize != null && !(translated_type is AsciiString))
            {
                // This is a collection. We have a dedicated type for this in the model
                translated_type = new FieldTypes.Collection(translated_type, ExpressionTranslator.Translate(original.CollectionSize));
            }

            return new Field(original.Name, translated_type, field_offset, offset_from);            // TODO: static and dynamic offsets
        }
    }
}

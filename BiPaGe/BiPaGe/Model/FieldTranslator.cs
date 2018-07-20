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
        public FieldTranslator(Dictionary<String, DataElement> elements)
        {
            this.Elements = elements;
            this.fieldTypeTranslator = new FieldTypeTranslator(elements);
        }

        public Model.Field Translate(AST.Field original, uint field_offset, String offset_from)
        {

            var translated_type = fieldTypeTranslator.Translate(original.Type, original.Name, original.CollectionSize);
            return new Field(original.Name, translated_type, field_offset, offset_from);
        }
    }
}

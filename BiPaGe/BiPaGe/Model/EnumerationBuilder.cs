using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.Model
{
    class EnumerationBuilder
    {
        private readonly Dictionary<String, DataElement> Elements;
        private readonly FieldTypeTranslator fieldTypeTranslator;
        public EnumerationBuilder(Dictionary<String, DataElement> elements)
        {
            this.Elements = elements;
            this.fieldTypeTranslator = new FieldTypeTranslator(elements);
        }
        public void Build(AST.Enumeration enumeration)
        {
            Build(enumeration.Identifier, enumeration.Type, enumeration.Enumerators);
        }    
        
        public void Build(AST.FieldTypes.InlineEnumeration enumeration, String name)
        {
            Build(name, enumeration.Type, enumeration.Enumerators);
        }

        private void Build(String name, AST.FieldType type, IList<AST.Enumerator> enumerators)
        {
            var enum_type = this.fieldTypeTranslator.TranslateIntegral(type); // We don't support other types of enumerations. F.i. enum : ascii_string { option_one : "bla", option_two : "blah"};
            var translated = new Enumeration(name, enum_type as FieldTypes.Integral);

            foreach (var enumerator in enumerators)
                translated.AddEnumerator(new Enumerator(enumerator.Name, enumerator.Value));

            Debug.Assert(!Elements.ContainsKey(name), "Element names should be unique");
            Elements[translated.Name] = translated;
        }
    }
}

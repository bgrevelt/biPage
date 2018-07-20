using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiPaGe.Model
{
    class StructureBuilder
    {
        private readonly Dictionary<String, DataElement> Elements;
        private readonly FieldTranslator fieldTranslator;
        public StructureBuilder(Dictionary<String, DataElement> elements)
        {
            this.Elements = elements;
            this.fieldTranslator = new FieldTranslator(elements);
        }

        public void BuildStructure(AST.Object o)
        {
            Debug.Assert(!Elements.ContainsKey(o.Identifier), "Element names should be unique");
            Elements[o.Identifier] = new Structure(o.Identifier);
        }

        public void BuildStructure(AST.FieldTypes.InlineObject o, String name)
        {
            Debug.Assert(!Elements.ContainsKey(name), "Element names should be unique");
            Elements[name] = new Structure(name);          
        }

        public void PopulateStructure(AST.Object o)
        {
            PopulateStructure(o.Identifier, o.Fields);
        }

        public void PopulateStructure(AST.FieldTypes.InlineObject o, String name)
        {
            PopulateStructure(name, o.Fields);
        }

        private void PopulateStructure(String name, IList<AST.Field> fields)
        {
            Debug.Assert(Elements.ContainsKey(name));
            Debug.Assert(Elements[name] is Structure);

            var structure = Elements[name] as Structure;

            uint field_offset = 0;
            String last_dynmic_field = null;
            foreach (var field in fields)
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
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
namespace BiPaGe.Model
{
    public class Builder
    {
        public List<Structure> Structures { get; }
        public List<Enumeration> Enumerations { get; }
        private readonly Dictionary<String, DataElement> elements = new Dictionary<string, DataElement>();
        private readonly FieldTranslator fieldTranslator;
        private readonly FieldTypeTranslator fieldTypeTranslator;

        public Builder()
        {
            this.Structures = new List<Structure>();
            this.Enumerations = new List<Enumeration>();
            this.fieldTranslator = new FieldTranslator(elements);
            this.fieldTypeTranslator = new FieldTypeTranslator(elements);
        }
        public void Build(AST.Parser AST)
        {
            FirstPass(AST);

            foreach (var element in AST.Elements)
                VisitElement((dynamic)element);

            // TODO: temporary hack to support old interface
            foreach(var element in elements.Values)
            {
                if (element is Structure)
                    Structures.Add((dynamic)element);
                else
                    Enumerations.Add((dynamic)element);
            }
        }

        private void FirstPass(AST.Parser AST)
        {
            // So the idea is that we build up a list of structures and enumerations here based on their name. So we are only bothered with 'root level' structures and enumerations
            foreach (var element in AST.Elements)
            {
                Debug.Assert(element is AST.Enumeration || element is AST.Object);
                if (element is AST.Enumeration)
                {
                    // TODO: I'm not really happy with this since this is basically a copy of how we build anonimous enumerations
                    var e = element as AST.Enumeration;
                    elements[e.Identifier] = new Enumeration(e.Identifier, (dynamic)this.fieldTypeTranslator.Translate(e.Type, e.Identifier));

                }
                else
                {
                    var o = element as AST.Object;
                    elements[o.Identifier] = new Structure(o.Identifier);
                }
            }

        }

        private void VisitElement(AST.Object o)
        {
            Debug.Assert(elements.ContainsKey(o.Identifier));
            Debug.Assert(elements[o.Identifier] is Structure);
            
            // TODO: don't really like the 'as' 
            var s = elements[o.Identifier] as Structure;

            // TODO: the code to build structures and enumeraitons is now duplicated here and in the fieldtype builder (for the anoniumous variant). We should extract the overlapping code.
            uint field_offset = 0;
            String last_dynmic_field = null;
            foreach (var field in o.Fields)
            {
                var model_field = fieldTranslator.Translate(field, field_offset, last_dynmic_field);
                s.AddField(model_field);
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

        private void VisitElement(AST.Enumeration e)
        {
            Debug.Assert(elements.ContainsKey(e.Identifier));
            Debug.Assert(elements[e.Identifier] is Enumeration);

            var enumeration = elements[e.Identifier] as Enumeration;
            
            foreach (var enumerator in e.Enumerators)            
                enumeration.AddEnumerator(new Enumerator(enumerator.Name, enumerator.Value));                
        }
    }
}

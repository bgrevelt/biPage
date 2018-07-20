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
        private readonly EnumerationBuilder enumBuilder;
        private readonly StructureBuilder structureBuilder;
        private readonly FieldTypeTranslator fieldTypeTranslator;

        public Builder()
        {
            this.Structures = new List<Structure>();
            this.Enumerations = new List<Enumeration>();
            this.enumBuilder = new EnumerationBuilder(elements);
            structureBuilder = new StructureBuilder(elements);
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
                    enumBuilder.Build(element as AST.Enumeration);
                }
                else
                {
                    structureBuilder.BuildStructure(element as AST.Object);                    
                }
            }

        }

        private void VisitElement(AST.Object o)
        {
            structureBuilder.PopulateStructure(o);
        }

        private void VisitElement(AST.Enumeration e)
        {
            // No need to do anything here, the enumeration is already filled in the first pass.
        }
    }
}

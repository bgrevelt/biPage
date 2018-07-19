using System;
using System.Collections.Generic;
using System.Diagnostics;
namespace BiPaGe.Model
{
    //public class Scope
    //{
    //    public Scope()
    //    {

    //    }
    //    private Scope(Scope other)
    //    {
    //        this.scope = new Stack<string>(other.scope.Reverse());
    //    }
    //    private Stack<String> scope = new Stack<string>();
    //    public void StepInto(String name)
    //    {
    //        scope.Push(name);
    //    }

    //    public void StepOut()
    //    {
    //        scope.Pop();
    //    }

    //    public Scope Clone()
    //    {
    //        return new Scope(this);
    //    }
    //}

    public class Scope
    {
        public Scope(Structure s)
        {
            this.CurrentStructure = s;
            this.CurrentFieldOffset = 0;
        }
        public Structure CurrentStructure { get; }
        public String CurrentFieldName { get; set; }
        public Field LastDynamicField { get; set; }
        public Enumeration CurrentEnumeration { get; set; }
        public uint CurrentFieldOffset { get; set; }
    }

    public class Builder
    {
        public List<Structure> Structures { get; }
        public List<Enumeration> Enumerations { get; }

        private Stack<Scope> scope = new Stack<Scope>(); 
        //private Stack<Structure> structure_stack = new Stack<Structure>();
        //private Stack<Enumeration> enum_stack = new Stack<Enumeration>();
        //private Stack<String> field_name_stack = new Stack<string>();

        private Dictionary<String, DataElement> elements = new Dictionary<string, DataElement>();
        private readonly FieldTranslator fieldTranslator;
        private readonly FieldTypeTranslator fieldTypeTranslator;

        //private Field last_dynamic_field = null;

        // TODO: eventually we want to make structures and enumerations private and wrap them in a Model class that also includes the parse rules.
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

        private void Visit(AST.Constants.LiteralCollection lc)
        {

        }
        private void Visit(AST.Constants.ObjectConstant oc)
        {
        }
        void Visit(AST.Constants.ObjectField of)
        {
        }

        private void Visit(AST.Literals.Boolean b)
        {
        }
        private void Visit(AST.Literals.Float f)
        {
        }
        private void Visit(AST.Literals.Integer i)
        {
        }
        private void Visit(AST.Literals.StringLiteral s)
        {
        }



        // VISITIING OF ELEMENTS

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

        //private void VisitField(AST.Field f)
        //{
        //    scope.Peek().CurrentFieldName = f.Name;             
        //    FieldType type = VisitFieldType((dynamic)f.Type, f.CollectionSize);

        //    var field = new Field(f.Name, type, scope.Peek().CurrentFieldOffset, scope.Peek().LastDynamicField?.Name);

        //    if (field.HasStaticSize())
        //    {
        //        scope.Peek().CurrentFieldOffset += field.SizeInBits();
        //    }
        //    scope.Peek().CurrentStructure.AddField(field);


        //    if(!field.HasStaticSize())            
        //        scope.Peek().LastDynamicField = field;
        //}

        //// Visiting of field types
        
        //// Helper function used by the field type visitors to determine if the field type is a colleciton. We need this because we treat colllection different in the model than in the AST
        //// in the AST each field has a type and an optional collection size. If the collection size is there, the type is a collection. In the model we want to use a dedicated type for a collection
        //private FieldType CheckIfCOllection(FieldType fieldtype, AST.Expressions.IExpression collection_size)
        //{
        //    if (collection_size == null)
        //        return fieldtype;

        //    return new FieldTypes.Collection(fieldtype, VisitExpression((dynamic)collection_size));
        //}



        //private FieldType VisitFieldType(AST.Identifiers.Identifier i, AST.Expressions.IExpression collection_size)
        //{
        //    Debug.Assert(elements.ContainsKey(i.Id));            

        //    return CheckIfCOllection(elements[i.Id], collection_size);
        //}
        
        //private FieldType VisitFieldType(AST.FieldTypes.Boolean b, AST.Expressions.IExpression collection_size)
        //{
        //    return CheckIfCOllection(new FieldTypes.Boolean(), collection_size);
        //}
        //private FieldType VisitFieldType(AST.FieldTypes.Float f, AST.Expressions.IExpression collection_size)
        //{
        //    return CheckIfCOllection(new FieldTypes.FloatingPoint(f.Size), collection_size);
        //}
        //private FieldType VisitFieldType(AST.FieldTypes.InlineEnumeration ie, AST.Expressions.IExpression collection_size)
        //{
        //    // Here's where it gets interesting. We want to do two things here
        //    // (1) Invent a name for this anonimous enumeration
        //    // (2) Add a new enumeration to the enumeration list 
        //    // (3) Return a (named) reference  to this new enumeration

        //    // (1) For now we just use the field name directly. TODO: this can lead to problems. Do better
        //    var name = scope.Peek().CurrentFieldName;
        //    // (2)
        //    scope.Peek().CurrentEnumeration = new Enumeration(name, VisitFieldType((dynamic)ie.Type, null));
        //    foreach(var enumerator in ie.Enumerators)
        //    {
        //        VisitEnumerator(enumerator);
        //    }
        //    // (3)
        //    return CheckIfCOllection(scope.Peek().CurrentEnumeration, collection_size);
        //}
        //private FieldType VisitFieldType(AST.FieldTypes.InlineObject io, AST.Expressions.IExpression collection_size)
        //{
        //    // Similar to the enumeration
        //    // (1) Invent a name for this anonimous structure
        //    // (2) Add a new structure to the structure list 
        //    // (3) Return a (named) reference  to this new structure

        //    // (1) For now we just use the field name directly. TODO: this can lead to problems. Do better
        //    var name = scope.Peek().CurrentFieldName;
        //    // (2)
        //    scope.Push(new Scope(new Structure(name)));
        //    foreach (var field in io.Fields)
        //    {
        //        VisitField(field);
        //    }
        //    var s = scope.Pop();
        //    Structures.Add(s.CurrentStructure);
        //    // (3)
        //    return CheckIfCOllection(s.CurrentStructure, collection_size);
        //}

        //private FieldType VisitFieldType(AST.FieldTypes.Signed s, AST.Expressions.IExpression collection_size)
        //{
        //    return CheckIfCOllection(new FieldTypes.SignedIntegral(s.Size), collection_size);
        //}
        //private FieldType VisitFieldType(AST.FieldTypes.Unsigned u, AST.Expressions.IExpression collection_size)
        //{
        //    return CheckIfCOllection(new FieldTypes.UnsignedIntegral(u.Size), collection_size);
        //}

        //private FieldType VisitFieldType(AST.FieldTypes.AsciiString s, AST.Expressions.IExpression collection_size)
        //{
        //    return new FieldTypes.AsciiString(VisitExpression((dynamic)collection_size));
        //}
        //private FieldType VisitFieldType(AST.FieldTypes.Utf8String s, AST.Expressions.IExpression collection_size)
        //{
        //    // I'm not really sure how to handle this yet
        //    throw new NotImplementedException();
        //}

        //// Visit Enumerator
        //private void VisitEnumerator(AST.Enumerator e)
        //{
        //    // The parse can throw, but we should have checked for that in SA
        //    // TODO: we need to determine if we want to do the parsing in the AST object or here and be consistent about it. For example AST.Literals.Integer parses on its own.
        //    scope.Peek().CurrentEnumeration.AddEnumerator(new Model.Enumerator(e.Name, e.Value));
        //}

        //// Visit expressions
        //private Expressions.Expression VisitExpression(AST.Expressions.Addition a)
        //{
        //    return new Expressions.Addition(VisitExpression((dynamic)a.Left), VisitExpression((dynamic)a.Right));
        //}
        //private Expressions.Expression VisitExpression(AST.Expressions.Subtraction s)
        //{
        //    return new Expressions.Subtraction(VisitExpression((dynamic)s.Left), VisitExpression((dynamic)s.Right));
        //}
        //private Expressions.Expression VisitExpression(AST.Expressions.Multiplication m)
        //{
        //    return new Expressions.Multiplication(VisitExpression((dynamic)m.Left), VisitExpression((dynamic)m.Right));
        //}
        //private Expressions.Expression VisitExpression(AST.Expressions.Division d)
        //{
        //    return new Expressions.Division(VisitExpression((dynamic)d.Left), VisitExpression((dynamic)d.Right));
        //}
        //private Expressions.Expression VisitExpression(AST.Expressions.This t)
        //{
        //    return new Expressions.This();
        //}

        //private Expressions.Expression VisitExpression(AST.Identifiers.FieldIdentifier f)
        //{
        //    return new Expressions.FieldIdentifier(f.Id);
        //}

        //private Expressions.Expression VisitExpression(AST.Literals.Integer i)
        //{
        //    return new Expressions.Number(i.Value);
        //}
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    public class Builder
    {

        private class Scope
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

        public List<Structure> Structures { get; }
        public List<Enumeration> Enumerations { get; }

        private Stack<Scope> scope = new Stack<Scope>(); 
        //private Stack<Structure> structure_stack = new Stack<Structure>();
        //private Stack<Enumeration> enum_stack = new Stack<Enumeration>();
        //private Stack<String> field_name_stack = new Stack<string>();

        private Dictionary<String, DataElement> elements = new Dictionary<string, DataElement>();

        //private Field last_dynamic_field = null;

        // TODO: eventually we want to make structures and enumerations private and wrap them in a Model class that also includes the parse rules.
        public Builder()
        {
            this.Structures = new List<Structure>();
            this.Enumerations = new List<Enumeration>();
        }
        public void Build(AST.Parser AST)
        {
            // TODO: we need to do this differently. Right now we iterate over the elements in the order in which they are defined. Because we support out of order definition (why do we support out of order definition any way?)
            // We can get regerences to types that are defined 'lower' in the tree. This is a problem when we want to determine the offsets for fields. For each field we want to store the offset to the previous dynamic field.
            // but if these fields are of a type defined somwhere else (e.g. a reference) we may not be able to determine if that field is dynamic or not. 

            /*
             * 1) create a dict<name, elementType>
             * 2) create the model. Instead of references (as a type) we create an actual reference to the elementType (e.g. structure or enumeration)
             * 3) Determine size information for all types.
            */

            // Create a 'root' scope (TODO: hmmm, that's a bit ugly...)
            scope.Push(new Scope(null));


            FirstPass(AST);

            foreach (var element in AST.Elements)
                VisitElement((dynamic)element);
        }

        private void FirstPass(AST.Parser AST)
        {
            // So the idea is that we build up a list of structures and enumerations here based on their name. So we are only bothered with 'root level' structures and enumerations
            foreach (var element in AST.Elements)
            {
                Debug.Assert(element is AST.Enumeration || element is AST.Object);
                if (element is AST.Enumeration)
                {
                    var e = element as AST.Enumeration;
                    elements[e.Identifier] = new Enumeration(e.Identifier, VisitFieldType((dynamic)e.Type, null));

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

            // TODO: this dynamic cast is ugly. See if we can do better (but we still need a dict to the common base class because we don't know the type for references)
            scope.Push(new Scope((dynamic)elements[o.Identifier]));
            foreach (var field in o.Fields)
            {
                VisitField(field);
            }
            Structures.Add(scope.Pop().CurrentStructure);
        }

        private void VisitElement(AST.Enumeration e)
        {
            Debug.Assert(elements.ContainsKey(e.Identifier));
            // TODO: this dynamic cast is ugly. See if we can do better (but we still need a dict to the common base class because we don't know the type for references)
            scope.Peek().CurrentEnumeration = (dynamic)elements[e.Identifier];
            
            foreach (var enumerator in e.Enumerators)
            {
                VisitEnumerator(enumerator);
            }
            Enumerations.Add(scope.Peek().CurrentEnumeration);
        }

        private void VisitField(AST.Field f)
        {
            scope.Peek().CurrentFieldName = f.Name;             
            FieldType type = VisitFieldType((dynamic)f.Type, f.CollectionSize);

            // TODO: we're only putting in the 'from' field right now. Still need to do the static offsets
            var field = new Field(f.Name, type, scope.Peek().CurrentFieldOffset, scope.Peek().LastDynamicField?.Name);

            if (field.HasStaticSize())
            {
                scope.Peek().CurrentFieldOffset += field.SizeInBits();
            }
            scope.Peek().CurrentStructure.AddField(field);


            if(!field.HasStaticSize())            
                scope.Peek().LastDynamicField = field;
        }

        // Visiting of field types
        
        // Helper function used by the field type visitors to determine if the field type is a colleciton. We need this because we treat colllection different in the model than in the AST
        // in the AST each field has a type and an optional collection size. If the collection size is there, the type is a collection. In the model we want to use a dedicated type for a collection
        private FieldType CheckIfCOllection(FieldType fieldtype, AST.Expressions.IExpression collection_size)
        {
            if (collection_size == null)
                return fieldtype;

            return new FieldTypes.Collection(fieldtype, VisitExpression((dynamic)collection_size));
        }



        private FieldType VisitFieldType(AST.Identifiers.Identifier i, AST.Expressions.IExpression collection_size)
        {
            Debug.Assert(elements.ContainsKey(i.Id));            

            return CheckIfCOllection(elements[i.Id], collection_size);
        }
        
        private FieldType VisitFieldType(AST.FieldTypes.Boolean b, AST.Expressions.IExpression collection_size)
        {
            return CheckIfCOllection(new FieldTypes.Boolean(), collection_size);
        }
        private FieldType VisitFieldType(AST.FieldTypes.Float f, AST.Expressions.IExpression collection_size)
        {
            return CheckIfCOllection(new FieldTypes.FloatingPoint(f.Size), collection_size);
        }
        private FieldType VisitFieldType(AST.FieldTypes.InlineEnumeration ie, AST.Expressions.IExpression collection_size)
        {
            // Here's where it gets interesting. We want to do two things here
            // (1) Invent a name for this anonimous enumeration
            // (2) Add a new enumeration to the enumeration list 
            // (3) Return a (named) reference  to this new enumeration

            // (1) For now we just use the field name directly. TODO: this can lead to problems. Do better
            var name = scope.Peek().CurrentFieldName;
            // (2)
            scope.Peek().CurrentEnumeration = new Enumeration(name, VisitFieldType((dynamic)ie.Type, null));
            foreach(var enumerator in ie.Enumerators)
            {
                VisitEnumerator(enumerator);
            }
            // (3)
            return CheckIfCOllection(scope.Peek().CurrentEnumeration, collection_size);
        }
        private FieldType VisitFieldType(AST.FieldTypes.InlineObject io, AST.Expressions.IExpression collection_size)
        {
            // Similar to the enumeration
            // (1) Invent a name for this anonimous structure
            // (2) Add a new structure to the structure list 
            // (3) Return a (named) reference  to this new structure

            // (1) For now we just use the field name directly. TODO: this can lead to problems. Do better
            var name = scope.Peek().CurrentFieldName;
            // (2)
            scope.Push(new Scope(new Structure(name)));
            foreach (var field in io.Fields)
            {
                VisitField(field);
            }
            var s = scope.Pop();
            Structures.Add(s.CurrentStructure);
            // (3)
            return CheckIfCOllection(s.CurrentStructure, collection_size);
        }

        private FieldType VisitFieldType(AST.FieldTypes.Signed s, AST.Expressions.IExpression collection_size)
        {
            return CheckIfCOllection(new FieldTypes.SignedIntegral(s.Size), collection_size);
        }
        private FieldType VisitFieldType(AST.FieldTypes.Unsigned u, AST.Expressions.IExpression collection_size)
        {
            return CheckIfCOllection(new FieldTypes.UnsignedIntegral(u.Size), collection_size);
        }

        private FieldType VisitFieldType(AST.FieldTypes.AsciiString s, AST.Expressions.IExpression collection_size)
        {
            return new FieldTypes.AsciiString(VisitExpression((dynamic)collection_size));
        }
        private FieldType VisitFieldType(AST.FieldTypes.Utf8String s, AST.Expressions.IExpression collection_size)
        {
            // I'm not really sure how to handle this yet
            throw new NotImplementedException();
        }

        // Visit Enumerator
        private void VisitEnumerator(AST.Enumerator e)
        {
            // The parse can throw, but we should have checked for that in SA
            // TODO: we need to determine if we want to do the parsing in the AST object or here and be consistent about it. For example AST.Literals.Integer parses on its own.
            scope.Peek().CurrentEnumeration.AddEnumerator(new Model.Enumerator(e.Name, e.Value));
        }

        // Visit expressions
        private Expressions.Expression VisitExpression(AST.Expressions.Addition a)
        {
            return new Expressions.Addition(VisitExpression((dynamic)a.Left), VisitExpression((dynamic)a.Right));
        }
        private Expressions.Expression VisitExpression(AST.Expressions.Subtraction s)
        {
            return new Expressions.Subtraction(VisitExpression((dynamic)s.Left), VisitExpression((dynamic)s.Right));
        }
        private Expressions.Expression VisitExpression(AST.Expressions.Multiplication m)
        {
            return new Expressions.Multiplication(VisitExpression((dynamic)m.Left), VisitExpression((dynamic)m.Right));
        }
        private Expressions.Expression VisitExpression(AST.Expressions.Division d)
        {
            return new Expressions.Division(VisitExpression((dynamic)d.Left), VisitExpression((dynamic)d.Right));
        }
        private Expressions.Expression VisitExpression(AST.Expressions.This t)
        {
            return new Expressions.This();
        }

        private Expressions.Expression VisitExpression(AST.Identifiers.FieldIdentifier f)
        {
            return new Expressions.FieldIdentifier(f.Id);
        }

        private Expressions.Expression VisitExpression(AST.Literals.Integer i)
        {
            return new Expressions.Number(i.Value);
        }
    }
}

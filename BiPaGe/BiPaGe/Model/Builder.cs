using System;
using System.Collections.Generic;
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

        public List<Structure> Structures { get; }
        public List<Enumeration> Enumerations { get; }

        private Stack<Structure> structure_stack = new Stack<Structure>();
        private Stack<Enumeration> enum_stack = new Stack<Enumeration>();
        private Stack<String> field_name_stack = new Stack<string>();

        private String last_dynamic_field = null;

        // TODO: eventually we want to make structures and enumerations private and wrap them in a Model class that also includes the parse rules.
        public Builder()
        {
            this.Structures = new List<Structure>();
            this.Enumerations = new List<Enumeration>();
        }
        public void Build(AST.Parser AST)
        {
            // TODO: we need to do this differently. Right now we iterate over the elements in the order in which they are defined. Because we support out of order definition (why do we support out of order definition any way?)
            // We can get regerences to types that are defined 'lower' in the tree. 
            // To do this the 'right' way, we could create a tree of DataElements, where each element branches to referenced elements. That way we can start iteration at the leaves and move up. If we do that, we should not

            foreach (var element in AST.Elements)
                VisitElement((dynamic)element);
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
            structure_stack.Push(new Structure(o.Identifier));
            foreach (var field in o.Fields)
            {
                VisitField(field);
            }
            Structures.Add(structure_stack.Pop());
        }

        private void VisitElement(AST.Enumeration e)
        {
            enum_stack.Push(new Enumeration(e.Identifier, VisitFieldType((dynamic)e.Type, null)));
            foreach (var enumerator in e.Enumerators)
            {
                VisitEnumerator(enumerator);
            }
            Enumerations.Add(enum_stack.Pop());
        }

        private void VisitField(AST.Field f)
        {
            field_name_stack.Push(f.Name);
            FieldType type = VisitFieldType((dynamic)f.Type, f.CollectionSize);           

            structure_stack.Peek().AddField(new Field(f.Name, type, 0, last_dynamic_field));

            if(/* this is a dynamic field */)
            {
                last_dynamic_field = f.Name;
            }

            field_name_stack.Pop();
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
            return CheckIfCOllection(new FieldTypes.Reference(i.Id), collection_size);
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
            var name = field_name_stack.Peek();
            // (2)
            enum_stack.Push(new Enumeration(name, VisitFieldType((dynamic)ie.Type, null)));
            foreach(var enumerator in ie.Enumerators)
            {
                VisitEnumerator(enumerator);
            }
            Enumerations.Add(enum_stack.Pop());
            // (3)
            return CheckIfCOllection(new FieldTypes.Reference(name), collection_size);
        }
        private FieldType VisitFieldType(AST.FieldTypes.InlineObject io, AST.Expressions.IExpression collection_size)
        {
            // Similar to the enumeration
            // (1) Invent a name for this anonimous structure
            // (2) Add a new structure to the structure list 
            // (3) Return a (named) reference  to this new structure

            // (1) For now we just use the field name directly. TODO: this can lead to problems. Do better
            var name = field_name_stack.Peek();
            // (2)
            structure_stack.Push(new Structure(name));            
            foreach (var field in io.Fields)
            {
                VisitField(field);
            }
            Structures.Add(structure_stack.Pop());
            // (3)
            return CheckIfCOllection(new FieldTypes.Reference(name), collection_size);
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
            enum_stack.Peek().AddEnumerator(new Model.Enumerator(e.Name, e.Value));
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

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

        public List<Structure> structures { get; }
        public List<Enumeration> enumerations { get; }

        private Stack<Structure> structure_stack = new Stack<Structure>();
        private Stack<Enumeration> enum_stack = new Stack<Enumeration>();
        private Stack<String> field_name_stack = new Stack<string>();

        // TODO: eventually we want to make structures and enumerations private and wrap them in a Model class that also includes the parse rules.
        public Builder()
        {
            structures = new List<Structure>();
            enumerations = new List<Enumeration>();
        }
        public void Build(AST.Parser AST)
        {
            foreach (var element in AST.Elements)
                Visit((dynamic)element);
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

        private void Visit(AST.Expressions.Addition a)
        {
        }
        private void Visit(AST.Expressions.Subtraction s)
        {
        }
        private void Visit(AST.Expressions.Multiplication m)
        {
        }
        private void Visit(AST.Expressions.Division d)
        {
        }
        private void Visit(AST.Expressions.This t)
        {
        }

       

        private void Visit(AST.Identifiers.FieldIdentifier f)
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

        private void Visit(AST.Object o)
        {
            structure_stack.Push(new Structure(o.identifier));
            foreach (var field in o.fields)
            {
                Visit(field);
            }
            structures.Add(structure_stack.Pop());
        }

        private void Visit(AST.Enumeration e)
        {
            enum_stack.Push(new Enumeration(e.Identifier, Visit((dynamic)e.Type)));
            foreach (var enumerator in e.Enumerators)
            {
                Visit((dynamic)enumerator);
            }
            enumerations.Add(enum_stack.Pop());
        }

        private void Visit(AST.Field f)
        {
            field_name_stack.Push(f.Name);
            FieldType type = Visit((dynamic)f.Type);
            structure_stack.Peek().AddField(new Field(f.Name, type));
            field_name_stack.Pop();
        }

        // Visiting of field types
        private FieldType Visit(AST.Identifiers.Identifier i)
        {
            return new FieldTypes.Reference(i.Id);        }
        
        private FieldType Visit(AST.FieldTypes.Boolean b)
        {
            return new FieldTypes.Boolean();
        }
        private FieldType Visit(AST.FieldTypes.Float f)
        {
            return new FieldTypes.FloatingPoint(f.Size);
        }
        private FieldType Visit(AST.FieldTypes.InlineEnumeration ie)
        {
            // Here's where it gets interesting. We want to do two things here
            // (1) Invent a name for this anonimous enumeration
            // (2) Add a new enumeration to the enumeration list 
            // (3) Return a (named) reference  to this new enumeration

            // (1) For now we just use the field name directly. TODO: this can lead to problems. Do better
            var name = field_name_stack.Peek();
            // (2)
            enum_stack.Push(new Enumeration(name, Visit((dynamic)ie.Type)));
            foreach(var enumerator in ie.Enumerators)
            {
                Visit((dynamic)enumerator);
            }
            enumerations.Add(enum_stack.Pop());
            // (3)
            return new FieldTypes.Reference(name);
        }
        private FieldType Visit(AST.FieldTypes.InlineObject io)
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
                Visit((dynamic)field);
            }
            structures.Add(structure_stack.Pop());
            // (3)
            return new FieldTypes.Reference(name);
        }
        private FieldType Visit(AST.FieldTypes.Signed s)
        {
            return new FieldTypes.Integral(true, s.Size);
        }
        private FieldType Visit(AST.FieldTypes.Unsigned u)
        {
            // TODO: we should really do the 'signedness' with inheritance rather than a boolean...
            return new FieldTypes.Integral(false, u.Size);
        }

        private FieldType Visit(AST.FieldTypes.AsciiString s)
        {
            // I'm not really sure how to handle this yet
            throw new NotImplementedException();
        }
        private FieldType Visit(AST.FieldTypes.Utf8String s)
        {
            // I'm not really sure how to handle this yet
            throw new NotImplementedException();
        }

        // Visit Enumerator
        private void Visit(AST.Enumerator e)
        {
            // The parse can throw, but we should have checked for that in SA
            enum_stack.Peek().AddEnumerator(new Model.Enumerator(e.Name, int.Parse(e.Value)));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BiPaGe
{
    public class Scope
    {
        public Scope()
        {

        }
        private Scope(Scope other)
        {
            this.scope = new Stack<string>(other.scope.Reverse());
        }
        private Stack<String> scope = new Stack<string>();
        public void StepInto(String name)
        {
            scope.Push(name);
        }

        public void StepOut()
        {
            scope.Pop();
        }

        public Scope Clone()
        {
            return new Scope(this);
        }
    }

    public class ModelBuilder
    {
        private Model.Model model = new Model.Model();
        private String current_field_name;
        private Scope current_scope = new Scope();
        private Stack<Model.Object> current_object = new Stack<Model.Object>();
        private uint current_offset_in_object = 0;
        public ModelBuilder()
        {

        }
        
        public Model.Model Build(AST.Parser AST)
        {
            model = new Model.Model();

            foreach (var element in AST.Elements)
                Visit((dynamic)element);

            return model;
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

        private void Visit(AST.FieldTypes.AsciiString s)
        {
        }
        private void Visit(AST.FieldTypes.Boolean b)
        {
        }
        private void Visit(AST.FieldTypes.Float f)
        {
        }
        private void Visit(AST.FieldTypes.InlineEnumeration ie)
        {
            Model.Enumeration enumeration = new Model.Enumeration(current_field_name, current_scope);
            foreach (var enumerator in ie.Enumerators)
            {
                // The int.parse can throw, but we check this in SA so if we get something here that's invalid, just let it crash & burn
                enumeration.AddEnumerator(new Model.Enumerator(enumerator.Name, int.Parse(enumerator.Value)));
            }
            this.model.enumerations.Add(enumeration);
        }
        private void Visit(AST.FieldTypes.InlineObject io)
        {
            // Inline objects are not named. Only the fields for which they are declared are named, so use that name 
            // (for now. We probably have to change the grammar to get to a good solution. See github wiki)
            current_scope.StepInto(current_field_name);

            current_offset_in_object = 0;
            current_object.Push(new Model.Object(current_field_name));
            foreach (var field in io.Fields)
                Visit((dynamic)field);
            this.model.objects.Add(current_object.Pop());


            foreach (var field in io.Fields)
                Visit((dynamic)field);
            current_scope.StepOut();
       }
       private void Visit(AST.FieldTypes.Signed s)
       {
       }
       private void Visit(AST.FieldTypes.Unsigned u)
       {
       }
       private void Visit(AST.FieldTypes.Utf8String s)
       {
       }

       private void Visit(AST.Identifiers.FieldIdentifier f)
       {
       }
       private void Visit(AST.Identifiers.Identifier i)
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

       private void Visit(AST.Enumeration e)
       {
            Model.Enumeration enumeration = new Model.Enumeration(e.Identifier,current_scope);
            foreach(var enumerator in e.Enumerators)
            {
                // The int.parse can throw, but we check this in SA so if we get something here that's invalid, just let it crash & burn
                enumeration.AddEnumerator(new Model.Enumerator(enumerator.Name, int.Parse(enumerator.Value)));
            }
            this.model.enumerations.Add(enumeration);
       }

       private void Visit(AST.Object o)
       {
            current_scope.StepInto(o.identifier);
            current_offset_in_object = 0;
            current_object.Push(new Model.Object(o.identifier));
            foreach (var field in o.fields)
                Visit((dynamic)field);
            this.model.objects.Add(current_object.Pop());
            current_scope.StepOut();
       }
       private void Visit(AST.Field f)
       {
            current_field_name = f.Name;
            uint size = 0;
            try { size = f.Type.SizeInBits(); }
            catch (Exception){ }
            current_object.Peek().AddField(new Model.Field(f.Name, current_offset_in_object, size));
            Visit((dynamic)f.Type);
            /* TODO: we need to get the the size in bits of the type.
            I added a SizeInBits method to the field type, but that does not seem to make sense for some of the types,
            such as inline objects or identifiers. Also we have to deal with collections. Maybe thing about this a bit more...
            */
            current_offset_in_object += size;
        }
       private void Visit(AST.FieldType t)
       {
       }
   }
}

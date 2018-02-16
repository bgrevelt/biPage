using NUnit.Framework;
using System;
using System.Collections.Generic;
using BiPaGe.AST;
using BiPaGe.AST.Expressions;
using BiPaGe.AST.Constants;
using System.Linq;

namespace Helpers
{
    public class SimpleBuilder
    {
        public static Parser Build(String input)
        {
            var errors = new List<BiPaGe.SemanticAnalysis.Error>();
            var warnings = new List<BiPaGe.SemanticAnalysis.Warning>();
            var builder = new BiPaGe.AST.Builder(errors, warnings);
            return builder.Program(input);
        }
    }

    public class Enumeration
    {
        public String Name;
        public FieldType Type;
        public List<(String, int)> Enumerators = new List<(string, int)>();
    }

    public class Object
    {
        public String Name;
        public List<(String, FieldType, IExpression, IFixer)> Fields = new List<(string, FieldType, IExpression, IFixer)>();
    }

    public class ProgramBuilder
    {
        private List<Enumeration> Enumerations = new List<Enumeration>();
        private List<Object> Objects = new List<Object>();

        public Enumeration AddEnumeration()
        {
            var new_enum = new Enumeration();
            Enumerations.Add(new_enum);
            return new_enum;
        }

        public Object AddObject()
        {
            var new_object = new Object();
            Objects.Add(new_object);
            return new_object;
        }

        public void Validate(IASTNode program)
        {
            program.Validate(ToAst());
        }

        private IASTNode ToAst()
        {
            List<BiPaGe.AST.Element> elements = new List<Element>();
            foreach (var e in this.Enumerations)
                elements.Add(new BiPaGe.AST.Enumeration(null, e.Name, e.Type, e.Enumerators.Select(f => new BiPaGe.AST.Enumerator(null, f.Item1, f.Item2.ToString())).ToList()));
            foreach (var o in this.Objects)
                elements.Add(new BiPaGe.AST.Object(null, o.Name, o.Fields.Select(f => new Field(null, f.Item1, f.Item2, f.Item3, f.Item4)).ToList()));

            return new BiPaGe.AST.Parser(null, "No name yet", elements);
        }

    }
}
using System;
using System.Collections.Generic;
using BiPaGe.SemanticAnalysis;

namespace BiPaGe.AST.Constants
{
    public class ObjectConstant : ASTNode, IFixer
    {
        public List<ObjectField> FieldFixers { get; }
        public ObjectConstant(SourceInfo sourceInfo, List<ObjectField> field_fixers) : base(sourceInfo)
        {
            this.FieldFixers = field_fixers;
        }

        public override void Print(int indentLevel)
        {
            PrintIndented("ObjectConstant:", indentLevel);
            foreach (var ff in FieldFixers)
                PrintIndented(String.Format("{0}: {1}", ff.FieldId, ff.Value), indentLevel + 1);
        }

        public override bool CheckSemantics(IList<Error> errors, IList<Warning> warnings)
        {
            throw new NotImplementedException();
        }
    }
}

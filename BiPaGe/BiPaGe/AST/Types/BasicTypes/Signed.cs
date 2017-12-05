﻿using System;
namespace BiPaGe.AST.Types.BasicTypes
{
    public class Signed : SizedType
    {
        public Signed(String typeId) : base(typeId)
        {
        }

        public override void Print(int indentLevel)
        {
            PrintIndented(String.Format("{0} bit signed integer", Size), indentLevel);
        }
    }
}
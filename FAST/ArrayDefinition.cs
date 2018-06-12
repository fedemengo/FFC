using System;
using System.Collections.Generic;
using FFC.FLexer;
using FFC.FParser;

namespace FFC.FAST
{
    class ArrayDefinition : FPrimary
    {
        public ExpressionList values;
        public ArrayDefinition(ExpressionList values, TextSpan span = null)
        {
            this.values = values;
            this.Span = span;
        }

        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Array definition");
            values.Print(tabs + 1);
        }
    }
}
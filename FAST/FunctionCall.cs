using System.Collections.Generic;
using System;
using FFC.FParser;
using FFC.FGen;

namespace FFC.FAST
{
    public class FunctionCall : FSecondary
    {
        public FSecondary toCall;
        public ExpressionList exprs;
        public FunctionCall(FSecondary toCall, ExpressionList exprs, TextSpan span)
        {
            this.Span = span;
            this.toCall = toCall;
            this.exprs = exprs;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Function call");
            toCall.Print(tabs + 1);
            exprs.Print(tabs + 1);
        }
    }
}

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
        public override void BuildType(SymbolTable st)
        {
            var t = toCall.GetValueType(st) as FunctionType;
            if(t == null) throw new NotImplementedException($"{Span} - Can't call function on {toCall.GetValueType(st)}.");
            valueType = t.returnType;
        }
    }
}

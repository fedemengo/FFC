using System.Collections.Generic;
using System;

namespace FFC.FAST
{
    class FunctionCall : FSecondary
    {
        public FSecondary toCall;
        public ExpressionList exprs;
        public FunctionCall(FSecondary toCall, ExpressionList exprs)
        {
            this.toCall = toCall;
            this.exprs = exprs;
        }
        public override void Print(int tabs)
        {
            for(int i = 0; i < tabs; i++)
                Console.Write("\t");
            Console.WriteLine("Functlion cal");
            toCall.Print(tabs + 1);
            exprs.Print(tabs + 1);
        }
    }
}
using System.Collections.Generic;

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
    }
}
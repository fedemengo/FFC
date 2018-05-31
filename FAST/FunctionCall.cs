using System.Collections.Generic;

namespace FFC.FAST
{
    class FunctionCall : FSecondary
    {
        public FSecondary toCall;
        public List<FExpression> exprs;
        public FunctionCall(FSecondary toCall, List<FExpression> exprs)
        {
            this.toCall = toCall;
            this.exprs = exprs;
        }
    }
}
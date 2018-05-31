using System.Collections.Generic;

namespace FFC.FAST
{
    class FunctionCall : FSecondary
    {
        public FSecondary toCall;
        public List<FExpression> exprs;
    }
}
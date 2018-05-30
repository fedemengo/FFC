using System.Collections.Generic;

namespace FAST
{
    class FunctionCall : FSecondary
    {
        public FSecondary toCall;
        public List<FExpression> exprs;
    }
}
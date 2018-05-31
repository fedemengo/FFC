using System.Collections.Generic;

namespace FFC.FAST
{
    class ArrayDefinition : FPrimary
    {
        public List<FExpression> values;
        public ArrayDefinition(FExpression value)
        {
            values = new List<FExpression>{value};
        }
    }
}
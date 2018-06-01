using System.Collections.Generic;

namespace FFC.FAST
{
    class ArrayDefinition : FPrimary
    {
        public ExpressionList values;
        public ArrayDefinition(ExpressionList values)
        {
            this.values = values;
        }
    }
}
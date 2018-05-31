using System.Collections.Generic;
namespace FFC.FAST
{
    abstract class Conditional : FPrimary
    {
        public FExpression condition;
        public FExpression ifTrue;
        public FExpression ifFalse;
    }
}
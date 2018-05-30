using System.Collections.Generic;
namespace FAST
{
    abstract class Conditional : FPrimary
    {
        public FExpression condition;
        public FExpression ifTrue;
        public FExpression ifFalse;
    }
}
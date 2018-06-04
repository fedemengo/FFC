using System;
using System.Collections.Generic;

namespace FFC.FAST
{
    class Conditional : FPrimary
    {
        public FExpression condition;
        public FExpression ifTrue;
        public FExpression ifFalse;
        public Conditional(FExpression condition, FExpression ifTrue, FExpression ifFalse)
        {
            this.condition = condition;
            this.ifTrue = ifTrue;
            this.ifFalse = ifFalse;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Conditional expression");
            condition.Print(tabs + 1);
            ifTrue.Print(tabs + 1);
            ifFalse.Print(tabs + 1);
        }

    }
}
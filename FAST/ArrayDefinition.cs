using System;
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

        public override void Print(int tabs)
        {
            for(int i = 0; i < tabs; i++)
                Console.Write("\t");
            Console.WriteLine("Array definition");
            values.Print(tabs + 1);
        }
    }
}
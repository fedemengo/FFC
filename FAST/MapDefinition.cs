using System.Collections.Generic;
using System;

namespace FFC.FAST
{
    class MapDefinition : FPrimary
    {
        public ExpressionPairList entries;

        public MapDefinition(ExpressionPairList entries)
        {
            this.entries = entries;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Map definition");
            entries.Print(tabs + 1);
        }
    }
    class ExpressionPairList : FASTNode
    {
        public List<ExpressionPair> pairs;
        public ExpressionPairList(ExpressionPair pair)
        {
            pairs = new List<ExpressionPair>{pair};
        }
        public ExpressionPairList()
        {
            pairs = new List<ExpressionPair>();
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Expression pair list");
            foreach(var e in pairs)
                e.Print(tabs + 1);
        }
    }
    class ExpressionPair : FASTNode
    {
        public FExpression first;
        public FExpression second;

        public ExpressionPair(FExpression first, FExpression second)
        {
            this.first = first;
            this.second = second;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Expression pair");
            first.Print(tabs +1 );
            second.Print(tabs +1 );
        }
    }
}
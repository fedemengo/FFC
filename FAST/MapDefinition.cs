using System.Collections.Generic;
using System;
using FFC.FParser;
using FFC.FGen;

namespace FFC.FAST
{
    public class MapDefinition : FPrimary
    {
        public ExpressionPairList entries;

        public MapDefinition(ExpressionPairList entries, TextSpan span = null)
        {
            this.Span = span;
            this.entries = entries;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Map definition");
            entries.Print(tabs + 1);
        }
    }
    public class ExpressionPairList : FASTNode
    {
        public List<ExpressionPair> pairs;
        public ExpressionPairList(ExpressionPair pair, TextSpan span = null)
        {
            this.Span = span;
            pairs = new List<ExpressionPair>{pair};
        }
        public ExpressionPairList(TextSpan span = null)
        {
            this.Span = span;
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
    public class ExpressionPair : FASTNode
    {
        public FExpression first;
        public FExpression second;

        public ExpressionPair(FExpression first, FExpression second, TextSpan span = null)
        {
            this.Span = span;
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
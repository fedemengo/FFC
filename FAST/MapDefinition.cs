using System.Collections.Generic;
using System;
using FFC.FParser;
using FFC.FGen;

namespace FFC.FAST
{
    public class MapDefinition : FPrimary
    {
        public ExpressionPairList Entries {get; set;}

        public MapDefinition(ExpressionPairList entries, TextSpan span = null)
        {
            Span = span;
            Entries = entries;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Map definition");
            Entries.Print(tabs + 1);
        }
    }
    public class ExpressionPairList : FASTNode
    {
        public List<ExpressionPair> pairs;
        public ExpressionPairList(ExpressionPair pair, TextSpan span = null)
        {
            Span = span;
            pairs = new List<ExpressionPair>{pair};
        }
        public ExpressionPairList(TextSpan span = null)
        {
            Span = span;
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
        public FExpression First {get; set;}
        public FExpression Second {get; set;}

        public ExpressionPair(FExpression first, FExpression second, TextSpan span = null)
        {
            First = first;
            Second = second;
            Span = span;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Expression pair");
            First.Print(tabs +1 );
            Second.Print(tabs +1 );
        }
    }
}
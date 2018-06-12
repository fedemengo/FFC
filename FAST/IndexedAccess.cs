using System.Collections.Generic;
using System;
using FFC.FParser;

namespace FFC.FAST
{
    class IndexedAccess : FSecondary
    {
        public FSecondary item;
        public Indexer index;

        public IndexedAccess(FSecondary item, Indexer index, TextSpan span)
        {
            this.Span = span;
            this.item = item;
            this.index = index;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Indexed access");
            item.Print(tabs + 1);
            index.Print(tabs + 1);
        }
    }
    abstract class Indexer : FASTNode
    {
    }
    class DotIndexer : Indexer
    {
        public Identifier id;
        public IntegerValue index;

        public DotIndexer(Identifier id, IntegerValue index, TextSpan span)
        {
            this.Span = span;
            this.id = id;
            this.index = index;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("DotIndexer");
            if(id != null) id.Print(tabs + 1);
            else index.Print(tabs + 1);
        }
    }
    class SquaresIndexer : Indexer
    {
        public FExpression index;

        public SquaresIndexer(FExpression index, TextSpan span)
        {
            this.Span = span;
            this.index = index;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Squares indexer");
            index.Print(tabs + 1);
        }
    }
}
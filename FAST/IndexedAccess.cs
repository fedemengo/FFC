using System.Collections.Generic;
using System;

namespace FFC.FAST
{
    class IndexedAccess : FSecondary
    {
        public FSecondary item;
        public Indexer index;

        public IndexedAccess(FSecondary item, Indexer index)
        {
            this.item = item;
            this.index = index;
        }
        public override void Print(int tabs)
        {
            for(int i = 0; i < tabs; i++)
                Console.Write("\t");
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

        public DotIndexer(Identifier id, IntegerValue index)
        {
            this.id = id;
            this.index = index;
        }
        public override void Print(int tabs)
        {
            for(int i = 0; i < tabs; i++)
                Console.Write("\t");
            Console.WriteLine("DotIndexer");
            if(id != null) id.Print(tabs + 1);
            else index.Print(tabs + 1);
        }
    }
    class SquaresIndexer : Indexer
    {
        public FExpression index;

        public SquaresIndexer(FExpression index)
        {
            this.index = index;
        }
        public override void Print(int tabs)
        {
            for(int i = 0; i < tabs; i++)
                Console.Write("\t");
            Console.WriteLine("Squares indexer");
            index.Print(tabs + 1);
        }
    }
}
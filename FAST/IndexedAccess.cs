using System.Collections.Generic;

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
    }
    abstract class Indexer : FASTNode
    {
        
    }
    class DotIndexer : Indexer
    {
        public Identifier id;
        public IntegerValue index;

        public DotIndexer(Identifier id, IntegerType index)
        {
            this.id = id;
            this.index = index;
        }
    }
    class SquaresIndexer : Indexer
    {
        public FExpression index;

        public SquaresIndexer(FExpression index)
        {
            this.index = index;
        }
    }
}
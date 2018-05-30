using System.Collections.Generic;

namespace FAST
{
    class IndexedAccess : FSecondary
    {
        public FSecondary item;
        public Indexer index;
    }
    abstract class Indexer : FASTNode
    {
        
    }
    class DotIndexer : Indexer
    {
        public Identifier id;
        public IntegerValue index;
    }
    class SquaresIndexer : Indexer
    {
        public FExpression index;
    }
}
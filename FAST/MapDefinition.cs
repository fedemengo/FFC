using System.Collections.Generic;

namespace FFC.FAST
{
    class MapDefinition : FPrimary
    {
        public List<Pair> entries;

        public MapDefinition(List<Pair> entries)
        {
            this.entries = entries;
        }
    }
    class Pair : FASTNode
    {
        public FExpression first;
        public FExpression second;

        public Pair(FExpression first, FExpression second)
        {
            this.first = first;
            this.second = second;
        }
    }
}
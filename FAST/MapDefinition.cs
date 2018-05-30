using System.Collections.Generic;

namespace FAST
{
    class MapDefinition : FPrimary
    {
        public List<Pair> entries;
    }
    class Pair : FASTNode
    {
        public FExpression first;
        public FExpression second;
    }
}
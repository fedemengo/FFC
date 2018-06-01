using System.Collections.Generic;

namespace FFC.FAST
{
    class MapDefinition : FPrimary
    {
        public ExpressionPairList entries;

        public MapDefinition(ExpressionPairList entries)
        {
            this.entries = entries;
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
            pairs = null;
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
    }
}
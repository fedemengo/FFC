using System.Collections.Generic;

namespace FAST
{
    class TupleDefinition : FPrimary //this count as (Expression)
    {
        public List<TupleElement> elements;
    }
    class TupleElement : FASTNode
    {
        public Identifier id;
        public FExpression value;
    }
}
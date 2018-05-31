using System.Collections.Generic;

namespace FFC.FAST
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
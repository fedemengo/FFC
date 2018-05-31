using System.Collections.Generic;

namespace FFC.FAST
{
    class TupleDefinition : FPrimary //this count as (Expression)
    {
        public List<TupleElement> elements;

        public TupleDefinition(List<TupleElement> elements)
        {
            this.elements = elements;
        } 
    }
    class TupleElement : FASTNode
    {
        public Identifier id;
        public FExpression value;

        public TupleElement(Identifier id, FExpression value)
        {
            this.id = id;
            this.value = value;
        }
    }
}
using System.Collections.Generic;

namespace FFC.FAST
{
    class TupleDefinition : FPrimary //this count as (Expression)
    {
        public TupleElementList elements;

        public TupleDefinition(TupleElementList elements)
        {
            this.elements = elements;
        } 
    }
    class TupleElementList : FASTNode
    {
        public List<TupleElement> elements;
        public TupleElementList()
        {
            elements = new List<TupleElement>();
        }
        public TupleElementList(TupleElement element)
        {
            elements = new List<TupleElement>{element};
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
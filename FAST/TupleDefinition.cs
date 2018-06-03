using System.Collections.Generic;
using System;

namespace FFC.FAST
{
    class TupleDefinition : FPrimary //this count as (Expression)
    {
        public TupleElementList elements;

        public TupleDefinition(TupleElementList elements)
        {
            this.elements = elements;
        }
        public override void Print(int tabs)
        {
            for(int i = 0; i < tabs; i++)
                Console.Write("\t");
            Console.WriteLine("Tuple definition");
            elements.Print(tabs + 1);
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
        public override void Print(int tabs)
        {
            for(int i = 0; i < tabs; i++)
                Console.Write("\t");
            if(elements.Count == 1 && elements[0].id == null)
            {
                Console.WriteLine("(Expression) [deduced from TupleElemList]");
                elements[0].value.Print(tabs + 1); 
            }
            else
            {
                Console.WriteLine("Tuple elements list");
                foreach(var element in elements)
                    element.Print(tabs + 1);
            }
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
        public override void Print(int tabs)
        {
            for(int i = 0; i < tabs; i++)
                Console.Write("\t");
            Console.WriteLine("Tuple element");
            if(id != null) id.Print(tabs + 1);
            value.Print(tabs + 1);   
        }
    }
}
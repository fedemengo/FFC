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
        public bool IsExpression()
        {
            return elements.elements.Count == 1 && elements.elements[0].id == null;
        }
        public override void Print(int tabs)
        {
            //ignores print of (tuple definition) if we are dealing with a single expression
            if(IsExpression())
                elements.Print(tabs);
            else
            {
                PrintTabs(tabs);
                Console.WriteLine("Tuple definition");
                elements.Print(tabs + 1);
            }
        }
        public override void Generate(System.Reflection.Emit.ILGenerator generator)
        {
            //only for expression, tuple to do!
            if(IsExpression()) elements.elements[0].value.Generate(generator);
            else throw new NotImplementedException("Tuple definition is not implemented yet");
        }
        public override void BuildType()
        {
            if(IsExpression())
            {
                //get inner expression type
                ValueType = elements.elements[0].value.ValueType;   
            }
            else throw new NotImplementedException("Tuple definition is not implemented yet");
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
            PrintTabs(tabs);
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
        public override void Generate(System.Reflection.Emit.ILGenerator generator)
        {
            throw new NotImplementedException("tuple element list not implemented");
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
            PrintTabs(tabs);
            Console.WriteLine("Tuple element");
            if(id != null) id.Print(tabs + 1);
            value.Print(tabs + 1);   
        }
    }
}
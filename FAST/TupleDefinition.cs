using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using FFC.FParser;
using FFC.FGen;
using FFC.FRunTime;

namespace FFC.FAST
{
    public class TupleDefinition : FPrimary //this count as (Expression)
    {
        public TupleElementList elements;

        public TupleDefinition(TupleElementList elements, TextSpan span)
        {
            this.Span = span;
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
        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            //only for expression, tuple to do!
            if(IsExpression()) 
            {
                elements.elements[0].value.Generate(generator, currentType, st, exitLabel, conditionLabel);
            }
            else
            {
                //throw new NotImplementedException("Tuple definition is not implemented yet");
                // construct a tuple
                LocalBuilder localTuple = generator.DeclareLocal(typeof(FTuple));
                generator.Emit(OpCodes.Newobj, typeof(FTuple).GetConstructor(Type.EmptyTypes));
                Generator.EmitStore(generator, localTuple);
                foreach(TupleElement elem in elements.elements)
                {
                    Generator.EmitLoad(generator, localTuple);
                    elem.Generate(generator, currentType, st, exitLabel, conditionLabel);
                    Type elementType = elem.value.GetValueType(st).GetRunTimeType();
                    generator.Emit(OpCodes.Callvirt, typeof(FTuple).GetMethod("Add", new Type[]{typeof(object)}));
                }
                Generator.EmitLoad(generator, localTuple);
            }
        }
        public override void BuildType(SymbolTable st)
        {
            if(IsExpression())
            {
                //get inner expression type
                valueType = elements.elements[0].value.GetValueType(st);   
            }
            else
            {
                TypeList types = new TypeList();
                foreach(TupleElement e in elements.elements)
                    types.Add(e.value.GetValueType(st));

                TupleType tupleType = new TupleType(types);
                for(int i=0; i<elements.elements.Count; ++i)
                    if(elements.elements[i].id != null)
                        tupleType.names.Add(elements.elements[i].id.name, i + 1);

                valueType = tupleType;
                // throw new NotImplementedException("Tuple definition is not implemented yet");
            }
        }
    }
    public class TupleElementList : FASTNode
    {
        public List<TupleElement> elements;
        public TupleElementList(TextSpan span)
        {
            this.Span = span;
            elements = new List<TupleElement>();
        }
        public TupleElementList(TupleElement element, TextSpan span)
        {
            this.Span = span;
            elements = new List<TupleElement>{element};
        }
        public void Add(TupleElement e) => elements.Add(e);
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
    }
    public class TupleElement : FASTNode
    {
        public Identifier id;
        public FExpression value;

        public TupleElement(Identifier id, FExpression value, TextSpan span)
        {
            this.Span = span;
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
        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            value.Generate(generator, currentType, st, exitLabel, conditionLabel);
        }
    }
}
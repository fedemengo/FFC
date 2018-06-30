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
        public TupleElementList ElemsList {get; set;}

        public TupleDefinition(TupleElementList elements, TextSpan span)
        {
            ElemsList = elements;
            Span = span;
        }
        public bool IsExpression() => ElemsList.Elements.Count == 1 && ElemsList.Elements[0].Id == null;
        public override void Print(int tabs)
        {
            //ignores print of (tuple definition) if we are dealing with a single expression
            if(IsExpression())
                ElemsList.Print(tabs);
            else
            {
                PrintTabs(tabs);
                Console.WriteLine("Tuple definition");
                ElemsList.Print(tabs + 1);
            }
        }
        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            if(IsExpression()) 
                ElemsList.Elements[0].Value.Generate(generator, currentType, st, exitLabel, conditionLabel);
            else
            {
                // construct a tuple
                LocalBuilder localTuple = generator.DeclareLocal(typeof(FTuple));
                generator.Emit(OpCodes.Newobj, typeof(FTuple).GetConstructor(Type.EmptyTypes));
                Generator.EmitStore(generator, localTuple);
                foreach(TupleElement elem in ElemsList.Elements)
                {
                    Generator.EmitLoad(generator, localTuple);
                    elem.Generate(generator, currentType, st, exitLabel, conditionLabel);
                    Type elementType = elem.Value.GetValueType(st).GetRunTimeType();
                    generator.Emit(OpCodes.Callvirt, typeof(FTuple).GetMethod("Add", new Type[]{typeof(object)}));
                }
                Generator.EmitLoad(generator, localTuple);
            }
        }
        public override void BuildValueType(SymbolTable st)
        {
            //get inner expression type
            if(IsExpression())
                ValueType = ElemsList.Elements[0].Value.GetValueType(st);   
            else
            {
                TypeList types = new TypeList();
                foreach(TupleElement e in ElemsList.Elements)
                    types.Add(e.Value.GetValueType(st));

                TupleType tupleType = new TupleType(types);
                for(int i=0; i<ElemsList.Elements.Count; ++i)
                    if(ElemsList.Elements[i].Id != null)
                        tupleType.Names.Add(ElemsList.Elements[i].Id.Name, i + 1);

                ValueType = tupleType;
            }
        }
    }
    public class TupleElementList : FASTNode
    {
        public List<TupleElement> Elements {get; set;}
        public TupleElementList(TextSpan span)
        {
            Elements = new List<TupleElement>();
            Span = span;
        }
        public TupleElementList(TupleElement element, TextSpan span)
        {
            Elements = new List<TupleElement>{element};
            Span = span;
        }
        public void Add(TupleElement e) => Elements.Add(e);
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            if(Elements.Count == 1 && Elements[0].Id == null)
            {
                Console.WriteLine("(Expression) [deduced from TupleElemList]");
                Elements[0].Value.Print(tabs + 1); 
            }
            else
            {
                Console.WriteLine("Tuple elements list");
                foreach(var element in Elements)
                    element.Print(tabs + 1);
            }
        }
    }
    public class TupleElement : FASTNode
    {
        public Identifier Id {get; set;}
        public FExpression Value {get; set;}

        public TupleElement(Identifier id, FExpression value, TextSpan span)
        {
            Id = id;
            Value = value;
            Span = span;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Tuple element");
            if(Id != null) Id.Print(tabs + 1);
            Value.Print(tabs + 1);   
        }
        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            Value.Generate(generator, currentType, st, exitLabel, conditionLabel);
        }
    }
}
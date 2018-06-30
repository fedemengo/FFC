using System;
using System.Reflection;
using System.Reflection.Emit;

using FFC.FParser;
using FFC.FGen;
using FFC.FRunTime;

namespace FFC.FAST
{
    public class IndexedAccess : FSecondary
    {
        public FSecondary Container {get; set;}
        public Indexer Index {get; set;}

        public IndexedAccess(FSecondary container, Indexer index, TextSpan span)
        {
            Container = container;
            Index = index;
            Span = span;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Indexed access");
            Container.Print(tabs + 1);
            Index.Print(tabs + 1);
        }
        public override void BuildValueType(SymbolTable st)
        {
            ValueType = Container.GetValueType(st);
            if(ValueType is ArrayType) ValueType = (ValueType as ArrayType).Type;
            else if(ValueType is TupleType)
            {
                if(Index is DotIndexer == false)
                    throw new FCompilationException($"{Span} - Can't generate not DotIndexer for tuple");

                ValueType = (ValueType as TupleType).GetIndexType(Index as DotIndexer);
            }
            else throw new FCompilationException($"{Span} - Can't use indexers on type {ValueType.GetType().Name}");
        }
        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            Container.Generate(generator, currentType, st, exitLabel, conditionLabel);
            if(Index is SquaresIndexer)
            { 
                Index.Generate(generator, currentType, st, exitLabel, conditionLabel);
                generator.Emit(OpCodes.Callvirt, Container.GetValueType(st).GetRunTimeType().GetMethod("get_Item", new Type[]{Index.GetValueType(st).GetRunTimeType()}));
            }
            else if(Index is DotIndexer)
            {
                IntegerValue tupleIndex = new IntegerValue((Container.GetValueType(st) as TupleType).GetMappedIndex(Index as DotIndexer), Span);
                tupleIndex.Generate(generator, currentType, st, exitLabel, conditionLabel);
                generator.Emit(OpCodes.Callvirt, Container.GetValueType(st).GetRunTimeType().GetMethod("Get", new Type[]{typeof(FInteger)}));
            }
            else throw new FCompilationException($"{Span} - Generation not supported for {Index.GetType().Name}");
        }
    }
    public abstract class Indexer : FExpression
    {
    }
    public class DotIndexer : Indexer
    {
        public Identifier Id {get; set;}
        public IntegerValue Index {get; set;}

        public DotIndexer(Identifier id, IntegerValue index, TextSpan span)
        {
            Id = id;
            Index = index;
            Span = span;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("DotIndexer");
            if(Id != null) Id.Print(tabs + 1);
            else Index.Print(tabs + 1);
        }
        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            if(Id == null)
                Index.Generate(generator, currentType, st, exitLabel, conditionLabel);
            else
            {
                generator.Emit(OpCodes.Ldstr, Id.Name);
                generator.Emit(OpCodes.Newobj, typeof(FString).GetConstructor(new Type[]{typeof(string)}));
            }
        }

        public override void BuildValueType(SymbolTable st)
        {
            ValueType = Id.GetValueType(st);
        }
    }
    public class SquaresIndexer : Indexer
    {
        public FExpression IndexExpr {get; set;}

        public SquaresIndexer(FExpression index, TextSpan span)
        {
            IndexExpr = index;
            Span = span;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Squares indexer");
            IndexExpr.Print(tabs + 1);
        }

        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            //object is already emitted - we emit expression
            IndexExpr.Generate(generator, currentType, st, exitLabel, conditionLabel);
        }

        public override void BuildValueType(SymbolTable st)
        {
            ValueType = IndexExpr.GetValueType(st);
        }
    }
}
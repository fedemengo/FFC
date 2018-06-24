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
        public FSecondary container;
        public Indexer index;

        public IndexedAccess(FSecondary container, Indexer index, TextSpan span)
        {
            this.Span = span;
            this.container = container;
            this.index = index;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Indexed access");
            container.Print(tabs + 1);
            index.Print(tabs + 1);
        }
        public override void BuildType(SymbolTable st)
        {
            valueType = container.GetValueType(st);
            if(valueType is ArrayType) valueType = (valueType as ArrayType).type;
            else if(valueType is TupleType)
            {
                if(index is DotIndexer == false)
                    throw new NotImplementedException($"{Span} - Can't generate not DotIndexer for tuple");

                valueType = (valueType as TupleType).GetIndexType(index as DotIndexer);
            }
            else throw new NotImplementedException($"{Span} - Can't use indexers on type {valueType.GetType().Name}");
        }
        public override void Generate(ILGenerator generator, SymbolTable st)
        {
            container.Generate(generator, st);
            if(index is SquaresIndexer)
            { 
                index.Generate(generator, st);
                generator.Emit(OpCodes.Callvirt, container.GetValueType(st).GetRunTimeType().GetMethod("get_Item", new Type[]{index.GetValueType(st).GetRunTimeType()}));
            }
            else if(index is DotIndexer)
            {
                IntegerValue tupleIndex = new IntegerValue((container.GetValueType(st) as TupleType).GetMappedIndex(index as DotIndexer), Span);
                tupleIndex.Generate(generator, st);
                generator.Emit(OpCodes.Callvirt, container.GetValueType(st).GetRunTimeType().GetMethod("Get", new Type[]{typeof(FInteger)}));
            }
            else  throw new NotImplementedException($"{Span} - Generation not supported for {index.GetType().Name}");
        }
    }
    abstract public class Indexer : FExpression
    {
    }
    public class DotIndexer : Indexer
    {
        public Identifier id;
        public IntegerValue index;

        public DotIndexer(Identifier id, IntegerValue index, TextSpan span)
        {
            this.Span = span;
            this.id = id;
            this.index = index;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("DotIndexer");
            if(id != null) id.Print(tabs + 1);
            else index.Print(tabs + 1);
        }
        public override void Generate(ILGenerator generator, SymbolTable st)
        {
            if(id == null)
                index.Generate(generator, st);
            else
            {
                generator.Emit(OpCodes.Ldstr, id.name);
                generator.Emit(OpCodes.Newobj, typeof(FString).GetConstructor(new Type[]{typeof(string)}));
            }
        }

        public override void BuildType(SymbolTable st)
        {
            valueType = id.GetValueType(st);
        }
    }
    public class SquaresIndexer : Indexer
    {
        public FExpression index;

        public SquaresIndexer(FExpression index, TextSpan span)
        {
            this.Span = span;
            this.index = index;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Squares indexer");
            index.Print(tabs + 1);
        }

        public override void Generate(ILGenerator generator, SymbolTable st)
        {
            //object is already emitted - we emit expression
            index.Generate(generator, st);
        }

        public override void BuildType(SymbolTable st)
        {
            valueType = index.GetValueType(st);
        }

    }
}
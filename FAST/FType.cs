using System.Collections.Generic;
using System;
using System.Reflection.Emit;
using FFC.FParser;
using FFC.FRunTime;
using FFC.FGen;

namespace FFC.FAST
{
    public abstract class FType : FASTNode
    {
        public virtual void ConvertTo(FType target, ILGenerator generator)
        {
            try
            {
                generator.Emit(OpCodes.Newobj, target.GetRunTimeType().GetConstructor(new Type[]{this.GetRunTimeType()}));
            }
            catch (Exception)
            {
                throw new NotImplementedException($"{Span} - No conversion from {this.GetType().Name} to {target.GetType().Name}");
            }
        }
        public virtual Type GetRunTimeType() => throw new NotImplementedException($"{Span} - RunTimeType not available for {GetType().Name}");
    }
    public class TypeList : FType
    {
        public List<FType> types;
        public TypeList(TextSpan span = null)
        {
            this.Span = span;
            types = new List<FType>();
        }
        public TypeList(FType type, TextSpan span = null)
        {
            this.Span = span;
            types = new List<FType>{type};
        }
        public void Add(FType type) => types.Add(type);
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Type list");
            foreach(FType t in types)
                t.Print(tabs + 1);
        }
    }

    abstract class NumericType : FType
    {
    }
    class IntegerType : NumericType
    {
        public IntegerType(TextSpan span = null)
        {
            this.Span = span;

        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Integer type");
        }
        public override Type GetRunTimeType() => typeof(FInteger);
    }

    class RealType : NumericType
    {
        public RealType(TextSpan span = null)
        {
            this.Span = span;

        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Real type");
        }
        public override Type GetRunTimeType() => typeof(FReal);
    }

    class ComplexType : NumericType
    {
        public ComplexType(TextSpan span = null)
        {
            this.Span = span;

        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Complex type");
        }
        public override Type GetRunTimeType() => typeof(FComplex);
    }

    class RationalType : NumericType
    {
        public RationalType(TextSpan span = null)
        {
            this.Span = span;

        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Rational type");
        }
        public override Type GetRunTimeType() => typeof(FRational);
    }

    class StringType : FType
    {
        public StringType(TextSpan span = null)
        {
            this.Span = span;

        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("String type");
        }
        public override Type GetRunTimeType() => typeof(FString);
    }

    class BooleanType : FType
    {
        public BooleanType(TextSpan span = null)
        {
            this.Span = span;

        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Boolean type");
        }
        public override Type GetRunTimeType() => typeof(FBoolean);
    }
    
    public class FunctionType : FType
    {
        public TypeList paramTypes;
        public FType returnType;
        public FunctionType(TypeList paramTypes, FType returnType, TextSpan span = null)
        {
            this.Span = span;
            this.paramTypes = paramTypes;
            this.returnType = returnType;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Function type");
            paramTypes.Print(tabs + 1);
            if(returnType != null) returnType.Print(tabs + 1);
        }
        public override Type GetRunTimeType()
        {
            throw new NotImplementedException($"{Span} Function RunTimeType not yet implemented");
        }
    }

    public abstract class IterableType : FType
    {
        public FType type;
    }

    public class EllipsisType : IterableType
    {
        public EllipsisType() => type = new IntegerType();
        public override Type GetRunTimeType() => typeof(FEllipsis);
    }
    public class ArrayType : IterableType
    {
        public ArrayType(FType type, TextSpan span = null)
        {
            this.Span = span;
            this.type = type;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Array type");
            type.Print(tabs + 1);
        }
        public override Type GetRunTimeType()
        {
            return typeof(FArray<>).MakeGenericType(type.GetRunTimeType());
        }
    }
    public class MapType : FType
    {
        public FType key;
        public FType value;
        public MapType(FType key, FType value, TextSpan span = null)
        {
            this.key = key;
            this.value = value;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Map type");
            key.Print(tabs + 1);
            value.Print(tabs + 1);
        }
        public override Type GetRunTimeType() => typeof(FMap);
    }
    public class TupleType : FType
    {
        public TypeList types;
        public Dictionary<string, int> names;
        public TupleType(TypeList types, TextSpan span = null)
        {
            this.Span = span;
            this.types = types;
            names = new Dictionary<string, int>();
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Tuple type");
            types.Print(tabs + 1);
        }

        public FType GetIndexType(DotIndexer index) => types.types[(index.id != null ? names[index.id.name] : index.index.value) - 1];

        public int GetMappedIndex(DotIndexer index) => index.id != null ? names[index.id.name] : index.index.value;
        
        public override Type GetRunTimeType() => typeof(FTuple);
    }
}
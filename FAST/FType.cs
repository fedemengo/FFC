using System.Collections.Generic;
using System;
using System.Reflection.Emit;
using FFC.FParser;
using FFC.FRunTime;

namespace FFC.FAST
{
    abstract class FType : FASTNode
    {
        public static void Convert(FType t1, FType t2, ILGenerator generator)
        {
            if(t2.GetType() != typeof(RealType))
                throw new NotImplementedException("Conversions are not implemented yet");
            //anything (?) to real (double) 
            generator.Emit(OpCodes.Conv_R8);
        }
        public abstract Type GetPrintableType();

        public abstract Type GetRunTimeType();
    }
    class TypeList : FASTNode
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
        public override Type GetPrintableType() => typeof(int);
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
        public override Type GetPrintableType() => typeof(double);
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
        public override Type GetPrintableType() 
        {
            throw new NotImplementedException("Complex printing is not currently implemented");
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
        public override Type GetPrintableType()
        {
            throw new NotImplementedException("Rational printing is not currently implented");
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
        public override Type GetPrintableType() 
        {
            return typeof(string);
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
        public override Type GetPrintableType() 
        {
            return typeof(bool);
        }
        public override Type GetRunTimeType() => typeof(FBoolean);
    }
    
    class FunctionType : FType
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
        public override Type GetPrintableType() 
        {
            throw new NotImplementedException("Function Type printing is not implemented");
        }
        public override Type GetRunTimeType()
        {
            throw new NotImplementedException("Function RunTimeType not yet implemented");
        }
    }
    class ArrayType : FType
    {
        public FType type;
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
        public override Type GetPrintableType() 
        {
            throw new NotImplementedException("Array type printing is not implemented");
        }
        public override Type GetRunTimeType() => typeof(FArray);
    }
    class MapType : FType
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
        public override Type GetPrintableType() 
        {
            throw new NotImplementedException("Map Type printing is not implemented");
        }
        public override Type GetRunTimeType() => typeof(FMap);
    }
    class TupleType : FType
    {
        public TypeList types;
        public TupleType(TypeList types, TextSpan span = null)
        {
            this.Span = span;
            this.types = types;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Tuple type");
            types.Print(tabs + 1);
        }
        public override Type GetPrintableType() 
        {
            throw new NotImplementedException("Tuple type printing is not implemented");
        }
        public override Type GetRunTimeType() => typeof(FTuple);
    }
}
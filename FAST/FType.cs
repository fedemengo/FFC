using System.Collections.Generic;
using System;

namespace FFC.FAST
{
    abstract class FType : FASTNode
    {
    }
    class TypeList : FASTNode
    {
        public List<FType> types;
        public TypeList()
        {
            types = new List<FType>();
        }
        public TypeList(FType type)
        {
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
        public IntegerType()
        {

        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Integer type");
        }
    }

    class RealType : NumericType
    {
        public RealType()
        {

        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Real type");
        }
    }

    class ComplexType : NumericType
    {
        public ComplexType()
        {

        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Complex type");
        }
    }

    class RationalType : NumericType
    {
        public RationalType()
        {

        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Rational type");
        }
    }

    class StringType : FType
    {
        public StringType()
        {

        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("String type");
        }
    }

    class BooleanType : FType
    {
        public BooleanType()
        {

        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Boolean type");
        }
    }
    
    class FunctionType : FType
    {
        public TypeList paramTypes;
        public FType returnType;
        public FunctionType(TypeList paramTypes, FType returnType)
        {
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
    }
    class ArrayType : FType
    {
        public FType type;
        public ArrayType(FType type)
        {
            this.type = type;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Array type");
            type.Print(tabs + 1);
        }
    }
    class MapType : FType
    {
        public FType key;
        public FType value;
        public MapType(FType key, FType value)
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
    }
    class TupleType : FType
    {
        public TypeList types;
        public TupleType(TypeList types)
        {
            this.types = types;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Tuple type");
            types.Print(tabs + 1);
        }
    }
}
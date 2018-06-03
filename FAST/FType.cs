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
            for(int i = 0; i < tabs; i++)
                Console.Write("\t");
            Console.WriteLine("Type list");
            foreach(FType t in types)
                t.Print(tabs + 1);
        }
    }
    class IntegerType : FType
    {
        public IntegerType()
        {

        }
        public override void Print(int tabs)
        {
            for(int i = 0; i < tabs; i++)
                Console.Write("\t");
            Console.WriteLine("Integer type");
        }
    }

    class RealType : FType
    {
        public RealType()
        {

        }
        public override void Print(int tabs)
        {
            for(int i = 0; i < tabs; i++)
                Console.Write("\t");
            Console.WriteLine("Real type");
        }
    }

    class ComplexType : FType
    {
        public ComplexType()
        {

        }
        public override void Print(int tabs)
        {
            for(int i = 0; i < tabs; i++)
                Console.Write("\t");
            Console.WriteLine("Complex type");
        }
    }

    class RationalType : FType
    {
        public RationalType()
        {

        }
        public override void Print(int tabs)
        {
            for(int i = 0; i < tabs; i++)
                Console.Write("\t");
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
            for(int i = 0; i < tabs; i++)
                Console.Write("\t");
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
            for(int i = 0; i < tabs; i++)
                Console.Write("\t");
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
            for(int i = 0; i < tabs; i++)
                Console.Write("\t");
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
            for(int i = 0; i < tabs; i++)
                Console.Write("\t");
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
            for(int i = 0; i < tabs; i++)
                Console.Write("\t");
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
            for(int i = 0; i < tabs; i++)
                Console.Write("\t");
            Console.WriteLine("Tuple type");
            types.Print(tabs + 1);
        }
    }
}
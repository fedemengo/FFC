using System.Collections.Generic;
using System;
using System.Reflection.Emit;

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
        public override Type GetPrintableType() 
        {
            return typeof(int);
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
        public override Type GetPrintableType() 
        {
            return typeof(double);
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
        public override Type GetPrintableType() 
        {
            throw new NotImplementedException("Complex printing is not currently implemented");
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
        public override Type GetPrintableType()
        {
            throw new NotImplementedException("Rational printing is not currently implented");
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
        public override Type GetPrintableType() 
        {
            return typeof(string);
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
        public override Type GetPrintableType() 
        {
            return typeof(bool);
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
        public override Type GetPrintableType() 
        {
            throw new NotImplementedException("Function Type printing is not implemented");
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
        public override Type GetPrintableType() 
        {
            throw new NotImplementedException("Array type printing is not implemented");
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
        public override Type GetPrintableType() 
        {
            throw new NotImplementedException("Map Type printing is not implemented");
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
        public override Type GetPrintableType() 
        {
            throw new NotImplementedException("Tuple type printing is not implemented");
        }
    }
}
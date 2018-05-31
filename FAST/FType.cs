using System.Collections.Generic;
namespace FFC.FAST
{
    abstract class FType : FASTNode
    {

    }
    class IntegerType : FType
    {
        public IntegerType()
        {

        }
    }

    class RealType : FType
    {
        public RealType()
        {

        }
    }

    class ComplexType : FType
    {
        public ComplexType()
        {

        }
    }

    class RationalType : FType
    {
        public RationalType()
        {

        }
    }

    class StringType : FType
    {
        public StringType()
        {

        }
    }

    class BooleanType : FType
    {
        public BooleanType()
        {

        }
    }
    
    class FuncType : FType
    {
        public List<FType> paramTypes;
        public FType returnType;
        public FuncType(List<FType> paramTypes, FType returnType)
        {
            this.paramTypes = paramTypes;
            this.returnType = returnType;
        }
    }
    class ArrayType : FType
    {
        public FType type;
        public ArrayType(FType type)
        {
            this.type = type;
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
    }
    class TupleType : FType
    {
        public List<FType> types;
        public TupleType(FType type)
        {
            this.types = new List<FType>{type};
        }
    }
}
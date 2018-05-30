using System.Collections.Generic;
namespace FAST
{
    abstract class FType : FASTNode
    {

    }
    class IntegerType : FType
    {

    }

    class RealType : FType
    {

    }

    class ComplexType : FType
    {

    }

    class RationalType : FType
    {

    }

    class StringType : FType
    {

    }

    class BooleanType : FType
    {

    }
    
    class FuncType : FType
    {
        public List<FType> paramTypes;
        public FType returnType;
    }
    class ArrayType : FType
    {
        public FType type;
    }
    class MapType : FType
    {
        public FType key;
        public FType value;
    }
    class TupleType : FType
    {
        public List<FType> types;
    }
}
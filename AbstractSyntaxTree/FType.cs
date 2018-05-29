using System.Collections.Generic;
namespace AbstractSyntaxTree
{
    class FType
    {

    }
    class IntegerType : FType
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
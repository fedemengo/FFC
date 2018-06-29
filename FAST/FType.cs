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
        public override bool Equals(object o)
        {
            FType t = o as FType;
            if (Object.ReferenceEquals(t, null))
                return false;
            return this.ToString() == t.ToString();
        }
        public override int GetHashCode() => this.ToString().GetHashCode();
        public static bool SameType(FType a, FType b)
        {
            Type t1 = a.GetType();
            Type t2 = b.GetType();
            //to avoid troubles
            if(t1 == typeof(DeducedVoidType)) t1 = typeof(VoidType);
            if(t2 == typeof(DeducedVoidType)) t2 = typeof(VoidType);
            if(t1 != t2) return false;
            //numeric types
            if(a is IntegerType || a is RealType || a is ComplexType || a is RationalType)
                return true;
            //other basic types
            if(a is StringType || a is VoidType || a is BooleanType)
                return true;
            //array type
            if(a is ArrayType)
                return SameType((a as ArrayType).type, (b as ArrayType).type);
            //map type
            if(a is MapType)
            {
                MapType ta = a as MapType;
                MapType tb = b as MapType;
                return SameType(ta.key, tb.key) && SameType(ta.value, tb.value);
            }
            //tuple type
            if(a is TupleType)
                return SameType((a as TupleType).types, (b as TupleType).types);
            //Type list
            if(a is TypeList)
            {
                TypeList ta = a as TypeList;
                TypeList tb = b as TypeList;
                //check number of types
                if(ta.types.Count != tb.types.Count)
                    return false;
                //check all types
                for(int i = 0; i < ta.types.Count; i++)
                    if(SameType(ta.types[i], tb.types[i]) == false)
                        return false;
                return true;
            }
            //function type
            if(a is FunctionType)
            {
                FunctionType ta = a as FunctionType;
                FunctionType tb = b as FunctionType;
                //check return type
                if(SameType(ta.returnType, tb.returnType) == false)
                    return false;
                //check params
                return SameType(ta.paramTypes, tb.paramTypes);
            }
            throw new NotImplementedException($"FType comparison for {a.GetType().Name} is not implemented");
        }

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
        public override string ToString()
        {
            string s = "(";
            foreach(var t in types)
                s += t.ToString() + ", ";
            if(s.Length >= 4) s = s.Remove(s.Length - 2);
            return s + ")";
        }
        public override bool Equals(object o)
        {
            TypeList tl = o as TypeList;
            if (Object.ReferenceEquals(tl, null))
                return false;
            return this.ToString() == tl.ToString();
        }
        public override int GetHashCode() => types.ToString().GetHashCode();
    }

    public abstract class NumericType : FType
    {
    }
    public class IntegerType : NumericType
    {
        public IntegerType(TextSpan span = null)
        {
            this.Span = span;

        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine(this);
        }
        public override string ToString() => "IntegerType";
        public override Type GetRunTimeType() => typeof(FInteger);
    }

    public class RealType : NumericType
    {
        public RealType(TextSpan span = null)
        {
            this.Span = span;

        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine(this);
        }
        public override string ToString() => "RealType";
        public override Type GetRunTimeType() => typeof(FReal);
    }

    public class ComplexType : NumericType
    {
        public ComplexType(TextSpan span = null)
        {
            this.Span = span;

        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine(this);
        }
        public override string ToString() => "ComplexType";
        public override Type GetRunTimeType() => typeof(FComplex);
    }

    public class RationalType : NumericType
    {
        public RationalType(TextSpan span = null)
        {
            this.Span = span;

        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine(this);
        }
        public override string ToString() => "RationalType";
        public override Type GetRunTimeType() => typeof(FRational);
    }

    public class StringType : FType
    {
        public StringType(TextSpan span = null)
        {
            this.Span = span;

        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine(this);
        }
        public override string ToString() => "StringType";
        public override Type GetRunTimeType() => typeof(FString);
    }

    public class BooleanType : FType
    {
        public BooleanType(TextSpan span = null) => this.Span = span;
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine(this);
        }
        public override string ToString() => "BooleanType";
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
        public override Type GetRunTimeType() => Generator.GetDelegate(this);
        public override string ToString() => "FunctionType: " + returnType.ToString() + paramTypes.ToString();
        public override bool Equals(object o)
        {
            FunctionType ft = o as FunctionType;
            if (Object.ReferenceEquals(ft, null))
                return false;
            if (this.ToString() != ft.ToString())
                return false;
            if(this.returnType.ToString() != ft.returnType.ToString())
                return false;
            return this.paramTypes.Equals(ft.paramTypes);
        }
        public override int GetHashCode() => paramTypes.ToString().GetHashCode() ^ returnType.ToString().GetHashCode();
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
        public override string ToString() => "ArrayType[" + type.ToString() + "]";
        public override Type GetRunTimeType() => typeof(FArray<>).MakeGenericType(type.GetRunTimeType());
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
        public override string ToString() => "MapType{" + key.ToString() + ": " + value.ToString() + "}";
        public override Type GetRunTimeType() => typeof(FMap<,>).MakeGenericType(new Type[]{key.GetRunTimeType(), value.GetRunTimeType()});
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

        public override string ToString()
        {
            string s = "TupleType(";
            foreach(FType type in types.types)
                s += type.ToString() + ", ";
            if(types.types.Count > 0) s = s.Remove(s.Length - 2);
            return s + ")";
        }

        public override bool Equals(object o)
        {
            TupleType ft = o as TupleType;
            if (Object.ReferenceEquals(ft, null))
                return false;
            if (this.ToString() != ft.ToString())
                return false;
            return this.types.ToString() == ft.types.ToString();
        }
        public override int GetHashCode() => types.ToString().GetHashCode();
    }

    public class VoidType : FType
    {
        public override string ToString() => "VoidType";
        public override Type GetRunTimeType() => typeof(void);
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine(this);
        }
        //we currently have no support to specify either span or other stuff. todo
        public VoidType(){}
    }
    public class DeducedVoidType : VoidType
    {
        public override string ToString() => "VoidType";
    }

}
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
                generator.Emit(OpCodes.Newobj, target.GetRunTimeType().GetConstructor(new Type[]{GetRunTimeType()}));
            }
            catch (Exception)
            {
                throw new FCompilationException($"{Span} - No conversion from {this} to {target}");
            }
        }

        //To ensure all FTypes have a proper ToString method, as we don't want ugly error messages
        public override abstract string ToString();
        
        public virtual Type GetRunTimeType() => throw new FCompilationException($"{Span} - RunTimeType not available for {GetType().Name}");
        
        public override bool Equals(object o)
        {
            FType type = o as FType;
            if (Object.ReferenceEquals(type, null))
                return false;
            return ToString() == type.ToString();
        }
        
        public override int GetHashCode() => ToString().GetHashCode();
        
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
                return SameType((a as ArrayType).Type, (b as ArrayType).Type);
            //map type
            if(a is MapType)
            {
                MapType ta = a as MapType;
                MapType tb = b as MapType;
                return SameType(ta.Key, tb.Key) && SameType(ta.Value, tb.Value);
            }
            //tuple type
            if(a is TupleType)
                return SameType((a as TupleType).TypesList, (b as TupleType).TypesList);
            //Type list
            if(a is TypeList)
            {
                TypeList ta = a as TypeList;
                TypeList tb = b as TypeList;
                //check number of types
                if(ta.Types.Count != tb.Types.Count)
                    return false;
                //check all types
                for(int i = 0; i < ta.Types.Count; i++)
                    if(SameType(ta.Types[i], tb.Types[i]) == false)
                        return false;
                return true;
            }
            //function type
            if(a is FunctionType)
            {
                FunctionType ta = a as FunctionType;
                FunctionType tb = b as FunctionType;
                //check return type
                if(SameType(ta.ReturnType, tb.ReturnType) == false)
                    return false;
                //check params
                return SameType(ta.ParamsList, tb.ParamsList);
            }
            throw new FCompilationException($"FType comparison for {a} is not implemented");
        }
    }

    public class TypeList : FType
    {
        public List<FType> Types {get; set;}
        public TypeList(TextSpan span = null)
        {
            Types = new List<FType>();
            Span = span;
        }
        public TypeList(FType type, TextSpan span = null)
        {
            Types = new List<FType>{type};
            Span = span;
        }
        public void Add(FType type) => Types.Add(type);
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Type list");
            foreach(FType t in Types)
                t.Print(tabs + 1);
        }
        public override string ToString()
        {
            string s = "(";
            foreach(var t in Types)
                s += t.ToString() + ", ";
            if(s.Length >= 4) s = s.Remove(s.Length - 2);
            return s + ")";
        }
        public override bool Equals(object o)
        {
            TypeList typeList = o as TypeList;
            if (Object.ReferenceEquals(typeList, null))
                return false;
            return ToString() == typeList.ToString();
        }
        public override int GetHashCode() => Types.ToString().GetHashCode();
    }

    public abstract class NumericType : FType
    {
    }
    public class IntegerType : NumericType
    {
        public IntegerType(TextSpan span = null)
        {
            Span = span;
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
            Span = span;
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
            Span = span;
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
            Span = span;
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
            Span = span;
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
        public BooleanType(TextSpan span = null) => Span = span;
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
        public TypeList ParamsList {get; set;}
        public FType ReturnType {get; set;}
        public FunctionType(TypeList paramTypes, FType returnType, TextSpan span = null)
        {
            ParamsList = paramTypes;
            ReturnType = returnType;
            Span = span;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Function type");
            ParamsList.Print(tabs + 1);
            if(ReturnType != null) ReturnType.Print(tabs + 1);
        }
        public override Type GetRunTimeType() => Generator.GetDelegate(this).CreateType();
        public override string ToString() => "FunctionType: " + ReturnType.ToString() + ParamsList.ToString();
        
        public override bool Equals(object o)
        {
            FunctionType ft = o as FunctionType;
            if (Object.ReferenceEquals(ft, null))
                return false;
            if (ToString() != ft.ToString())
                return false;
            if(ReturnType.ToString() != ft.ReturnType.ToString())
                return false;
            return ParamsList.Equals(ft.ParamsList);
        }
        public override int GetHashCode() => ParamsList.ToString().GetHashCode() ^ ReturnType.ToString().GetHashCode();
    }

    public abstract class IterableType : FType
    {
        public FType Type {get; set;}
    }

    public class EllipsisType : IterableType
    {
        public EllipsisType() => Type = new IntegerType();
        public override Type GetRunTimeType() => typeof(FEllipsis);
        public override string ToString() => "EllipsisType";
    }
    public class ArrayType : IterableType
    {
        public ArrayType(FType type, TextSpan span = null)
        {
            Type = type;
            Span = span;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Array type");
            Type.Print(tabs + 1);
        }
        public override string ToString() => "ArrayType[" + Type.ToString() + "]";
        public override Type GetRunTimeType() => typeof(FArray<>).MakeGenericType(Type.GetRunTimeType());
    }
    public class MapType : FType
    {
        public FType Key {get; set;}
        public FType Value {get; set;}
        public MapType(FType key, FType value, TextSpan span = null)
        {
            Key = key;
            Value = value;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Map type");
            Key.Print(tabs + 1);
            Value.Print(tabs + 1);
        }
        public override string ToString() => "MapType{" + Key.ToString() + ": " + Value.ToString() + "}";
        public override Type GetRunTimeType() => typeof(FMap<,>).MakeGenericType(new Type[]{Key.GetRunTimeType(), Value.GetRunTimeType()});
    }
    public class TupleType : FType
    {
        public TypeList TypesList {get; set;}
        public Dictionary<string, int> Names {get; set;}
        public TupleType(TypeList types, TextSpan span = null)
        {
            Names = new Dictionary<string, int>();
            TypesList = types;
            Span = span;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Tuple type");
            TypesList.Print(tabs + 1);
        }

        public FType GetIndexType(DotIndexer index) => TypesList.Types[(index.Id != null ? Names[index.Id.Name] : index.Index.Value) - 1];

        public int GetMappedIndex(DotIndexer index)
        {
            if(index.Index != null)
                return index.Index.Value;
            else if(Names.ContainsKey(index.Id.Name))
                return Names[index.Id.Name];

            throw new FCompilationException($"{Span} - Tuple doesn't contain value with identifier '{index.Id.Name}'");
        }
        
        public override Type GetRunTimeType() => typeof(FTuple);

        public override string ToString()
        {
            string s = "TupleType(";
            foreach(FType type in TypesList.Types)
                s += type.ToString() + ", ";
            if(TypesList.Types.Count > 0) s = s.Remove(s.Length - 2);
            return s + ")";
        }

        public override bool Equals(object o)
        {
            TupleType ft = o as TupleType;
            if (Object.ReferenceEquals(ft, null))
                return false;
            if (ToString() != ft.ToString())
                return false;
            return TypesList.ToString() == ft.TypesList.ToString();
        }
        public override int GetHashCode() => TypesList.ToString().GetHashCode();
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
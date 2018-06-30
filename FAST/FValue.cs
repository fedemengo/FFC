using System.Collections.Generic;
using System;
using System.Reflection.Emit;
using FFC.FParser;
using FFC.FRunTime;
using FFC.FGen;

namespace FFC.FAST
{
    public abstract class FValue : FPrimary
    {
        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label)) => throw new NotImplementedException($"{Span} - Generation for {GetType().Name} not implemented");
    }
    public class BooleanValue : FValue
    {
        public bool Value {get; set;}

        public BooleanValue(bool value, TextSpan span)
        {
            ValueType = new BooleanType();
            Value = value;
            Span = span;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine($"Boolean value({Value})");
        }
        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            generator.Emit(Value ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
            generator.Emit(OpCodes.Newobj, typeof(FBoolean).GetConstructor(new Type[]{typeof(bool)}));
        }
    }
    public class IntegerValue : FValue
    {
        public int Value {get; set;}

        public IntegerValue(int value, TextSpan span)
        {
            ValueType = new IntegerType();
            Value = value;
            Span = span;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine($"Integer value({Value})");
        }

        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            generator.Emit(OpCodes.Ldc_I4_S, Value);
            generator.Emit(OpCodes.Newobj, typeof(FInteger).GetConstructor(new Type[]{typeof(int)}));
        }
    }
    public class RealValue : FValue
    {
        public double Value {get; set;}

        public RealValue(double value, TextSpan span)
        {
            ValueType = new RealType();
            Value = value;
            Span = span;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine($"RealVal value({Value})");
        }

        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            generator.Emit(OpCodes.Ldc_R8, Value);
            generator.Emit(OpCodes.Newobj, typeof(FReal).GetConstructor(new Type[]{typeof(double)}));
        }
    }
    public class RationalValue : FValue
    {
        public int Num {get; set;}
        public int Den {get; set;}

        public RationalValue(int numerator, int denominator, TextSpan span)
        {
            ValueType = new RationalType();
            Num = numerator;
            Den = denominator;
            Span = span;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine($"Rational value({Num} / { Den})");
        }

        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            generator.Emit(OpCodes.Ldc_I4, this.Num);
            generator.Emit(OpCodes.Ldc_I4, this.Den);
            generator.Emit(OpCodes.Newobj, typeof(FRational).GetConstructor(new Type[]{typeof(int), typeof(int)}));
        }
    }
    public class ComplexValue : FValue
    {
        public double Real {get; set;}
        public double Img {get; set;}

        public ComplexValue(double real, double img, TextSpan span)
        {
            ValueType = new ComplexType();
            Real = real;
            Img = img;
            Span = span;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine($"Complex value({Real}i{Img})");
        }
        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            generator.Emit(OpCodes.Ldc_R8, Real);
            generator.Emit(OpCodes.Ldc_R8, Img);
            generator.Emit(OpCodes.Newobj, typeof(FComplex).GetConstructor(new Type[]{typeof(double), typeof(double)}));
        }

    }
    public class StringValue : FValue
    {
        public string Value {get; set;}

        public StringValue(string value, TextSpan span)
        {
            ValueType = new StringType();
            Value = value;
            Span = span;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine($"String value({Value})");
        }

        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            generator.Emit(OpCodes.Ldstr, Value);
            generator.Emit(OpCodes.Newobj, typeof(FString).GetConstructor(new Type[]{typeof(string)}));
        }
    }
    public class Identifier : FValue
    {
        public string Name {get; set;}

        public Identifier(string name, TextSpan span)
        {
            this.Name = name;
            this.Span = span;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine($"Identifier({Name})");
        }

        public override void Generate(ILGenerator generator, TypeBuilder currentType, SymbolTable st, Label exitLabel = default(Label), Label conditionLabel = default(Label))
        {
            if(st.Contains(Name))
                Generator.EmitLoad(generator, st.Find(Name).Builder);
            else
                throw new NotImplementedException($"{Span} - Identifier {Name} not found");
        }

        public override void BuildValueType(SymbolTable st)
        {
            NameInfo entry = st.Find(Name);
            if(entry == null) throw new NotImplementedException($"{Span} - Couldn't resolve name \"{Name}\"");
            ValueType = st.Find(Name).Type;
        }
    }

    public class IdentifierList : FValue
    {
        public List<Identifier> IdList {get; set;}

        public IdentifierList(Identifier id, TextSpan span)
        {
            IdList = new List<Identifier>{id};
            Span = span;
        }

        public void Add(Identifier id) => IdList.Add(id);
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Identifier list");
            foreach(Identifier id in IdList)
                id.Print(tabs+1);
        }
    }
}
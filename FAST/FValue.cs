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
        public override void Generate(ILGenerator generator, SymbolTable st)
        {
            throw new NotImplementedException($"{Span} - Generation for {GetType().Name} not implemented");
        }
    }
    public class BooleanValue : FValue
    {
        public bool value;

        public BooleanValue(bool value, TextSpan span)
        {
            this.Span = span;
            this.value = value;
            valueType = new BooleanType();
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine($"Boolean value({value})");
        }
        public override void Generate(ILGenerator generator, SymbolTable st)
        {
            generator.Emit(value ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
            generator.Emit(OpCodes.Newobj, typeof(FBoolean).GetConstructor(new Type[]{typeof(bool)}));
        }

    }
    public class IntegerValue : FValue
    {
        public int value;

        public IntegerValue(int value, TextSpan span)
        {
            this.Span = span;
            this.value = value;
            valueType = new IntegerType();
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine($"Integer value({value})");
        }

        public override void Generate(ILGenerator generator, SymbolTable st)
        {
            generator.Emit(OpCodes.Ldc_I4_S, value);
            generator.Emit(OpCodes.Newobj, typeof(FInteger).GetConstructor(new Type[]{typeof(int)}));
        }
    }
    public class RealValue : FValue
    {
        public double value;

        public RealValue(double value, TextSpan span)
        {
            this.Span = span;
            this.value = value;
            valueType = new RealType();
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine($"RealVal value({value})");
        }

        public override void Generate(ILGenerator generator, SymbolTable st)
        {
            generator.Emit(OpCodes.Ldc_R8, value);
            generator.Emit(OpCodes.Newobj, typeof(FReal).GetConstructor(new Type[]{typeof(double)}));
        }
    }
    public class RationalValue : FValue
    {
        public int numerator;
        public int denominator;

        public RationalValue(int numerator, int denominator, TextSpan span)
        {
            this.Span = span;
            this.numerator = numerator;
            this.denominator = denominator;
            valueType = new RationalType();
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine($"Rational value({numerator} / { denominator})");
        }

        public override void Generate(ILGenerator generator, SymbolTable st)
        {
            generator.Emit(OpCodes.Ldc_I4, this.numerator);
            generator.Emit(OpCodes.Ldc_I4, this.denominator);
            generator.Emit(OpCodes.Newobj, typeof(FRational).GetConstructor(new Type[]{typeof(int), typeof(int)}));
        }
    }
    public class ComplexValue : FValue
    {
        public double real;
        public double img;

        public ComplexValue(double real, double img, TextSpan span)
        {
            this.Span = span;
            this.real = real;
            this.img = img;
            valueType = new ComplexType();
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine($"Complex value({real}i{img})");
        }
        public override void Generate(ILGenerator generator, SymbolTable st)
        {
            generator.Emit(OpCodes.Ldc_R8, real);
            generator.Emit(OpCodes.Ldc_R8, img);
            generator.Emit(OpCodes.Newobj, typeof(FComplex).GetConstructor(new Type[]{typeof(double), typeof(double)}));
        }

    }
    public class StringValue : FValue
    {
        public string value;

        public StringValue(string value, TextSpan span)
        {
            this.Span = span;
            this.value = value;
            valueType = new StringType();
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine($"String value({value})");
        }

        public override void Generate(ILGenerator generator, SymbolTable st)
        {
            generator.Emit(OpCodes.Ldstr, value);
            generator.Emit(OpCodes.Newobj, typeof(FString).GetConstructor(new Type[]{typeof(string)}));
        }
    }
    public class Identifier : FValue
    {
        public string name;

        public Identifier(string name, TextSpan span)
        {
            this.Span = span;
            this.name = name;
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine($"Identifier({name})");
        }

        public override void Generate(ILGenerator generator, SymbolTable st)
        {
            if(st.Contains(name))
                generator.Emit(OpCodes.Ldloc, st.Find(name).LocBuilder);
            else
                throw new NotImplementedException($"{Span} - Identifier {name} not found");
        }

        public override void BuildType(SymbolTable st)
        {
            valueType = st.Find(name).Type;
        }
    }

    public class IdentifierList : FValue
    {
        public List<Identifier> ids;

        public IdentifierList(Identifier id, TextSpan span)
        {
            this.Span = span;
            this.ids = new List<Identifier>{id};
        }

        public void Add(Identifier id)
        {
            this.ids.Add(id);
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine("Identifier list");
            foreach(Identifier id in ids)
            {
                id.Print(tabs+1);
            }
        }
    }
}
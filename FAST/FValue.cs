using System.Collections.Generic;
using System;
using System.Reflection.Emit;
using FFC.FParser;
using FFC.FRunTime;

namespace FFC.FAST
{
    abstract class FValue : FPrimary
    {

        public override void Generate(ILGenerator generator)
        {
            throw new NotImplementedException($"{Span} - Generation for {GetType().Name} not implemented");
        }
    }
    class BooleanValue : FValue
    {
        public bool value;

        public BooleanValue(bool value, TextSpan span)
        {
            this.Span = span;
            this.value = value;
            ValueType = new BooleanType();
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine($"Boolean value({value})");
        }
    }
    class IntegerValue : FValue
    {
        public int value;

        public IntegerValue(int value, TextSpan span)
        {
            this.Span = span;
            this.value = value;
            ValueType = new IntegerType();
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine($"Integer value({value})");
        }

        public override void Generate(ILGenerator generator)
        {
            generator.Emit(OpCodes.Ldc_I4_S, value);
            generator.Emit(OpCodes.Newobj, typeof(FInteger).GetConstructor(new Type[]{typeof(int)}));
        }
    }
    class RealValue : FValue
    {
        public double value;

        public RealValue(double value, TextSpan span)
        {
            this.Span = span;
            this.value = value;
            ValueType = new RealType();
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine($"RealVal value({value})");
        }

        public override void Generate(ILGenerator generator)
        {
            generator.Emit(OpCodes.Ldc_R8, value);
        }
    }
    class RationalValue : FValue
    {
        public int numerator;
        public int denominator;

        public RationalValue(int numerator, int denominator, TextSpan span)
        {
            this.Span = span;
            this.numerator = numerator;
            this.denominator = denominator;
            ValueType = new RationalType();
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine($"Rational value({numerator} / { denominator})");
        }
    }
    class ComplexValue : FValue
    {
        public double real;
        public double img;

        public ComplexValue(double real, double img, TextSpan span)
        {
            this.Span = span;
            this.real = real;
            this.img = img;
            ValueType = new ComplexType();
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine($"Complex value({real}i{img})");
        }
    }
    class StringValue : FValue
    {
        public string value;

        public StringValue(string value, TextSpan span)
        {
            this.Span = span;
            this.value = value;
            ValueType = new StringType();
        }
        public override void Print(int tabs)
        {
            PrintTabs(tabs);
            Console.WriteLine($"String value({value})");
        }

        public override void Generate(ILGenerator generator)
        {
            generator.Emit(OpCodes.Ldstr, value);
        }
    }
    class Identifier : FValue
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

    }

    class IdentifierList : FValue
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
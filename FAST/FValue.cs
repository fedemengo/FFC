using System.Collections.Generic;
namespace FFC.FAST
{
    abstract class FValue : FPrimary
    {
    }
    class BooleanValue : FValue
    {
        public bool value;

        public BooleanValue(bool value)
        {
            this.value = value;
        }
    }
    class IntegerValue : FValue
    {
        public int value;

        public IntegerValue(int value)
        {
            this.value = value;
        }
    }
    class RealValue : FValue
    {
        public double value;

        public RealValue(double value)
        {
            this.value = value;
        }
    }
    class RationalValue : FValue
    {
        public int numerator;
        public int denominator;

        public RationalValue(int numerator, int denominator)
        {
            this.numerator = numerator;
            this.denominator = denominator;
        }
    }
    class ComplexValue : FValue
    {
        public double real;
        public double img;

        public ComplexValue(double real, double img)
        {
            this.real = real;
            this.img = img;
        }
    }
    class StringValue : FValue
    {
        public string value;

        public StringValue(string value)
        {
            this.value = value;
        }
    }
    class Identifier : FValue
    {
        public string name;

        public Identifier(string name)
        {
            this.name = name;
        }
    }
}
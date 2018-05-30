using System.Collections.Generic;
namespace FAST
{
    abstract class FValue : FPrimary
    {
    }
    class BooleanValue : FValue
    {
        public bool value;
    }
    class IntegerValue : FValue
    {
        public int value;
    }
    class RealValue : FValue
    {
        public double value;
    }
    class RationalValue : FValue
    {
        public int numerator;
        public int denominator;
    }
    class ComplexValue : FValue
    {
        public double real;
        public double img;
    }
    class StringValue : FValue
    {
        public string value;
    }
    class Identifier : FValue
    {
        public string name;
    }
}
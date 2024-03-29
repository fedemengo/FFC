using System;

namespace FFC.FRunTime
{
    public class FInteger : FRTType
    {
        public int Value;
        public FInteger(int val) => Value = val;

        public static FInteger operator+(FInteger i1, FInteger i2) => new FInteger(i1.Value + i2.Value);
        public static FInteger operator-(FInteger i1, FInteger i2) => new FInteger(i1.Value - i2.Value);
        public static FInteger operator*(FInteger i1, FInteger i2) => new FInteger(i1.Value * i2.Value);
        public static FReal operator/(FInteger i1, FInteger i2) => new FReal(i1.Value / (double) i2.Value);
        public static FInteger operator%(FInteger i1, FInteger i2) => new FInteger(i1.Value % i2.Value);

        public static FBoolean operator== (FInteger i1, FInteger i2) => new FBoolean(i1.Value == i2.Value); 
        public static FBoolean operator!= (FInteger i1, FInteger i2) => new FBoolean(i1.Value != i2.Value); 
        public static FBoolean operator< (FInteger i1, FInteger i2) => new FBoolean(i1.Value < i2.Value); 
        public static FBoolean operator<= (FInteger i1, FInteger i2) => new FBoolean(i1.Value <= i2.Value); 
        public static FBoolean operator> (FInteger i1, FInteger i2) => new FBoolean(i1.Value > i2.Value); 
        public static FBoolean operator>= (FInteger i1, FInteger i2) => new FBoolean(i1.Value >= i2.Value);

        public static FInteger operator-(FInteger i1) => new FInteger(- i1.Value);
        
        public override bool Equals(object obj)
        {
            FInteger fi = obj as FInteger;
            if (Object.ReferenceEquals(fi, null))
                return false;
            return Value == fi.Value;
        }

        public override int GetHashCode() => Value.GetHashCode();
    
        public override string ToString() => Value.ToString();

        //Right now, just take whole line and try to parse it.
        //A better idea to have clean c++ like input could be
        //to have some input buffer class to use that gets loaded
        //on Console.ReadLine()
        public static FInteger Read() => new FInteger(int.Parse(Console.ReadLine()));

        public void Assign(FInteger other) => Value = other.Value;
    }
}
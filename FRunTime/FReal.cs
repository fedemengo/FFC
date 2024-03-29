using System;

namespace FFC.FRunTime
{
    public class FReal : FRTType
    {
        public double Value;

        public FReal(double val) => Value = val;
        public FReal(FInteger i) => Value = i.Value;

        public static FReal operator+(FReal r1, FReal r2) => new FReal(r1.Value + r2.Value);
        public static FReal operator-(FReal r1, FReal r2) => new FReal(r1.Value - r2.Value);
        public static FReal operator*(FReal r1, FReal r2) => new FReal(r1.Value * r2.Value);
        public static FReal operator/(FReal r1, FReal r2) => new FReal(r1.Value / r2.Value);
        public static FReal operator%(FReal r1, FReal r2) => new FReal(r1.Value % r2.Value);

        public static FBoolean operator== (FReal r1, FReal r2) => new FBoolean(r1.Value == r2.Value); 
        public static FBoolean operator!= (FReal r1, FReal r2) => new FBoolean(r1.Value != r2.Value); 
        public static FBoolean operator< (FReal r1, FReal r2) => new FBoolean(r1.Value < r2.Value); 
        public static FBoolean operator<= (FReal r1, FReal r2) => new FBoolean(r1.Value <= r2.Value); 
        public static FBoolean operator> (FReal r1, FReal r2) => new FBoolean(r1.Value > r2.Value); 
        public static FBoolean operator>= (FReal r1, FReal r2) => new FBoolean(r1.Value >= r2.Value);

        public static FReal operator-(FReal r1) => new FReal(- r1.Value);

        public FInteger Round() => new FInteger((int) Math.Round(Value));
        
        public override bool Equals(object obj)
        {
            FReal freal = obj as FReal;
            if (Object.ReferenceEquals(freal, null))
                return false;
            return Value == freal.Value;
        }

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value.ToString();

        //Refer to FInteger Read function
        public static FReal Read() => new FReal(double.Parse(Console.ReadLine()));          
        public void Assign(FReal other) => Value = other.Value;

    }
}
using System;

namespace FFC.FRunTime
{
    public class FRational : FRTType
    {
        public FRational(int n, int d)
        {
            int gcd = GCD(n, d);
            Numerator = n / gcd;
            Denominator = d / gcd;
        }
        public FRational(FInteger i) : this(i.Value, 1){}
        public int Numerator {get; set;}
        public int Denominator {get; set;}
        private static int GCD(int a, int b) => b == 0 ? a : GCD(b, a % b);
        private static FRational Inverse(FRational r1) => new FRational(r1.Denominator, r1.Numerator);
        private FReal Value() => new FReal(Numerator / (double)Denominator);

        public static FRational operator +(FRational r1, FRational r2)
        {
            int gcd = GCD(r1.Denominator, r2.Denominator);
		    int den = r1.Denominator / gcd * r2.Denominator;
		    return new FRational(r2.Denominator / gcd * r1.Numerator + r1.Denominator / gcd * r2.Numerator, den);
        }

        public static FRational operator -(FRational r1, FRational r2)
        {
            //shall we write this as r1 + (-r2) ?
            int gcd = GCD(r1.Denominator, r2.Denominator);
		    int den = r1.Denominator / gcd * r2.Denominator;
		    return new FRational(r2.Denominator / gcd * r1.Numerator - r1.Denominator / gcd * r2.Numerator, den);
        }

        public static FRational operator *(FRational r1, FRational r2)
        {
            //not fancy written but avoids overflow as much as possible
            int gcd1 = GCD(r1.Numerator, r2.Denominator);
            int gcd2 = GCD(r1.Denominator, r2.Numerator);
            return new FRational(r1.Numerator / gcd1 * (r2.Numerator / gcd2), r1.Denominator / gcd2 * (r2.Denominator / gcd1));
        }

        public static FRational operator /(FRational r1, FRational r2) => r1 * Inverse(r2);

        public static FBoolean operator ==(FRational r1, FRational r2) => new FBoolean(r1.Numerator == r2.Numerator && r1.Denominator == r2.Denominator); 
        public static FBoolean operator !=(FRational r1, FRational r2) => new FBoolean(r1.Numerator != r2.Numerator || r1.Denominator != r2.Denominator); 
        public static FBoolean operator <(FRational r1, FRational r2) => r1.Value() < r2.Value();
        public static FBoolean operator <=(FRational r1, FRational r2) => r1.Value() <= r2.Value();
        public static FBoolean operator >(FRational r1, FRational r2) => r1.Value() > r2.Value();
        public static FBoolean operator >=(FRational r1, FRational r2) => r1.Value() >= r2.Value();
    
        public static FRational operator-(FRational r1) => new FRational(-r1.Numerator, r1.Denominator);

        public override string ToString() => Numerator.ToString() + "/" + Denominator.ToString();
        
        //Refer to FInteger Read function
        public static FRational Read()
        {
            string l = Console.ReadLine();
            var p = l.Split('\\');
            if(p.Length != 2) throw new FormatException("Input is not long enough to be a rational value");
            return new FRational(int.Parse(p[0]), int.Parse(p[1]));
        }
    }
}
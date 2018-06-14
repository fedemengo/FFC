using System;
using System.Collections.Generic;

namespace FFC.FRunTime
{
    class FRational
    {
        public FRational(int n, int d)
        {
            int gcd = GCD(Numerator, Denominator);
            Numerator = n / gcd;
            Denominator = d / gcd;
        }
        public int Numerator {get; set;}
        public int Denominator {get; set;}

        private static int GCD(int a, int b)
        {
            if(a == 0) return b;
            return GCD(b % a, b);
        }
        
        private static FRational Inverse(FRational r1) => new FRational(r1.Denominator, r1.Numerator);

        private FReal Value() => new FReal(Numerator / (double)Denominator);

        public override string ToString() => Numerator.ToString() + "/" + Denominator.ToString();
        
        public static FRational operator +(FRational r1, FRational r2)
        {
            int gcd = GCD(r1.Denominator, r2.Denominator);
		    int den = r1.Denominator / gcd * r2.Denominator;
		    return new FRational(r1.Numerator * r2.Denominator + r2.Numerator * r1.Denominator, den);
        }

        public static FRational operator -(FRational r1, FRational r2)
        {
            int gcd = GCD(r1.Denominator, r2.Denominator);
		    int den = r1.Denominator / gcd * r2.Denominator;
		    return new FRational(r1.Numerator * r2.Denominator - r2.Numerator * r1.Denominator, den);
        }

        public static FRational operator *(FRational r1, FRational r2)
        {
            return new FRational(r1.Numerator * r2.Numerator, r1.Denominator * r2.Denominator);
        }

        public static FRational operator /(FRational r1, FRational r2)
        {
            return r1 * Inverse(r2);
        }

        public static FBoolean operator ==(FRational r1, FRational r2) => new FBoolean(r1.Numerator == r2.Numerator && r1.Denominator == r2.Denominator); 
        public static FBoolean operator !=(FRational r1, FRational r2) => new FBoolean(r1.Numerator != r2.Numerator || r1.Denominator != r2.Denominator); 
        public static FBoolean operator <(FRational r1, FRational r2) => new FBoolean(r1.Value() < r2.Value());
        public static FBoolean operator <=(FRational r1, FRational r2) => new FBoolean(r1.Value() <= r2.Value());
        public static FBoolean operator >(FRational r1, FRational r2) => new FBoolean(r1.Value() > r2.Value());
        public static FBoolean operator >=(FRational r1, FRational r2) => new FBoolean(r1.Value() >= r2.Value());
    }
}
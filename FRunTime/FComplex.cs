using System;
using System.Collections.Generic;


namespace FFC.FRunTime
{
    public class FComplex : FRTType
    {
        public FComplex(double r, double i)
        {
            Real = r;
            Imaginary = i;
        }
        public double Real {get; set;}
        public double Imaginary {get; set;}
        public FComplex Conjugate() => new FComplex(Real, -Imaginary);

        public static FComplex operator +(FComplex c1, FComplex c2) => new FComplex(c1.Real + c2.Real, c1.Imaginary + c2.Imaginary);
        
        public static FComplex operator -(FComplex c1, FComplex c2) => new FComplex(c1.Real - c2.Real, c1.Imaginary - c2.Imaginary);

        public static FComplex operator *(FComplex c1, FComplex c2)
        {
            return new FComplex(c1.Real * c2.Real - c1.Imaginary * c2.Imaginary, c1.Imaginary * c2.Real + c1.Real * c2.Imaginary);
        }
        public static FComplex operator /(FComplex c1, FComplex c2)
        {
            // aib / cid -> we multiply both by den conjugate, so we get rid of j below and we can solve
            // maybe can be shortened, not sure
            FComplex num = c1 * c2.Conjugate();
            double den = c2.Real * c2.Real + c2.Imaginary * c2.Imaginary;
            return new FComplex(num.Real / den, num.Imaginary / den);
        }
        public double Modulo() => Math.Sqrt(Real * Real + Imaginary * Imaginary);

        public static bool operator == (FComplex c1, FComplex c2) => c1.Real == c2.Real && c1.Imaginary == c2.Imaginary; 
        public static bool operator != (FComplex c1, FComplex c2) => c1.Real != c2.Real || c1.Imaginary != c2.Imaginary; 
        public static bool operator < (FComplex c1, FComplex c2) => c1.Modulo() < c2.Modulo();
        public static bool operator <= (FComplex c1, FComplex c2) => c1.Modulo() <= c2.Modulo();
        public static bool operator > (FComplex c1, FComplex c2) => c1.Modulo() > c2.Modulo();
        public static bool operator >= (FComplex c1, FComplex c2) => c1.Modulo() >= c2.Modulo();

        public static FComplex operator -(FComplex c1) => new FComplex(-c1.Real, c1.Imaginary);

        public override string ToString() => Real.ToString() + "i" + Imaginary.ToString();
    }
}
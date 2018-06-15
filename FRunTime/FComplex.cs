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
        public override string ToString()
        {
            return Real.ToString() + "i" + Imaginary.ToString();
        }
        public FComplex Conjugate()
        {
            return new FComplex(Real, -Imaginary);
        }
        public static FComplex operator +(FComplex c1, FComplex c2)
        {
            return new FComplex(c1.Real + c2.Real, c1.Imaginary + c2.Imaginary);
        }
        public static FComplex operator -(FComplex c1, FComplex c2)
        {
            return new FComplex(c1.Real - c2.Real, c1.Imaginary - c2.Imaginary);
        }
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
            FComplex ans = new FComplex(num.Real / den, num.Imaginary / den);
            return ans;
        }
        public double Modulo()
        {
            return Math.Sqrt(Real * Real + Imaginary * Imaginary);
        }
        public static bool operator == (FComplex c1, FComplex c2) => c1.Real == c2.Real && c1.Imaginary == c2.Imaginary; 
        public static bool operator != (FComplex c1, FComplex c2) => c1.Real != c2.Real || c1.Imaginary != c2.Imaginary; 
        public static bool operator < (FComplex c1, FComplex c2) => c1.Modulo() < c2.Modulo();
        public static bool operator <= (FComplex c1, FComplex c2) => c1.Modulo() <= c2.Modulo();
        public static bool operator > (FComplex c1, FComplex c2) => c1.Modulo() > c2.Modulo();
        public static bool operator >= (FComplex c1, FComplex c2) => c1.Modulo() >= c2.Modulo();
    }
}
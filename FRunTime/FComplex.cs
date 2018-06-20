using System;
namespace FFC.FRunTime
{
    public class FComplex : FRTType
    {
        public FComplex(double r, double i)
        {
            Real = r;
            Imaginary = i;
        }
        public FComplex(FReal r) : this(r.Value, 0){}
        public FComplex(FInteger i) : this(i.Value, 0){}
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
        
        //Refer to FInteger Read function
        //I no longer like that we need to have REAL i REAL, I think we should also support INT i INT
        public static FComplex Read()
        {
            string l = Console.ReadLine();
            var p = l.Split('i');
            if(p.Length != 2) throw new Exception();
            if(p[0].Length == 0 || p[0][p[0].Length-1] == '.' || p[0][0] == '.')
                throw new Exception("Wrong complex format");
            if(p[1].Length == 0 || p[1][p[1].Length-1] == '.' || p[1][0] == '.')
                throw new Exception("Wrong complex format");
            return new FComplex(double.Parse(p[0]), double.Parse(p[1]));
        }
    }
}
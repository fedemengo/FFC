using System;
namespace FFC.FRunTime
{
    public class FComplex : FRTType
    {
        private static int PRIME1 = 1019;
        private static int PRIME2 = 2050918644;
        public FComplex(double r, double i)
        {
            Real = r;
            Imaginary = i;
        }
        public FComplex(FReal r) : this(r.Value, 0){}
        public FComplex(FInteger i) : this(i.Value, 0){}
        public double Real;
        public double Imaginary;
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

        public static FBoolean operator == (FComplex c1, FComplex c2) => new FBoolean(c1.Real == c2.Real && c1.Imaginary == c2.Imaginary);
        public static FBoolean operator != (FComplex c1, FComplex c2) => new FBoolean(c1.Real != c2.Real || c1.Imaginary != c2.Imaginary);
        public static FBoolean operator < (FComplex c1, FComplex c2) => new FBoolean(c1.Modulo() < c2.Modulo());
        public static FBoolean operator <= (FComplex c1, FComplex c2) => new FBoolean(c1.Modulo() <= c2.Modulo());
        public static FBoolean operator > (FComplex c1, FComplex c2) => new FBoolean(c1.Modulo() > c2.Modulo());
        public static FBoolean operator >= (FComplex c1, FComplex c2) => new FBoolean(c1.Modulo() >= c2.Modulo());

        public static FComplex operator -(FComplex c1) => new FComplex(-c1.Real, c1.Imaginary);

        public override string ToString() => Real.ToString() + "i" + Imaginary.ToString();

        public override bool Equals(object obj)
        {
            FComplex fc = obj as FComplex;
            if (Object.ReferenceEquals(fc, null))
                return false;
            return Real == fc.Real && Imaginary == fc.Imaginary;
        }

        public override int GetHashCode() => ((PRIME1 * Real.GetHashCode() + Imaginary.GetHashCode()) % PRIME2);

        public static FComplex Compl(object r, object i)
        {
            double real, img;
            if(r is FInteger)
                real = (r as FInteger).Value;
            else
                real = (r as FReal).Value;
            
            if(i is FInteger)
                img = (i as FInteger).Value;
            else
                img = (i as FReal).Value;
            
            return new FComplex(real, img);
        }
        //Refer to FInteger Read function
        //I no longer like that we need to have REAL i REAL, I think we should also support INT i INT
        public static FComplex Read()
        {
            string l = Console.ReadLine();
            var p = l.Split('i');
            if(p.Length != 2) throw new FormatException("Input is not long enough to be a complex value");
            if(p[0].Length == 0 || p[0][p[0].Length-1] == '.' || p[0][0] == '.')
                throw new FormatException("Wrong complex format, should be NUMiNUM");
            if(p[1].Length == 0 || p[1][p[1].Length-1] == '.' || p[1][0] == '.')
                throw new FormatException("Wrong complex format, should be NUMiNUM");
            return new FComplex(double.Parse(p[0]), double.Parse(p[1]));
        }
        public void Assign(FComplex other)
        {
            Imaginary = other.Imaginary;
            Real = other.Real;
        }

    }
}

namespace FFC.FRunTime
{
    public class FReal
    {
        public double Value {get; set;}

        public FReal(double val) => Value = val;

        public static FReal operator+(FReal r1, FReal r2) => new FReal(r1.Value + r2.Value);
        public static FReal operator-(FReal r1, FReal r2) => new FReal(r1.Value - r2.Value);
        public static FReal operator*(FReal r1, FReal r2) => new FReal(r1.Value * r2.Value);
        public static FReal operator/(FReal r1, FReal r2) => new FReal(r1.Value / r2.Value);
        public static FReal operator%(FReal r1, FReal r2) => new FReal(r1.Value % r2.Value);

        public static bool operator== (FReal r1, FReal r2) => r1.Value == r2.Value; 
        public static bool operator!= (FReal r1, FReal r2) => r1.Value != r2.Value; 
        public static bool operator< (FReal r1, FReal r2) => r1.Value < r2.Value; 
        public static bool operator<= (FReal r1, FReal r2) => r1.Value <= r2.Value; 
        public static bool operator> (FReal r1, FReal r2) => r1.Value > r2.Value; 
        public static bool operator>= (FReal r1, FReal r2) => r1.Value >= r2.Value;

        public static FReal operator-(FReal r1) => new FReal(- r1.Value);
        public override string ToString() => Value.ToString();
    }
}
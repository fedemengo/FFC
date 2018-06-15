namespace FFC.FRunTime
{
    public class FInteger : FRTType
    {
        public int Value {get; set;}
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
        
        public override string ToString() => Value.ToString();
    }
}
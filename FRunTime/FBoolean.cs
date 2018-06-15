namespace FFC.FRunTime
{
    public class FBoolean : FRTType
    {
        public bool Value {get; set;}
        public FBoolean(bool val)
        {
            Value = val;
        }

        public static FBoolean operator&(FBoolean b1, FBoolean b2) => new FBoolean(b1.Value & b2.Value);
        public static FBoolean operator|(FBoolean b1, FBoolean b2) => new FBoolean(b1.Value | b2.Value);
        public static FBoolean operator^(FBoolean b1, FBoolean b2) => new FBoolean(b1.Value ^ b2.Value);

        public static FBoolean operator== (FBoolean b1, FBoolean r2) => new FBoolean(b1.Value == r2.Value); 
        public static FBoolean operator!= (FBoolean b1, FBoolean r2) => new FBoolean(b1.Value != r2.Value); 

        public static FBoolean operator!(FBoolean b1) => new FBoolean(!b1.Value);
        
        public override string ToString() => Value.ToString();
    }
}
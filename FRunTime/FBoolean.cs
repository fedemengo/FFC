using System;

namespace FFC.FRunTime
{
    public class FBoolean : FRTType
    {
        public bool Value;
        public FBoolean(bool val) => Value = val;

        public static FBoolean operator&(FBoolean b1, FBoolean b2) => new FBoolean(b1.Value & b2.Value);
        public static FBoolean operator|(FBoolean b1, FBoolean b2) => new FBoolean(b1.Value | b2.Value);
        public static FBoolean operator^(FBoolean b1, FBoolean b2) => new FBoolean(b1.Value ^ b2.Value);

        public static FBoolean operator== (FBoolean b1, FBoolean r2) => new FBoolean(b1.Value == r2.Value); 
        public static FBoolean operator!= (FBoolean b1, FBoolean r2) => new FBoolean(b1.Value != r2.Value); 

        public static FBoolean operator!(FBoolean b1) => new FBoolean(!b1.Value);
        
        public override bool Equals(object obj)
        {
            FBoolean fb = obj as FBoolean;
            if (Object.ReferenceEquals(fb, null))
                return false;
            return Value == fb.Value;
        }

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value == true ? "true" : "false";
        public bool GetBool() => Value;

        //Refer to FInteger Read function
        public static FBoolean Read()
        {
            string l = Console.ReadLine();
            if(l == "true") return new FBoolean(true);
            if(l == "false") return new FBoolean(false);
            throw new FormatException($"\"{l}\" is not a valid boolean value (true, false)");
        }

        public void Assign(FBoolean other) => Value = other.Value;

    }
}
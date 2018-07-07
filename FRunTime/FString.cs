using System;
using System.Collections.Generic;

namespace FFC.FRunTime
{
    public class FString : FRTType
    {
        public string Value;
        public FString(string s) => Value = s;

        public override string ToString() => Value;
        //Refer to FInteger Read function
        public static FString Read() => new FString(Console.ReadLine());
        //Immutable concatenation
        public static FString operator+(FString a, FString b) => new FString(String.Concat(a, b));  
        public static FBoolean operator ==(FString a, FString b) => new FBoolean(a.Value == b.Value);  
        public static FBoolean operator !=(FString a, FString b) => new FBoolean(a.Value != b.Value);

        public override bool Equals(object obj)
        {
            FString fs = obj as FString;
            if (Object.ReferenceEquals(fs, null))
                return false;
            return Value == fs.Value;
        }

        public override int GetHashCode() => Value.GetHashCode(); 
        public void Assign(FString other) => Value = other.Value;


    }
}
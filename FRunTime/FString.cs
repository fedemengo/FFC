using System;
using System.Collections.Generic;

namespace FFC.FRunTime
{
    public class FString : FRTType
    {
        public string Text;
        public FString(string s) => Text = s;

        public override string ToString() => Text;
        //Refer to FInteger Read function
        public static FString Read() => new FString(Console.ReadLine());
        //Immutable concatenation
        public static FString operator+(FString a, FString b) => new FString(String.Concat(a, b));  
        public static FBoolean operator ==(FString a, FString b) => new FBoolean(a.Text == b.Text);  
        public static FBoolean operator !=(FString a, FString b) => new FBoolean(a.Text != b.Text);

        public override bool Equals(object obj)
        {
            FString fs = obj as FString;
            if (Object.ReferenceEquals(fs, null))
                return false;
            return Text == fs.Text;
        }

        public override int GetHashCode() => Text.GetHashCode(); 

    }
}
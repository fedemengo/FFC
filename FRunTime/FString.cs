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

    }
}
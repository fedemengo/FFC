using System;
using System.Collections.Generic;

namespace FFC.FRunTime
{
    public class FString : FRTType
    {
        public string Text {get; set;}
        public FString(string s)
        {
            Text = s;
        }

        public override string ToString()
        {
            return Text;
        }
        //Refer to FInteger Read function
        public static FString Read() => new FString(Console.ReadLine());          

    }
}
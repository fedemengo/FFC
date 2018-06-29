using System;
using System.Collections.Generic;

namespace FFC.FRunTime
{
    public class FTuple : FRTType
    {
        List<object> elements;

        public FTuple()
        {
            elements = new List<object>();
        }
        public void Add(object e)
        {
            if(!(e is FRTType || e is Delegate))
                throw new NotImplementedException($"Cannot create tuple element of type {e.GetType().Name}");  
            elements.Add(e);
        }
        public object Get(FInteger index) => elements[index.Value - 1];

        public override string ToString()
        {
            string ans = "{";
            foreach(object elem in elements)
                ans += elem.ToString() + ", ";
            if(ans.Length > 4) 
                ans = ans.Remove(ans.Length-2);
            return ans + "}";
        }
    }
}
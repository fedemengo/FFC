using System;
using System.Collections.Generic;

namespace FFC.FRunTime
{
    public class FTuple : FRTType
    {
        List<object> Elements;
        public FTuple() => Elements = new List<object>();
        public void Add(object e)
        {
            if(!(e is FRTType || e is Delegate))
                throw new ArrayTypeMismatchException($"Cannot create tuple element of type {e.GetType().Name}");  
            Elements.Add(e);
        }
        public object Get(FInteger index) => Elements[index.Value - 1];
        public void Set(FInteger index, object obj) => Elements[index.Value - 1] = obj;
        public override string ToString()
        {
            string ans = "{";
            foreach(object elem in Elements)
                ans += elem.ToString() + ", ";
            if(ans.Length > 4) 
                ans = ans.Remove(ans.Length-2);
            return ans + "}";
        }
        public void Assign(FTuple other) => Elements = other.Elements;

    }
}
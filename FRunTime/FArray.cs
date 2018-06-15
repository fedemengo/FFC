using System;
using System.Collections.Generic;

namespace FFC.FRunTime
{
    public class FArray<V> : FRTType where V: FRTType
    {
        public List<V> Values {get; set;}
        public FArray()
        {
            Values = new List<V>{};
        }
        public FArray(FArray<V> l, V v)
        {
            Values = l.Values;
            Values.Add(v);
        }
        public override string ToString()
        {
            string ans = "{";
            foreach(V v in Values)
                ans += v.ToString() + ", ";
            //remove last ", "
            ans = ans.Remove(ans.Length - 2);
            ans += "}";
            return ans;
        }
    }
}
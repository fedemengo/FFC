using System;
using System.Collections.Generic;

namespace FFC.FRunTime
{
    public class FArray<V> : FRTType where V: FRTType
    {
        public List<V> Values {get; set;}
        public FArray() => Values = new List<V>{};
        public FArray(V v) => Values = new List<V>{v};

        //even if we create N FArray to create a (non immutable) FArray of size N,
        //GC should save us from wasted memory
        public FArray(FArray<V> a, V v)
        {
            Values = a.Values;
            Values.Add(v);
        }
        //non persistent concatenation
        public FArray(FArray<V> a1, FArray<V> a2)
        {
            //we should handle empty arrays here too
            Values = a1.Values;
            //not even remotely efficient
            foreach(V v in a2.Values)
                Values.Add(v);
        }
        public static FArray<V> operator+(FArray<V> a1, FArray<V> a2) => new FArray<V>(a1, a2);
        public static FArray<V> operator+(FArray<V> a, V v) => new FArray<V>(a, v);
        
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
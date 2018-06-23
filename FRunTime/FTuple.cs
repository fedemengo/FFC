using System.Collections.Generic;

namespace FFC.FRunTime
{
    public class FTuple : FRTType
    {
        List<FRTType> elements;

        public FTuple()
        {
            elements = new List<FRTType>();
        }
        public void Add(FRTType e)
        {
            elements.Add(e);
        }
        public FRTType Get(FInteger index) => elements[index.Value - 1];

        public override string ToString()
        {
            string ans = "{";
            foreach(FRTType elem in elements)
                ans += elem.ToString() + ", ";
            if(ans.Length > 4) 
                ans = ans.Remove(ans.Length-2);
            return ans + "}";
        }
    }
}
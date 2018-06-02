using System.Collections.Generic;
using FFC.FParser;
namespace FFC.FLexer
{
    class TokenValue : TValue
    {
        //wraps around list
        private List<object> list = new List<object>();
        public void Add(object o)
        {
            list.Add(o);
        }
        public int Count
        {
            get => list.Count;
        }
        public object this[int i]
        {
            get => list[i];
            set => list[i] = value;
        }

    }
    class Token
    {
        public ETokens type{get; set;}
        public TokenValue values = new TokenValue();
        //Position is in a [L, R) format
        public QUT.Gppg.LexLocation span;
        public Token(ETokens tokenType, Position begin, Position end)
        {
            this.type = tokenType;
            this.span = new QUT.Gppg.LexLocation(begin.Row, begin.Column, end.Row, end.Column);
        }

        public Token(ETokens tokenType, List<object> objs, Position begin, Position end) : this(tokenType, begin, end)
        {
            foreach(object o in objs)
                values.Add(o);
        }

        override public string ToString()
        {
            string ans = type.ToString();
            for(int i = 0; i < values.Count; i++)
            {
                if(i == 0)
                    ans += "(";
                else
                    ans += ", ";
                ans += values[i].ToString();
                if(i == values.Count - 1)
                    ans += ")";
            }
            ans += " " + span.ToString();
            return ans;
        }
    }
}
using System;
using System.Collections.Generic;

namespace testLex
{
    public class LexTrie
    {
        private LexNode root = new LexNode();

        public void Add(string s, string tk)
        {
            LexNode current = root;
            foreach (char c in s)
            {
                if(current.next.ContainsKey(c) == false)
                {
                    current.next.Add(c, new LexNode());
                }
                current = current.next[c];
            }
            current.token = tk;
        }

        public List<string> Parse(string text)
        {
            LexNode current = root;
            List<string> answer = new List<string>();
            foreach(char c in text)
            {
                if(current.next.ContainsKey(c))
                {
                    current = current.next[c];
                }
                else
                {
                    if(current.token != "")
                    {
                        answer.Add(current.token);
                    }
                    current = root;
                }
            }
            return answer;
        }

        public void Add(Regex r, string tk)
        {
            List<LexNode> regexNodes = new List<LexNode>();
            foreach(RegexItem ri in r)
                regexNodes.Add(new LexNode());
            
            for(int i = 0; i < regexNodes.size(); i++)
            {
                if(i + 1 < regexNodes.size())
                    foreach(char c in r[i])
                        regexNodes[i].next.Add(c, regexNodes[i+1]);
                if(r[i].isStar)
                    for(char c in r[i])
                        
            }
        }
    }
}
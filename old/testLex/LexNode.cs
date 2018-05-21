using System;
using System.Collections.Generic;

namespace testLex
{
    public class LexNode
    {
        public Dictionary<char, LexNode> next = new Dictionary<char, LexNode>();
        public string token = "";
    }
}
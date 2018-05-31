using System;

namespace FFC.FLexer
{
    class Position
    {
        public int Row {get; set;}
        public int Column{get; set;}
        public void NextChar()
        {
            Column++;
        }
        public void NextLine()
        {
            Row++;
            Column = 0;
        }
        public Tuple<int, int> GetPair()
        {
            return Tuple.Create(Row, Column);
        }
        public Position()
        {
            Row = Column = 0;
        }
        public Position(Position copy)
        {
            Row = copy.Row;
            Column = copy.Column;
        }
        public override string ToString()
        {
            return "[" + Row + ", " + Column + "]";
        }
    }
}

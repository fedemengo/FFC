using System;
using System.IO;
using FFC.FGen;

namespace FFC.FLexer
{
    class SourceReader
    {
        private string[] buffer;
        private bool isFinished;
        private Position position;
        public Position GetPosition()
        {
            return new Position(position);
        }
        public SourceReader(string filePath)
        {
            this.buffer = File.ReadAllLines(filePath);
            this.isFinished = false;
            this.position = new Position();
        }
        public void SkipLine()
        {
            char x = GetChar();
            if(x == '\n' || x == '\0')
            {
                Advance();
                return;
            }
            else
            {
                Advance();
                SkipLine();
            }
        }

        public bool Empty()
        {
            return isFinished;
        }
        public void SkipBlank()
        {
            char x = GetChar();
            if(x == ' ' || x == '\n' || x == '\t' || x == '\0')
            {
                Advance();
                return;
            }
            else
            {
                Advance();
                SkipBlank();
            }
        }
        public char GetChar()
        {
            if(this.isFinished)
                return '\0';
            if(this.position.Column == this.buffer[this.position.Row].Length)
                return '\n';
            return this.buffer[this.position.Row][this.position.Column];
        }
        public void Advance()
        {
            if(this.position.Column == this.buffer[this.position.Row].Length)
                position.NextLine();
            else
                position.NextChar();

            if(this.position.Row == this.buffer.Length)
                this.isFinished = true;
        }
    }
}
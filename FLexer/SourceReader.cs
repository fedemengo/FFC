using System;
using System.IO;

namespace Lexer
{
    class SourceReader
    {
        private static int BYTE_TO_READ = 1024;
        private FileStream fileStream;
        private byte[] bufferContainer;
        private int bufferPosition;
        private int byteRead;
        private bool isFinished;
        private bool isEmpty;
        private Position position = new Position();
        public Position GetPosition()
        {
            return new Position(position);
        }
        public SourceReader(string filePath)
        {
            this.fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            this.bufferContainer = new byte[BYTE_TO_READ];
            this.isFinished = false;
            this.isEmpty = false;

            UpdateBuffer();
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

        private bool EndOfBuffer()
        {
            if(this.bufferPosition == this.byteRead){
                if(this.isFinished){
                    this.isEmpty = true;
                }
                return true;
            }
            return false;
        }

        private void UpdateBuffer()
        {
            if(!Empty())
            {
                this.bufferPosition = 0;
                this.byteRead = this.fileStream.Read(bufferContainer, 0, BYTE_TO_READ);

                if(this.byteRead != BYTE_TO_READ)
                {
                    this.isFinished = true;
                }
            }
        }
        public bool Empty() => this.isEmpty;
        public char GetChar()
        {
            if(this.Empty())
            {
                return '\0';
            }
            return Convert.ToChar(this.bufferContainer[this.bufferPosition]);
        }
        public void Advance()
        {
            if(GetChar() == '\n')
                position.NextLine();
            else
                position.NextChar();
            this.bufferPosition++;
            if(this.EndOfBuffer()){
                this.UpdateBuffer();
            }
        }
    }
}
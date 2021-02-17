using System;

namespace LdtkParser.Exceptions
{
    public class InvalidTileException: Exception
    {
        public InvalidTileException() : base() { }

        public InvalidTileException(string message) : base(message) { }

        public InvalidTileException(string message, Exception inner) : base(message, inner) { }
    }
}

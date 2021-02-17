using System;
using System.Collections.Generic;
using System.Text;

namespace LdtkParser.Exceptions
{
    public class TilesetNotFoundException : Exception
    {
        public TilesetNotFoundException() { }
        public TilesetNotFoundException(string message) : base(message) { }
        public TilesetNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}

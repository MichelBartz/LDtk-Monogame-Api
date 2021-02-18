using System;

namespace LdtkParser.Exceptions
{
    public class FieldStoreException : Exception
    {
        public FieldStoreException() : base() { }

        public FieldStoreException(string message) : base(message) { }

        public FieldStoreException(string message, Exception inner) : base(message, inner) { }
    }
}

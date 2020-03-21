using System;

namespace Chronicle
{
    public class ChronicleException : Exception
    {
        public ChronicleException(string message) : base(message)
        {
        }

        public ChronicleException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

using System;

namespace Chronicle
{
    public class ChronicleException : Exception
    {
        public ChronicleException(string message) : base(message)
        {
        }
    }
}

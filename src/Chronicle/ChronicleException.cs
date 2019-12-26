using System;
using System.Diagnostics.Tracing;

namespace Chronicle
{
    public class ChronicleException : Exception
    {
        public ChronicleException(string message) : base(message)
        { }
    }
}

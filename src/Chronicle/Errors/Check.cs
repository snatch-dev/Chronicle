using System;

namespace Chronicle.Errors
{
    internal static class Check
    {
        internal static void Is<TExpected>(Type type, string message = null)
        {
            if (type is TExpected)
            {
                throw new ChronicleException($"Invalid argument type");
            }
        }

        internal static void IsNull<TData>(TData data, string message = null) where TData : class
        {
            if(data is null)
            {
                throw new ChronicleException($"Argument null");
            }
        }
    }
}

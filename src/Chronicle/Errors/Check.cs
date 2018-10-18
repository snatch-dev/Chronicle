using System;

namespace Chronicle.Errors
{
    internal static class Check
    {
        internal static void Is<TExpected>(Type type, string message = null)
        {
            if (!(typeof(TExpected).IsAssignableFrom(type)))
            {
                message = message ?? CheckErrorMessages.InvalidArgumentType;
                throw new ChronicleException(message);
            }
        }

        internal static void IsNull<TData>(TData data, string message = null) where TData : class
        {
            if(data is null)
            {
                message = message ?? CheckErrorMessages.ArgumentNull;
                throw new ChronicleException(message);
            }
        }

        public static class CheckErrorMessages
        {
            public static string InvalidArgumentType = "Invalid argument type";
            public static string ArgumentNull = "Argument null";
        }
    }
}

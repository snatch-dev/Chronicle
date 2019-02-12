using System;
using System.Linq;

namespace Chronicle.Utils
{
    internal static class Extensions
    {
        public static Type GetSagaDataType(this ISaga saga)
            => saga
                .GetType()
                .GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISaga<>))
               ?.GetGenericArguments()
                .FirstOrDefault();
        
        public static object InvokeGeneric(this ISaga saga, string method, params object[] args)
            => saga
                .GetType()
                .GetMethod(method, args.Select(arg => arg.GetType()).ToArray())
                ?.Invoke(saga, args);
    }
}

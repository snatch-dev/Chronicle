using System;

namespace Chronicle.Utils
{
    internal static class DateTimeExtensions
    {
        internal static long GetTimeStamp(this DateTimeOffset dateTime)
            => dateTime.ToUnixTimeMilliseconds();
    }
}

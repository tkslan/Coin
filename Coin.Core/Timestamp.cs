using System;

namespace Coin.Core
{
    public static class TimeStamp
    {
        public static readonly DateTime UnixBase = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        static long Current()
        {
            var subtracted = DateTime.UtcNow.Subtract(UnixBase);
            return (long) subtracted.TotalMilliseconds;
        }
    }
}
using System;

namespace LiteNinja.TimeUtils
{
    public static class DurationExtensions
    {
        public static Duration Negate(this Duration duration)
        {
            return new Duration(-duration.Milliseconds);
        }

        public static Duration Abs(this Duration duration)
        {
            return new Duration(Math.Abs(duration.Milliseconds));
        }

        public static DateTime After(this Duration self, DateTime dateTime)
        {
            return dateTime.AddMilliseconds(self.Milliseconds);
        }


        public static DateTime Before(this Duration self, DateTime dateTime)
        {
            return dateTime.AddMilliseconds(-self.Milliseconds);
        }

        public static DateTime Before(this DateTime self, Duration duration)
        {
            return self.AddMilliseconds(-duration.Milliseconds);
        }

        public static DateTime After(this DateTime self, Duration duration)
        {
            return self.AddMilliseconds(duration.Milliseconds);
        }

        public static Duration Since(this DateTime self)
        {
            return new Duration(DateTime.Now.Subtract(self).TotalMilliseconds);
        }

        public static Duration Until(this DateTime self)
        {
            return new Duration(self.Subtract(DateTime.Now).TotalMilliseconds);
        }

        public static Duration Between(this DateTime self, DateTime other)
        {
            return new Duration(self.Subtract(other).TotalMilliseconds);
        }
        
        public static TimeSpan ToTimeSpan(this Duration self)
        {
            return TimeSpan.FromMilliseconds(self.Milliseconds);
        }
        
        public static Duration ToDuration(this TimeSpan self)
        {
            return new Duration(self.TotalMilliseconds);
        }
        
        
    }
}
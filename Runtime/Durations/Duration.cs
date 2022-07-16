using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace LiteNinja.TimeUtils
{
    /// <summary>
    /// A class to deal with Durations.
    /// Duration can be expressed as a string in the format of "10w9d8h7m6s5ms", with optional value for each unit.
    /// </summary>
    [Serializable]
    public class Duration
    {
        #region Consts

        private const double Millisecond = 1;
        private const double Second = 1000 * Millisecond;
        private const double Minute = 60 * Second;
        private const double Hour = 60 * Minute;
        private const double Day = 24 * Hour;
        private const double Week = 7 * Day;

        private static readonly Dictionary<string, double> UnitToMillisecond = new()
        {
            { "ms", Millisecond },
            { "s", Second },
            { "m", Minute },
            { "h", Hour },
            { "d", Day },
            { "w", Week }
        };

        private static Regex _unitRegex = new(@"(?:(\d+\.?\d*)|(\.\d*))\s*(ms|s|m|h|d|w)");
        private static Regex _alphaRegex = new(@"[^a-zA-Z]");

        #endregion

        [SerializeField] private double _milliseconds;

        public double Milliseconds => _milliseconds;
        public double Seconds => _milliseconds / Second;
        public double Minutes => _milliseconds / Minute;
        public double Hours => _milliseconds / Hour;
        public double Days => _milliseconds / Day;
        public double Weeks => _milliseconds / Week;

        public Duration(double milliseconds)
        {
            _milliseconds = milliseconds;
        }

        public Duration(string duration)
        {
            var value = Parse(duration);
            _milliseconds = value.Milliseconds;
        }

        public override string ToString()
        {
            var result = "";
            var milliseconds = _milliseconds < 0 ? -_milliseconds : _milliseconds;
            var sign = _milliseconds < 0 ? "-" : "";
            if (milliseconds == 0)
            {
                return "0";
            }

            // Weeks
            var weeks = Math.Floor(milliseconds / Week);
            if (weeks > 0)
            {
                result += $"{weeks}w";
                milliseconds -= weeks * Week;
            }

            // Days
            var days = Math.Floor(milliseconds / Day);
            if (days > 0)
            {
                result += $"{days}d";
                milliseconds -= days * Day;
            }

            //hours
            var hours = Math.Floor(milliseconds / Hour);
            if (hours > 0)
            {
                result += $"{hours}h";
                milliseconds -= hours * Hour;
            }

            //minutes
            var minutes = Math.Floor(milliseconds / Minute);
            if (minutes > 0)
            {
                result += $"{minutes}m";
                milliseconds -= minutes * Minute;
            }

            //seconds
            var seconds = Math.Floor(milliseconds / Second);
            if (seconds > 0)
            {
                result += $"{seconds}s";
                milliseconds -= seconds * Second;
            }

            //milliseconds
            var floorMilliseconds = Math.Floor(milliseconds);
            if (floorMilliseconds > 0)
            {
                result += $"{floorMilliseconds}ms";
                milliseconds -= floorMilliseconds;
            }


            return $"{sign}{result}";
        }

        public static Duration Parse(string duration)
        {
            if (duration == null) return new Duration(0f);
            duration = duration.Trim().ToLower();
            if (string.IsNullOrEmpty(duration) || (duration is "0" or "+0" or "-0"))
            {
                return new Duration(0f);
            }

            var negative = duration.StartsWith("-");
            if (negative)
            {
                duration = duration[1..];
            }

            var matches = _unitRegex.Matches(duration);
            double totalMilliseconds = 0f;
            foreach (var match in matches.AsEnumerable())
            {
                var matchString = match.ToString();
                var unit = _alphaRegex.Replace(matchString, "");
                matchString = matchString.Replace(unit, "");
                var matchValue = double.Parse(matchString);
                if (UnitToMillisecond.ContainsKey(unit))
                {
                    totalMilliseconds += matchValue * UnitToMillisecond[unit];
                }
            }

            if (negative)
            {
                totalMilliseconds = -totalMilliseconds;
            }

            return new Duration(totalMilliseconds);
        }

        public static bool Parseable(string duration)
        {
            var matches = _unitRegex.Matches(duration);
            //check if there is at least one match
            if (matches.Count == 0)
            {
                return false;
            }

            return matches.AsEnumerable().Select(match => match.ToString())
                .Select(matchString => _alphaRegex.Replace(matchString, ""))
                .All(unit => UnitToMillisecond.ContainsKey(unit));
        }

        #region Operators

        public static Duration operator +(Duration a, Duration b)
        {
            return new Duration(a._milliseconds + b._milliseconds);
        }

        public static Duration operator -(Duration a, Duration b)
        {
            return new Duration(a._milliseconds - b._milliseconds);
        }

        public static Duration operator *(Duration a, double b)
        {
            return new Duration(a._milliseconds * b);
        }

        public static Duration operator *(double a, Duration b)
        {
            return new Duration(a * b._milliseconds);
        }

        public static Duration operator /(Duration a, double b)
        {
            return new Duration(a._milliseconds / b);
        }
        
        public static DateTime operator +(Duration a, DateTime b)
        {
            return b.AddMilliseconds(a._milliseconds);
        }
        
        public static DateTime operator -(Duration a, DateTime b)
        {
            return b.AddMilliseconds(-a._milliseconds);
        }
        
        public static DateTime operator +(DateTime a, Duration b)
        {
            return a.AddMilliseconds(b._milliseconds);
        }
        
        public static DateTime operator -(DateTime a, Duration b)
        {
            return a.AddMilliseconds(-b._milliseconds);
        }
        

        #endregion

        #region Equality

        public override bool Equals(object obj)
        {
            if (obj is Duration duration)
            {
                return _milliseconds == duration._milliseconds;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return _milliseconds.GetHashCode();
        }

        public static bool operator ==(Duration a, Duration b)
        {
            return a._milliseconds == b._milliseconds;
        }
        
        
        public static bool operator !=(Duration a, Duration b)
        {
            return a._milliseconds != b._milliseconds;
        }

        public static bool operator <(Duration a, Duration b)
        {
            return a._milliseconds < b._milliseconds;
        }

        public static bool operator >(Duration a, Duration b)
        {
            return a._milliseconds > b._milliseconds;
        }

        public static bool operator <=(Duration a, Duration b)
        {
            return a._milliseconds <= b._milliseconds;
        }

        public static bool operator >=(Duration a, Duration b)
        {
            return a._milliseconds >= b._milliseconds;
        }

        public static bool operator ==(double a, Duration b)
        {
            return a == b._milliseconds;
        }

        public static bool operator !=(double a, Duration b)
        {
            return a != b._milliseconds;
        }

        public static bool operator <(double a, Duration b)
        {
            return a < b._milliseconds;
        }

        public static bool operator >(double a, Duration b)
        {
            return a > b._milliseconds;
        }

        public static bool operator <=(double a, Duration b)
        {
            return a <= b._milliseconds;
        }

        public static bool operator >=(double a, Duration b)
        {
            return a >= b._milliseconds;
        }

        public static bool operator ==(Duration a, double b)
        {
            return a._milliseconds == b;
        }

        public static bool operator !=(Duration a, double b)
        {
            return a._milliseconds != b;
        }

        public static bool operator <(Duration a, double b)
        {
            return a._milliseconds < b;
        }

        public static bool operator >(Duration a, double b)
        {
            return a._milliseconds > b;
        }

        public static bool operator <=(Duration a, double b)
        {
            return a._milliseconds <= b;
        }

        public static bool operator >=(Duration a, double b)
        {
            return a._milliseconds >= b;
        }

        #endregion

        #region Conversion

        public static implicit operator Duration(double milliseconds)
        {
            return new Duration(milliseconds);
        }

        public static implicit operator double(Duration duration)
        {
            return duration._milliseconds;
        }

        public static implicit operator string(Duration duration)
        {
            return duration.ToString();
        }

        #endregion
        

        #region static factory method

        public static Duration FromMilliseconds(double milliseconds)
        {
            return new Duration(milliseconds);
        }

        public static Duration FromSeconds(double seconds)
        {
            return new Duration(seconds * Second);
        }

        public static Duration FromMinutes(double minutes)
        {
            return new Duration(minutes * Minute);
        }

        public static Duration FromHours(double hours)
        {
            return new Duration(hours * Hour);
        }

        public static Duration FromDays(double days)
        {
            return new Duration(days * Day);
        }

        public static Duration FromWeeks(double weeks)
        {
            return new Duration(weeks * Week);
        }

        public static Duration FromString(string duration)
        {
            return new Duration(duration);
        }

        #endregion
    }
}
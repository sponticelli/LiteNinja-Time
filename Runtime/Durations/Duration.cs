using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LiteNinja.TimeUtils.Durations
{
    public static class Duration
    {
        #region Consts
        private const double Nanosecond = Microsecond / 1000;
        private const double Microsecond = Millisecond / 1000;
        private const double Millisecond = 1;
        private const double Second = 1000 * Millisecond;
        private const double Minute = 60 * Second;
        private const double Hour = 60 * Minute;
        private const double Day = 24 * Hour;
        private const double Week = 7 * Day;

        private static readonly Dictionary<string, double> UnitToMillisecond = new()
        {
            { "ns", Nanosecond },
            { "us", Microsecond },
            { "µs", Microsecond },
            { "ms", Millisecond },
            { "s", Second },
            { "m", Minute },
            { "h", Hour },
            { "d", Day },
            { "w", Week }
        };
        #endregion

        /// <summary>
        /// Convert a duration string to a time span, even if it's not a valid duration string.
        /// </summary>
        public static TimeSpan SafeParse(string duration)
        {
            return Parse(duration);
        }
        
        /// <summary>
        /// Convert a duration string to a time span.
        /// if ignoreInvalid is true, the part of the string that is not a valid duration will be ignored.
        /// </summary>
        public static TimeSpan Parse(string duration, bool ignoreInvalid = false)
        {
            var tokenizer = new Tokenizer(duration);
            tokenizer.Tokenize();
            var tokens = ignoreInvalid ? tokenizer.CleanTokens : tokenizer.Tokens;
            double totalMilliseconds = 0;
            if (tokens.Count == 0) return new TimeSpan(0);
            var index = 0;
            while (index < tokens.Count)
            {
                var token = tokens[index];
                if (token.Type == TokenType.Number)
                {
                    var number = double.Parse(token.Value);
                    index = ExtractUnit(duration, ignoreInvalid, index, tokens, number, ref totalMilliseconds);
                }
                else
                {
                    index++;
                }
            }
            
            return TimeSpan.FromMilliseconds(totalMilliseconds);
        }

        

        /// <summary>
        /// Check if a string is a completely valid duration string.
        /// </summary>
        public static bool Parseable(string duration)
        {
            var tokenizer = new Tokenizer(duration);
            tokenizer.Tokenize();
            return !tokenizer.HasNonToken;
        }
        
        
        
        /// <summary>
        /// Try to parse a duration string to a time span.
        /// </summary>
        public static bool TryParse(string duration, out TimeSpan timeSpan)
        {
            try
            {
                timeSpan = Parse(duration);
                return true;
            }
            catch
            {
                timeSpan = new TimeSpan();
                return false;
            }
        }

        /// <summary>
        /// Convert a TimeSpan to a duration string.
        /// </summary>
        public static string ToDuration(this TimeSpan timeSpan)
        {
            var result = "";
            var milliseconds = timeSpan.TotalMilliseconds;
            var sign = milliseconds < 0 ? "-" : "";
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
            
            //microseconds
            var microseconds = Math.Floor(milliseconds / Microsecond);
            if (microseconds > 0)
            {
                result += $"{microseconds}µs";
                milliseconds -= microseconds * Microsecond;
            }
            
            //nanoseconds
            var nanoseconds = Math.Floor(milliseconds / Nanosecond);
            if (nanoseconds > 0)
            {
                result += $"{nanoseconds}ns";
                milliseconds -= nanoseconds * Nanosecond;
            }
            
            return $"{sign}{result}";
        }
        
        #region Private methods
        private static int ExtractUnit(string duration, bool ignoreInvalid, int index, IReadOnlyList<Token> tokens, double number,
            ref double totalMilliseconds)
        {
            if (index + 1 < tokens.Count)
            {
                var unitToken = tokens[index + 1];
                if (unitToken.Type == TokenType.Unit)
                {
                    totalMilliseconds = AddMilliseconds(ignoreInvalid, unitToken, totalMilliseconds, number,
                        ref index);
                }
                else
                {
                    throw new FormatException($"Invalid duration format: {duration}");
                }
            }
            else
            {
                totalMilliseconds += number;
                index++;
            }

            return index;
        }

        private static double AddMilliseconds(bool ignoreInvalid, Token unitToken, double totalMilliseconds, double number,
            ref int index)
        {
            var unit = unitToken.Value;
            if (UnitToMillisecond.TryGetValue(unit, out var unitMilliseconds))
            {
                totalMilliseconds += number * unitMilliseconds;
                index += 2;
            }
            else
            {
                if (ignoreInvalid)
                {
                    index += 2;
                }
                else
                {
                    throw new ArgumentException($"Invalid unit: {unit}");
                }
            }

            return totalMilliseconds;
        }
        #endregion
        
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace LiteNinja.TimeUtils.Tests
{
    public class DurationTests
    {
        private const double Millisecond = 1;
        private const double Second = 1000 * Millisecond;
        private const double Minute = 60 * Second;
        private const double Hour = 60 * Minute;
        private const double Day = 24 * Hour;
        private const double Week = 7 * Day;


        //Test that Duration.Parse() works correctly with the following inputs:
        [Test]
        public void Simple()
        {
            Assert.AreEqual(0, Duration.Parse("0").Milliseconds);
            Assert.AreEqual(5 * Second, Duration.Parse("5s").Milliseconds);
            Assert.AreEqual(30 * Second, Duration.Parse("30s").Milliseconds);
            Assert.AreEqual(1478 * Second, Duration.Parse("1478s").Milliseconds);
            Assert.AreEqual(Second, Duration.Parse("1s").Milliseconds);
            Assert.AreEqual(Minute, Duration.Parse("1m").Milliseconds);
            Assert.AreEqual(Hour, Duration.Parse("1h").Milliseconds);
            Assert.AreEqual(Day, Duration.Parse("1d").Milliseconds);
            Assert.AreEqual(Week, Duration.Parse("1w").Milliseconds);
        }

        [Test]
        public void Sign()
        {
            Assert.AreEqual(-5 * Second, Duration.Parse("-5s").Milliseconds);
            Assert.AreEqual(5 * Second, Duration.Parse("+5s").Milliseconds);
            Assert.AreEqual(0, Duration.Parse("-0").Milliseconds);
            Assert.AreEqual(0, Duration.Parse("+0").Milliseconds);
        }

        [Test]
        public void Decimal()
        {
            Assert.AreEqual(5 * Second, Duration.Parse("5.0s").Milliseconds);
            Assert.AreEqual(5 * Second + 600, Duration.Parse("5.6s").Milliseconds);
            Assert.AreEqual(5 * Second, Duration.Parse("5.s").Milliseconds);
            Assert.AreEqual(0.5 * Second, Duration.Parse(".5s").Milliseconds);
            Assert.AreEqual(0.5 * Second, Duration.Parse("0.5s").Milliseconds);
            Assert.AreEqual(1 * Second, Duration.Parse("1.0s").Milliseconds);
            Assert.AreEqual(1 * Second, Duration.Parse("1.00s").Milliseconds);
            Assert.AreEqual(1 * Second + 4, Duration.Parse("1.004s").Milliseconds);
            Assert.AreEqual(1 * Second + 4, Duration.Parse("1.0040s").Milliseconds);
            Assert.AreEqual(100 * Second + 1, Duration.Parse("100.00100s").Milliseconds);
        }

        [Test]
        public void DifferentUnits()
        {
            Assert.AreEqual(13, Duration.Parse("13ms").Milliseconds);
            Assert.AreEqual(14 * Second, Duration.Parse("14s").Milliseconds);
            Assert.AreEqual(15 * Minute, Duration.Parse("15m").Milliseconds);
            Assert.AreEqual(16 * Hour, Duration.Parse("16h").Milliseconds);
            Assert.AreEqual(17 * Day, Duration.Parse("17d").Milliseconds);
            Assert.AreEqual(18 * Week, Duration.Parse("18w").Milliseconds);
        }

        [Test]
        public void CompositeDuration()
        {
            Assert.AreEqual(5 * Second + 600, Duration.Parse("5s600ms").Milliseconds);
            Assert.AreEqual(3 * Hour + 30 * Minute + 5 * Second, Duration.Parse("3h30m5s").Milliseconds);
            Assert.AreEqual(4 * Minute + 10 * Second + 500, Duration.Parse("10.5s4m").Milliseconds);
            Assert.AreEqual(5 * Hour + 6 * Minute + 7 * Second + 8 * Millisecond,
                Duration.Parse("5h6m7.008s").Milliseconds);
            Assert.AreEqual(-(2 * Minute + 3 * Second + 400 * Millisecond), Duration.Parse("-2m3.4s").Milliseconds);
            Assert.AreEqual(-(3 * Hour + 4 * Minute + 5 * Second + 6 * Millisecond),
                Duration.Parse("-3h4m5.006s").Milliseconds);
            Assert.AreEqual(10 * Week + 5 * Day + 39 * Hour + 9 * Minute + 14 * Second + 425 * Millisecond,
                Duration.Parse("10w5d39h9m14.425s").Milliseconds);
        }

        [Test]
        public void LargeValues()
        {
            Assert.AreEqual(long.MaxValue, Duration.Parse(long.MaxValue + "ms").Milliseconds);
            Assert.AreEqual(long.MinValue, Duration.Parse(long.MinValue + "ms").Milliseconds);
        }

        //Test that Duration.ToString returns the correct string for the provided list of inputs
        [Test]
        public void ConvertToString()
        {
            var tests = new List<string>()
            {
                "1ms",
                "-1ms",
                "2s",
                "1m",
                "1h",
                "1d",
                "1w",
                "1w1d",
                "1w1d1h",
            };
            foreach (var test in tests)
            {
                var duration = new Duration(test);
                Assert.AreEqual(test, duration.ToString(), test);
            }
        }

        //Test Between extension method
        [Test]
        public void Between()
        {
            var duration = new DateTime(2000, 1, 2).Between(new DateTime(2000, 1, 1));
            Assert.AreEqual(1 * Day, duration.Milliseconds);
            duration = new DateTime(2000, 1, 1).Between(new DateTime(2000, 1, 1));
            Assert.AreEqual(0, duration.Milliseconds);
            duration = new DateTime(2000, 1, 2).Between(new DateTime(2000, 1, 1, 23, 59, 59, 1));
            Assert.AreEqual(999, duration.Milliseconds);
        }
        
        //Test Before extension method
        [Test]
        public void Before()
        {
            var duration = Duration.FromDays(1);
            var date = new DateTime(2000, 1, 1);
            Assert.AreEqual(new DateTime(1999, 12, 31), duration.Before(date));
            Assert.AreEqual(new DateTime(1999, 12, 31), date.Before(duration));
        }
        
        //Test After extension method
        [Test]
        public void After()
        {
            var duration = Duration.FromDays(1);
            var date = new DateTime(2000, 1, 1);
            Assert.AreEqual(new DateTime(2000, 1, 2), duration.After(date));
            Assert.AreEqual(new DateTime(2000, 1, 2), date.After(duration));
        }

        [Test]
        public void Parseable()
        {
            Assert.IsTrue(Duration.Parseable("1s"));
            Assert.IsFalse(Duration.Parseable("1person"));
        }

        [Test]
        public void Operators()
        {
            var duration = Duration.FromDays(1);
            Assert.AreEqual(Duration.FromDays(2), duration + Duration.FromDays(1));
            Assert.AreEqual(Duration.FromDays(0), duration - Duration.FromDays(1));
            Assert.AreEqual(Duration.FromDays(2), duration * 2);
            Assert.AreEqual(Duration.FromDays(0.5), duration / 2);
            Assert.AreEqual(Duration.FromHours(12), duration * 0.5f);
        }
        
        [Test]
        public void Equality()
        {
            var duration = Duration.FromDays(1);
            Assert.IsTrue(duration > Duration.FromDays(0.5f));
            Assert.IsTrue(duration >= 0.5f * Day);
            Assert.IsTrue(duration < Duration.FromDays(2));
            Assert.IsTrue(duration <= 1.5 * Day);
            Assert.IsTrue(duration == Duration.Parse("24h"));
        }
    }
}
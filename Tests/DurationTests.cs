using System;
using System.Collections;
using System.Collections.Generic;
using LiteNinja.TimeUtils.Durations;
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


        //Test Duration.Parse to check if creates correct TimeSpan
        [Test]
        public void ToTimeSpan()
        {
            var timeSpan = Duration.Parse("1s");
            Assert.AreEqual(timeSpan.TotalSeconds, 1);
            
            timeSpan = Duration.Parse("1m");
            Assert.AreEqual(timeSpan.TotalMinutes, 1);
            
            timeSpan = Duration.Parse("1h");
            Assert.AreEqual(timeSpan.TotalHours, 1);
            
            timeSpan = Duration.Parse("1.5h");
            Assert.AreEqual(timeSpan.TotalHours, 1.5);

        }

        [Test]
        public void FromTimeSpan()
        {
            var timeSpan = new TimeSpan(0, 0, 1);
            Assert.AreEqual( "1s", timeSpan.ToDuration());
            
            timeSpan = new TimeSpan(0, 1, 0);
            Assert.AreEqual( "1m", timeSpan.ToDuration());
            
            timeSpan = new TimeSpan(1, 0, 0);
            Assert.AreEqual( "1h", timeSpan.ToDuration());
            
            timeSpan = new TimeSpan(1, 30, 0);
            Assert.AreEqual( "1h30m", timeSpan.ToDuration());
        }
        
        
    }
}
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodaTime;

namespace MiscUnitTests.TimeRange
{
    [TestClass]
    public class TimeRangeTests
    {
        [TestMethod]
        public void IsBetweenExclusive_ValueIsBetween_True()
        {
            Assert.IsTrue(GetLocalTime(0).IsBetweenExclusive(GetTimeSpan(23), GetTimeSpan(1), 2));
            Assert.IsTrue(GetLocalTime(22).IsBetweenExclusive(GetTimeSpan(0), GetTimeSpan(6), 2));
            Assert.IsTrue(GetLocalTime(23).IsBetweenExclusive(GetTimeSpan(1), GetTimeSpan(6), 2));
            Assert.IsTrue(GetLocalTime(2).IsBetweenExclusive(GetTimeSpan(2), GetTimeSpan(3), 2));
            Assert.IsTrue(GetLocalTime(8).IsBetweenExclusive(GetTimeSpan(6), GetTimeSpan(9), 2));
            Assert.IsTrue(GetLocalTime(12).IsBetweenExclusive(GetTimeSpan(13), GetTimeSpan(20), 2));
            Assert.IsTrue(GetLocalTime(12).IsBetweenExclusive(GetTimeSpan(10), GetTimeSpan(13), 2));
            Assert.IsTrue(GetLocalTime(12).IsBetweenExclusive(GetTimeSpan(12), GetTimeSpan(12), 0));
        }

        [TestMethod]
        public void IsBetweenExclusive_ValueIsNotBetween_False()
        {
            Assert.IsFalse(GetLocalTime(0).IsBetweenExclusive(GetTimeSpan(3), GetTimeSpan(6), 2));
            Assert.IsFalse(GetLocalTime(23).IsBetweenExclusive(GetTimeSpan(2), GetTimeSpan(6), 2));
            Assert.IsFalse(GetLocalTime(12).IsBetweenExclusive(GetTimeSpan(15), GetTimeSpan(19), 2));
            Assert.IsFalse(GetLocalTime(12).IsBetweenExclusive(GetTimeSpan(3), GetTimeSpan(9), 2));
            Assert.IsFalse(GetLocalTime(12).IsBetweenExclusive(GetTimeSpan(15), GetTimeSpan(20), 2));
            Assert.IsFalse(GetLocalTime(12).IsBetweenExclusive(GetTimeSpan(6), GetTimeSpan(10), 2));
        }

        [TestMethod]
        public void MergeRanges_ContainsItemsToMerge_ReturnsMergedItems()
        {
            Assert.IsTrue(ScheduleTimeValidationService.MergeRanges(GetTestTimeRanges()).Count == 2);
        }

        private static List<TimeRange> GetTestTimeRanges()
        {
            var ranges = new List<TimeRange>
            {
                new TimeRange(GetLocalTime(8), GetLocalTime(10)),
                new TimeRange(GetLocalTime(9), GetLocalTime(11)),
                new TimeRange(GetLocalTime(12), GetLocalTime(18))
            };
            return ranges;
        }

        //private static Tuple<LocalTime, LocalTime> GetTestTimeRange()
        //{
        //    //var item = new Tuple<LocalTime, LocalTime>(GetLocalTime(8), GetLocalTime(10));
        //    var item = Tuple.Create(GetLocalTime(8), GetLocalTime(10));
        //    return item;
        //}

        //private static (LocalTime, LocalTime) GetRange()
        //{
        //    var item = (GetLocalTime(8), GetLocalTime(10));
        //    var (startTime, endTime) = (GetLocalTime(8), GetLocalTime(10));
        //    return item;
        //}

        private static TimeSpan GetTimeSpan(int hours)
        {
            return new TimeSpan(hours, 0, 0);
        }

        private static LocalTime GetLocalTime(int hours)
        {
            return new LocalTime(hours, 0, 0);
        }
    }
}
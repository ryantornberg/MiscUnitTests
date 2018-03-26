using System.Collections.Generic;
using System.Linq;
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
            Assert.IsTrue(GetTime(0).IsBetween(GetTime(23), GetTime(1), 2));
            Assert.IsTrue(GetTime(5).IsBetween(GetTime(6), GetTime(6), 1));
            Assert.IsTrue(GetTime(22).IsBetween(GetTime(0), GetTime(6), 2));
            Assert.IsTrue(GetTime(23).IsBetween(GetTime(1), GetTime(6), 2));
            Assert.IsTrue(GetTime(2).IsBetween(GetTime(2), GetTime(3), 2));
            Assert.IsTrue(GetTime(8).IsBetween(GetTime(6), GetTime(9), 2));
            Assert.IsTrue(GetTime(12).IsBetween(GetTime(13), GetTime(20), 2));
            Assert.IsTrue(GetTime(12).IsBetween(GetTime(10), GetTime(13), 2));
            Assert.IsTrue(GetTime(12).IsBetween(GetTime(12), GetTime(12), 0));
            Assert.IsTrue(GetTime(11).IsBetween(GetTime(8), GetTime(11), 0));
        }

        [TestMethod]
        public void IsBetweenExclusive_ValueIsNotBetween_False()
        {
            Assert.IsFalse(GetTime(0).IsBetween(GetTime(3), GetTime(6), 2));
            Assert.IsFalse(GetTime(5).IsBetween(GetTime(6), GetTime(6), 0));
            Assert.IsFalse(GetTime(23).IsBetween(GetTime(2), GetTime(6), 2));
            Assert.IsFalse(GetTime(12).IsBetween(GetTime(15), GetTime(19), 2));
            Assert.IsFalse(GetTime(12).IsBetween(GetTime(3), GetTime(9), 2));
            Assert.IsFalse(GetTime(12).IsBetween(GetTime(15), GetTime(20), 2));
            Assert.IsFalse(GetTime(13).IsBetween(GetTime(6), GetTime(10), 2));
        }

        [TestMethod]
        public void MergeRanges_ContainsItemsToMerge_ReturnsMergedItems()
        {
            List<(int, int)> rangeTuples = null;

            rangeTuples = new List<(int, int)>();
            Assert.AreEqual(TimeRangeHelper.MergeRanges(GetTestRanges(rangeTuples)).Count, 0);

            rangeTuples = new List<(int, int)> {(9, 9), (10, 11)};
            Assert.AreEqual(TimeRangeHelper.MergeRanges(GetTestRanges(rangeTuples)).Count, 2);

            rangeTuples = new List<(int, int)> {(9, 10), (10, 11)};
            Assert.AreEqual(TimeRangeHelper.MergeRanges(GetTestRanges(rangeTuples)).Count, 1);

            rangeTuples = new List<(int, int)> {(10, 11), (9, 10)};
            Assert.AreEqual(TimeRangeHelper.MergeRanges(GetTestRanges(rangeTuples)).Count, 1);

            rangeTuples = new List<(int, int)> {(8, 10), (9, 11), (12, 18)};
            Assert.AreEqual(TimeRangeHelper.MergeRanges(GetTestRanges(rangeTuples)).Count, 2);

            rangeTuples = new List<(int, int)> {(8, 10), (9, 11), (7, 12), (12, 18)};
            Assert.AreEqual(TimeRangeHelper.MergeRanges(GetTestRanges(rangeTuples)).Count, 1);

            rangeTuples = new List<(int, int)> {(8, 10), (8, 10), (7, 11), (11, 12)};
            Assert.AreEqual(TimeRangeHelper.MergeRanges(GetTestRanges(rangeTuples)).Count, 1);

            rangeTuples = new List<(int, int)> {(8, 10), (11, 13), (15, 17)};
            Assert.AreEqual(TimeRangeHelper.MergeRanges(GetTestRanges(rangeTuples)).Count, 3);

            rangeTuples = new List<(int, int)> {(23, 0), (0, 1), (2, 3)};
            Assert.AreEqual(TimeRangeHelper.MergeRanges(GetTestRanges(rangeTuples)).Count, 2);

            rangeTuples = new List<(int, int)> {(23, 1), (2, 3)};
            Assert.AreEqual(TimeRangeHelper.MergeRanges(GetTestRanges(rangeTuples)).Count, 2);
        }

        private static List<TimeRange> GetTestRanges(IEnumerable<(int, int)> rangeTuples)
        {
            return rangeTuples.Select(rangeTuple => new TimeRange(GetTime(rangeTuple.Item1), GetTime(rangeTuple.Item2)))
                .ToList();
        }

        private static LocalTime GetTime(int hours)
        {
            return new LocalTime(hours, 0, 0);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using NodaTime;

namespace MiscUnitTests.TimeRange
{
    /// <summary>
    /// All times are only meant to handle hours. 
    /// </summary>
    public static class TimeRangeHelper
    {
        /// <summary>
        /// startTimeSpan lte timeToCheck lte endTimeSpan
        /// </summary>
        public static bool IsBetween(this LocalTime? timeToCheck, LocalTime startTime, LocalTime endTime, int safeHours,
            bool inclusive = true)
        {
            return timeToCheck.HasValue && timeToCheck.Value.IsBetween(startTime, endTime, safeHours, inclusive);
        }

        /// <summary>
        /// startTimeSpan lte timeToCheck lte endTimeSpan
        /// </summary>
        public static bool IsBetween(this LocalTime timeToCheck, LocalTime startTime, LocalTime endTime, int safeHours,
            bool inclusive = true)
        {
            startTime = startTime.PlusHours(safeHours * -1);
            endTime = endTime.PlusHours(safeHours);

            if (!inclusive)
            {
                // If this is an exclusive compare, then the times must be > 1 hours apart
                if (startTime.HoursBetween(endTime) <= 1)
                    return false;

                // shrink the window so we can do an inclusive compare from here on
                startTime = startTime.PlusHours(1);
                endTime = endTime.PlusHours(-1);
            }

            if (startTime == endTime)
                return startTime == timeToCheck;

            bool isLastCheck;
            do
            {
                isLastCheck = startTime.Hour == endTime.Hour;
                if (startTime.Hour == timeToCheck.Hour)
                    return true;
                startTime = startTime.PlusHours(1);
            } while (!isLastCheck);

            return false;
        }

        public static int HoursBetween(this LocalTime startTime, LocalTime endTime)
        {
            var count = 0;
            if (startTime.Hour == endTime.Hour)
                return count;
            do
            {
                count++;
                startTime = startTime.PlusHours(1);
            } while (startTime.Hour != endTime.Hour);

            return count;
        }

        public static LocalTime GetDisallowedStart(LocalTime startTime, int safeHours)
        {
            return startTime.PlusHours(safeHours * -1);
        }

        public static LocalTime GetDisallowedEnd(LocalTime endTime, int safeHours)
        {
            return endTime.PlusHours(safeHours);
        }

        public static List<TimeRange> MergeRanges(List<TimeRange> ranges)
        {
            return MergeRanges(ranges, new List<TimeRange>());
        }

        private static List<TimeRange> MergeRanges(IReadOnlyList<TimeRange> ranges, List<TimeRange> mergedRanges)
        {
            if (ranges.Count == 0)
                return new List<TimeRange>();

            if (mergedRanges.Count == 0 && ranges.Count > 0)
                // kick start the merger by adding the first item
                mergedRanges.Add(TimeRange.Create(ranges[0].StartTime, ranges[0].EndTime));

            foreach (TimeRange range in ranges)
            {
                // We need to use inclusive finds so we don't end up with something like 8-10 and 10-12, but we don't
                // want to make the recursive call if we matched on start or end times. I don't see a better solution right now.
                if (MergeFirstRange(mergedRanges, range, true) && MergeFirstRange(mergedRanges, range, false))
                    MergeRanges(ranges, mergedRanges);
                if (mergedRanges.All(x => !x.Equals(range)) && mergedRanges.All(x => !x.Contains(range)))
                    // and range is not contained in any of the merged ranges
                    mergedRanges.Add(range);
            }


            return mergedRanges;
        }

        private static bool MergeFirstRange(IEnumerable<TimeRange> mergedRanges, TimeRange rangeToCheck, bool inclusive)
        {
            foreach (TimeRange mergedRange in mergedRanges)
            {
                var merged = false;
                // IsBetween should send false for the inclusive flag because it's only considered a merge when the 
                // start or end time actually changes.
                if (mergedRange.StartTime.IsBetween(rangeToCheck.StartTime, rangeToCheck.EndTime, 0, inclusive))
                {
                    mergedRange.StartTime = rangeToCheck.StartTime;
                    merged = true;
                }

                if (mergedRange.EndTime.IsBetween(rangeToCheck.StartTime, rangeToCheck.EndTime, 0, inclusive))
                {
                    mergedRange.EndTime = rangeToCheck.EndTime;
                    merged = true;
                }

                if (merged)
                    return true;
            }

            return false;
        }

        public static bool Contains(this TimeRange currRange, TimeRange rangeToCheck)
        {
            bool startContains = rangeToCheck.StartTime.IsBetween(currRange.StartTime, currRange.EndTime, 0);
            bool endContains = rangeToCheck.EndTime.IsBetween(currRange.StartTime, currRange.EndTime, 0);
            return startContains && endContains;
        }
    }
}
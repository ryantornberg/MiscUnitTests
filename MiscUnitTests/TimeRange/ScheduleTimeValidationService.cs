using System.Collections.Generic;

namespace MiscUnitTests.TimeRange
{
    public class ScheduleTimeValidationService
    {
        public ScheduleTimeValidationService()
        {
        }

        public static List<TimeRange> MergeRanges(List<TimeRange> ranges)
        {
            return MergeRanges(ranges, new List<TimeRange>());
        }

        private static List<TimeRange> MergeRanges(List<TimeRange> ranges, List<TimeRange> mergedRanges)
        {
            if (mergedRanges.Count == 0 && ranges.Count > 0)
                mergedRanges.Add(TimeRange.Create(ranges[0].StartTime.Hour, ranges[0].EndTime.Hour));

            foreach (TimeRange range in ranges)
            {
                if (MergeFirstRange(mergedRanges, range))
                    MergeRanges(ranges, mergedRanges);
                else
                    mergedRanges.Add(range);
            }

            return mergedRanges;
        }

        private static bool MergeFirstRange(IEnumerable<TimeRange> mergedRanges, TimeRange rangeToCheck)
        {
            foreach (TimeRange mergedRange in mergedRanges)
            {
                var merged = false;
                if (mergedRange.StartTime >= rangeToCheck.EndTime)
                {
                    mergedRange.StartTime = rangeToCheck.EndTime;
                    merged = true;
                }

                if (mergedRange.EndTime <= rangeToCheck.StartTime)
                {
                    mergedRange.EndTime = rangeToCheck.StartTime;
                    merged = true;
                }

                if (merged)
                    return true;
            }

            return false;
        }

        private static TimeRange FindContainingRange(IEnumerable<TimeRange> mergedRanges, TimeRange rangeToCheck)
        {
            foreach (TimeRange mergedRange in mergedRanges)
            {
                if (mergedRange.StartTime >= rangeToCheck.EndTime
                    || mergedRange.EndTime <= rangeToCheck.StartTime)
                    return mergedRange;
            }

            return null;
        }
    }
}
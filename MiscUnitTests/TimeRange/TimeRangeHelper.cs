using System;
using NodaTime;

namespace MiscUnitTests.TimeRange
{
    public static class TimeRangeHelper
    {
        public static bool IsBetweenExclusive(this LocalTime? timeToCheck, TimeSpan startTimeSpan, TimeSpan endTimeSpan,
            int safeHours)
        {
            return !timeToCheck.HasValue || timeToCheck.Value.IsBetweenExclusive(startTimeSpan, endTimeSpan, safeHours);
        }

        public static bool IsBetweenExclusive(this TimeSpan timeToCheck, TimeSpan startTimeSpan, TimeSpan endTimeSpan,
            int safeHours)
        {
            return timeToCheck.ToLocalTime().IsBetweenExclusive(startTimeSpan, endTimeSpan, safeHours);
        }

        public static bool IsBetweenExclusive(this LocalTime timeToCheck, TimeSpan startTimeSpan, TimeSpan endTimeSpan,
            int safeHours)
        {
            LocalTime startTime = GetDisallowedStart(startTimeSpan, safeHours);
            LocalTime endTime = GetDisallowedEnd(endTimeSpan, safeHours);

            do
            {
                if (startTime.Hour == timeToCheck.Hour)
                    return true;
                startTime = startTime.PlusHours(1);
            } while (startTime.Hour != endTime.Hour);

            return false;
        }

        public static LocalTime GetDisallowedStart(TimeSpan startTimeSpan, int safeHours)
        {
            return startTimeSpan.ToLocalTime().PlusHours(safeHours * -1);
        }

        public static LocalTime GetDisallowedEnd(TimeSpan endTimeSpan, int safeHours)
        {
            return endTimeSpan.ToLocalTime().PlusHours(safeHours);
        }
    }
}
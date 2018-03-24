using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NodaTime;

namespace MiscUnitTests.TimeRange
{
    [DebuggerDisplay("StartTime={StartTime}, EndTime={EndTime}")]
    public class TimeRange
    {
        public TimeRange(LocalTime startTime, LocalTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        public LocalTime StartTime { get; set; }
        public LocalTime EndTime { get; set; }
        public int Id { get; set; }

        /// <summary>
        /// asdf
        /// </summary>
        /// <param name="startHours"></param>
        /// <param name="endHours"></param>
        /// <returns></returns>
        public static TimeRange Create(int startHours, int endHours)
        {
            if (!IsValidHours(startHours))
                throw new ArgumentOutOfRangeException(nameof(startHours),
                    $"{nameof(startHours)} must be between 0 and 23");
            if (!IsValidHours(endHours))
                throw new ArgumentOutOfRangeException(nameof(endHours),
                    $"{nameof(endHours)} must be between 0 and 23");

            return new TimeRange(new LocalTime(startHours, 0, 0), new LocalTime(endHours, 0, 0));
        }

        public static bool IsValidHours(int hours)
        {
            return hours >= 0 && hours <= 23;
        }

        public static int GetNextId(List<TimeRange> ranges)
        {
            if (ranges.Count == 0)
                return 1;
            return ranges.Max(x => x.Id) + 1;
        }

        //public override string ToString() => $"{GetType().Name}: {JsonConvert.SerializeObject(this)}";
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
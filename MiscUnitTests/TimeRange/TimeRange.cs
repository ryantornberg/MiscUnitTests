using System;
using System.Diagnostics;
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

        /// <summary>
        /// asdf
        /// </summary>
        /// <param name="startHours"></param>
        /// <param name="endHours"></param>
        /// <returns></returns>
        public static TimeRange Create(int startHours, int endHours)
        {
            const string validationMsg = "must be between 0 and 23";
            if (!IsValidHours(startHours))
                throw new ArgumentOutOfRangeException(nameof(startHours), $"{nameof(startHours)} {validationMsg}");
            if (!IsValidHours(endHours))
                throw new ArgumentOutOfRangeException(nameof(endHours), $"{nameof(endHours)} {validationMsg}");

            return Create(new LocalTime(startHours, 0, 0), new LocalTime(endHours, 0, 0));
        }

        public static TimeRange Create(LocalTime startTime, LocalTime endTime)
        {
            return new TimeRange(startTime, endTime);
        }

        public static bool IsValidHours(int hours)
        {
            return hours >= 0 && hours <= 23;
        }

        public override bool Equals(object obj)
        {
            return obj is TimeRange range &&
                   StartTime.Equals(range.StartTime) &&
                   EndTime.Equals(range.EndTime);
        }

        protected bool Equals(TimeRange other)
        {
            return StartTime.Equals(other.StartTime) && EndTime.Equals(other.EndTime);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (StartTime.GetHashCode() * 397) ^ EndTime.GetHashCode();
            }
        }
    }
}
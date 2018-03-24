using System;
using System.Globalization;
using NodaTime;

namespace MiscUnitTests.TimeRange
{
    public static class NodaTimeHelper
    {
        #region Methods Returning Noda Objects

        public static LocalDateTime GetNow()
        {
            DateTimeZone tz = DateTimeZoneProviders.Tzdb.GetSystemDefault();
            return GetInstant().InZone(tz).LocalDateTime;
        }

        public static ZonedDateTime GetUtcNow()
        {
            return GetNow().InUtc();
        }

        public static Instant GetInstant()
        {
            return SystemClock.Instance.GetCurrentInstant();
        }

        public static LocalDateTime ToLocalDateTime(this Instant instant, DateTimeZone zone)
        {
            return instant.InZone(zone).LocalDateTime;
        }

        public static LocalDate ToLocalDate(this Instant instant, DateTimeZone zone)
        {
            return instant.InZone(zone).Date;
        }

        public static Instant GetMonthStartInstant(this LocalDate localDate, DateTimeZone timeZone)
        {
            return timeZone.AtStartOfDay(new LocalDate(localDate.Year, localDate.Month, 1)).ToInstant();
        }

        public static Instant GetMonthEndInstant(this LocalDate forDate, DateTimeZone timeZone)
        {
            ZonedDateTime zonedStartDateTime = timeZone.AtStartOfDay(forDate);
            CalendarSystem calendar = zonedStartDateTime.Calendar;
            int daysInMonth = calendar.GetDaysInMonth(zonedStartDateTime.Year, zonedStartDateTime.Month);
            ZonedDateTime zonedEndDateTime =
                timeZone.AtStartOfDay(new LocalDate(zonedStartDateTime.Year, zonedStartDateTime.Month, daysInMonth));
            return zonedEndDateTime.ToInstant();
        }

        public static LocalTime ToLocalTime(this DateTime dateTime)
        {
            return LocalTime.FromTicksSinceMidnight(dateTime.TimeOfDay.Ticks);
        }

        public static LocalTime ToLocalTime(this TimeSpan timeSpan)
        {
            return LocalTime.FromTicksSinceMidnight(timeSpan.Ticks);
        }

        public static LocalDate ToLocalDate(this DateTime dateTime)
        {
            return new LocalDate(dateTime.Year, dateTime.Month, dateTime.Day);
        }

        public static LocalTime ToLocalTime(this LocalDateTime localDateTime)
        {
            return new LocalTime(localDateTime.Hour, localDateTime.Minute);
        }

        public static Period ToPeriod(this TimeSpan timeSpan)
        {
            return Period.FromTicks(timeSpan.Ticks);
        }

        public static DateTimeZone GetLocalTimeZone()
        {
            return DateTimeZoneProviders.Tzdb.GetSystemDefault();
        }


        /// <summary>
        /// https://stackoverflow.com/questions/16674008/nodatime-conversions-part-2-how-to
        /// </summary>
        /// <param name="zone"></param>
        /// <param name="dateTimeUtc"></param>
        /// <returns></returns>
        public static LocalDateTime ToLocalDateTimeFromUtc(this DateTimeZone zone, DateTime dateTimeUtc)
        {
            dateTimeUtc = dateTimeUtc.EnsureKind(DateTimeKind.Utc);
            // for responses
            Instant instant = Instant.FromDateTimeUtc(dateTimeUtc);
            return ToLocalDateTime(instant, zone);
        }

        #endregion

        #region Methods Returning C# Native Objects

        public static DateTime ToDateTime(this LocalDate localDate)
        {
            return new DateTime(localDate.Year, localDate.Month, localDate.Day);
        }

        public static DateTime ToDateTime(this LocalDateTime localDateTime)
        {
            return localDateTime.Date.ToDateTime().AddTicks(localDateTime.TickOfDay);
        }

        public static DateTime EnsureKind(this DateTime dateTime, DateTimeKind kind)
        {
            return dateTime.Kind != kind ? DateTime.SpecifyKind(dateTime, kind) : dateTime;
        }

        public static DateTime EnsureUtc(this DateTime dateTime)
        {
            return dateTime.EnsureKind(DateTimeKind.Utc);
        }

        /// <summary>
        /// https://stackoverflow.com/questions/16674008/nodatime-conversions-part-2-how-to
        /// </summary>
        /// <param name="zone"></param>
        /// <param name="localDateTime"></param>
        /// <returns></returns>
        public static DateTime ToDateTimeUtc(this DateTimeZone zone, LocalDateTime localDateTime)
        {
            //for requests
            return zone.AtStrictly(localDateTime).ToDateTimeUtc();
        }

        public static DateTime ToDateTime(this LocalTime localTime)
        {
            //for requests
            return GetNow().Date.ToDateTime().Date.AddTicks(localTime.TickOfDay);
        }

        public static DateTime ToDateUtc(this DateTimeZone zone, LocalDate localDate)
        {
            //for requests
            return zone.AtStartOfDay(localDate).ToDateTimeUtc().Date;
        }

        public static TimeSpan ToTimeSpan(this LocalTime localTime)
        {
            return new TimeSpan(localTime.TickOfDay);
        }

        #endregion

        #region ToStartOfMonth

        public static DateTime ToStartOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        public static LocalDate ToStartOfMonth(this LocalDate localDate)
        {
            return new LocalDate(localDate.Year, localDate.Month, 1);
        }

        public static LocalDate ToStartOfMonth(this LocalDateTime localDateTime)
        {
            return localDateTime.Date.ToStartOfMonth();
        }

        #endregion

        #region Formatting

        public static string FormatTime(this LocalTime localTime)
        {
            return localTime.ToString("hh:mm tt", CultureInfo.CurrentCulture);
        }

        public static string FormatTime(this LocalDateTime localDateTime)
        {
            return localDateTime.TimeOfDay.FormatTime();
        }

        public static string FormatTimeLight(this LocalTime localTime)
        {
            int hours = localTime.Hour;
            int minutes = localTime.Minute;

            if (hours == 0)
                hours = 24;

            string minutesDisplay = minutes > 0 ? $":{minutes}" : "";
            string meridiem = hours >= 12 ? "pm" : "am";

            string hoursDisplay = hours > 12 ? (hours - 12).ToString() : hours.ToString();

            if (minutes == 0)
            {
                switch (hours)
                {
                    case 12:
                        hoursDisplay = "noon";
                        meridiem = "";
                        break;
                    case 24:
                        hoursDisplay = "midnight";
                        meridiem = "";
                        break;
                }
            }

            string finalDisplay = hoursDisplay + minutesDisplay + meridiem;
            return finalDisplay;
        }

        public static string FormatTimeLight(this LocalDateTime localDateTime)
        {
            return localDateTime.TimeOfDay.FormatTimeLight();
        }

        public static string FormatDateTime(this LocalDateTime localDateTime)
        {
            return localDateTime.ToString("MM/dd/yyyy hh:mm tt", CultureInfo.CurrentCulture);
        }

        public static string FormatDate(this LocalDateTime localDateTime)
        {
            return localDateTime.Date.FormatDate();
        }

        public static string FormatDate(this LocalDate localDate)
        {
            return localDate.ToString("MM/dd/yyyy", CultureInfo.CurrentCulture);
        }

        public static string FormatDate(this DateTime dateTime)
        {
            return dateTime.ToString("MM/dd/yyyy", CultureInfo.CurrentCulture);
        }

        public static string FormatLongDate(this DateTime dateTime)
        {
            return string.Format("{0:MMMM d, yyyy}", dateTime);
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;

namespace EmployeeMealReport.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime FirstDayOfMonth(this DateTime @ref)
        {
            return new DateTime(@ref.Year,@ref.Month,1);
        }
        public static DateTime LastDayOfMonth(this DateTime @ref)
        {
            return new DateTime(@ref.Year,@ref.Month,DateTime.DaysInMonth(@ref.Year,@ref.Month));
        }

        public static IEnumerable<DateTime> GetDateRange(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
                throw new ArgumentException("endDate must be greater than or equal to startDate");

            while (startDate <= endDate)
            {
                yield return startDate;
                startDate = startDate.AddDays(1);
            }
        }

        public static bool IsBetween(this DateTime date,DateTime start, DateTime end)
        {
	        return date >= start && date <= end;
        }

    }
}

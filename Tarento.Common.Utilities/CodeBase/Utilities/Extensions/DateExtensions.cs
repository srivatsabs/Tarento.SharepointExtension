using System;

namespace Tarento.Common.Utilities
{
    /// <summary>
    /// Extension class for date time data
    /// </summary>
    /// <remarks></remarks>
    public static class DateExtensions
    {
        /// <summary>
        /// Determines whether [is date time] [the specified string_date].
        /// </summary>
        /// <param name="string_date">The string_date.</param>
        /// <returns><c>true</c> if [is date time] [the specified string_date]; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public static bool IsDateTime(this string string_date)
        {
            return IsDateTime(string_date, false);
        }

        /// <summary>
        /// Determines whether [is date time] [the specified string_date].
        /// </summary>
        /// <param name="string_date">The string_date.</param>
        /// <param name="throw_error">if set to <c>true</c> [show_error].</param>
        /// <returns><c>true</c> if [is date time] [the specified string_date]; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public static bool IsDateTime(this string string_date, bool throw_error)
        {
            try
            {
                DateTime dt = DateTime.Parse(string_date);
                if (dt != DateTime.MinValue && dt != DateTime.MaxValue)
                {
                    return true;
                }
            }
            catch (ArgumentNullException ex)
            {
                if (throw_error) 
                { 
                    throw ex; 
                }
            }
            catch (FormatException ex)
            {
                if (throw_error) 
                {
                    throw ex; 
                }
            }
            return false;
        }

        /// <summary>
        /// Converts a string to a date time.
        /// </summary>
        /// <param name="string_datetime">The datetime string.</param>
        /// <returns>a data time object representation of the string that was passed</returns>
        /// <remarks></remarks>
        public static DateTime ToDateTime(this string string_datetime)
        {
            try
            {
                if (string_datetime.IsDateTime(true))
                {
                    return Convert.ToDateTime(string_datetime);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return default(DateTime);
        }

        /// <summary>
        /// Creates a timestamp in "YYMMDDHHMMSSS" format and returns it as a string
        /// </summary>
        /// <param name="date_time">date time object</param>
        /// <returns>the timestamp string</returns>
        /// <remarks>Helpful and more informational then a plain hash and guarenteed unique</remarks>
        public static string GetTimestamp(this DateTime date_time)
        {
            return date_time.ToString("yyyyMMddHHmmsssffffff");
        }
    }
}

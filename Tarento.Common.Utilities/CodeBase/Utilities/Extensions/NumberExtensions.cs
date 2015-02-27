
using System;
using System.Globalization;

namespace Tarento.Common.Utilities
{
    /// <summary>
    /// Exttension classes for numbers.
    /// </summary>
    /// <remarks></remarks>
    public static class NumberExtensions
    {
        /// <summary>
        /// Converts the value to a double.
        /// </summary>
        /// <param name="string_double">The string double.</param>
        /// <returns>value cast to a double</returns>
        /// <remarks></remarks>
        public static double ToDouble(this string string_double)
        {
            if (!string.IsNullOrEmpty(string_double))
            {
                if (string_double.IsNumeric())
                {
                    try
                    {
                        return Double.Parse(string_double, CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        throw new OverflowException("Value is greater or less than the maximum allowable value for a double number type.");
                    }
                }
                else
                {
                    throw new ArgumentException("Value is not numeric");
                }
            }
            else
            {
                throw new ArgumentNullException("Value is empty or null");
            }
        }

        /// <summary>
        /// Determines whether the specified string_number is numeric.
        /// </summary>
        /// <param name="string_number">The string_number.</param>
        /// <returns><c>true</c> if the specified string_number is numeric; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public static bool IsNumeric(this string string_number)
        {
            double dbl = 0;
            return double.TryParse(string_number, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out dbl);
        }

        /// <summary>
        /// Converts the value to a int.
        /// </summary>
        /// <param name="string_int">The string int.</param>
        /// <returns>value cast to an int</returns>
        /// <remarks></remarks>
        public static int ToInt(this string string_int)
        {
            if (string_int.IsNumeric())
            {
                if ((double)Int32.Parse(string_int, CultureInfo.InvariantCulture) <= (double)Int32.MaxValue &&
                    (double)Int32.Parse(string_int, CultureInfo.InvariantCulture) >= (double)Int32.MinValue)
                {
                    return Int32.Parse(string_int, CultureInfo.InvariantCulture);
                }
                else
                {
                    throw new OverflowException(string.Format("Value {0} was either too large or too small for an Int32.", string_int.ToSafeString()));
                }
            }
            else
            {
                throw new ArgumentException(string.Format("Value {0} is not numeric.", string_int.ToSafeString()));
            }
        }

        /// <summary>
        /// Converts the value to a int.
        /// </summary>
        /// <param name="object_int">The object_int.</param>
        /// <param name="treat_null_as_zero">if set to <c>true</c> [treat_null_as_zero].</param>
        /// <returns>value cast to an int</returns>
        /// <remarks></remarks>
        public static int ToInt(this object object_int, bool treat_null_as_zero)
        {
            string string_int = object_int.ToSafeString();
            if (treat_null_as_zero && string.IsNullOrEmpty(string_int)) 
            { 
                return 0; 
            }
            
            if (string_int.IsNumeric())
            {
                if ((double)Int32.Parse(string_int, CultureInfo.InvariantCulture) <= (double)Int32.MaxValue &&
                    (double)Int32.Parse(string_int, CultureInfo.InvariantCulture) >= (double)Int32.MinValue)
                {
                    return Int32.Parse(string_int, CultureInfo.InvariantCulture);
                }
                else
                {
                    throw new OverflowException(string.Format("Value {0} was either too large or too small for an Int32.", object_int.ToSafeString()));
                }
            }
            else
            {
                throw new ArgumentException(string.Format("Value {0} is not numeric.", object_int.ToSafeString()));
            }
        }
    }
}

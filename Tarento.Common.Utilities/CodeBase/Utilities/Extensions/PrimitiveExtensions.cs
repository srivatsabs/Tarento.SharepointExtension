using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace Tarento.Common.Utilities
{
    /// <summary>
    /// Extensions that use simple base types like string
    /// </summary>
    /// <remarks></remarks>
    public static class PrimitiveExtensions
    {
        /// <summary>
        /// expands upon the conversion of ToString() by checking for null and returning an empty string is it is the result
        /// </summary>
        /// <param name="object_value">The object value</param>
        /// <returns>the object as a string value or an empty string if the value passed was null</returns>
        /// <remarks></remarks>
        public static string ToSafeString(this object object_value)
        {
            if (object_value == null) 
            {
                return ""; 
            }
            return object_value.ToString();
        }

        /// <summary>
        /// Changes the passed string to the culture proper case
        /// </summary>
        /// <param name="string_to_change">The string to change</param>
        /// <returns>proper cased string</returns>
        /// <remarks></remarks>
        public static string ToProperCase(this string string_to_change)
        {
            return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(string_to_change.ToLower());
        }

        /// <summary>
        /// Removes empty spaces in a string if it exists.
        /// </summary>
        /// <param name="str">The string to examine</param>
        /// <returns>the modified string</returns>
        /// <remarks></remarks>
        public static string RemoveSpacesIfExist(this string str)
        {
            return str.Replace(" ",string.Empty);            
        }

        /// <summary>
        /// Determines whether the specified object_value is nothing.
        /// </summary>
        /// <param name="object_value">The object_value.</param>
        /// <returns><c>true</c> if the specified object_value is nothing; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public static bool IsNothing(this object object_value)
        {
            if (object_value == null) 
            { 
                return true; 
            }
            return false;
        }

        /// <summary>
        /// Strings to byte array.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>An array of bytes representing the passed string</returns>
        /// <remarks></remarks>
        public static byte[] StringToByteArray(this string value)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(value);
        }

        /// <summary>
        /// Appends the string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="appendString">The append string.</param>
        /// <returns>string value</returns>
        /// <remarks></remarks>
        public static string AppendString(this string input, string appendString)
        {
            string retString = input;
            if (!String.IsNullOrEmpty(input) && String.IsNullOrEmpty(appendString))
            {
                retString = input + " " + appendString;
            }
            else if (String.IsNullOrEmpty(input))
            {
                retString = appendString;
            }
            return retString;
        }

        /// <summary>
        /// Determines whether the specified string_guid is GUID.
        /// </summary>
        /// <param name="string_guid">The string_guid.</param>
        /// <returns><c>true</c> if the specified string_guid is GUID; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public static bool IsGuid(this string string_guid)
        {
            return Regex.IsMatch(StripGuid(string_guid), @"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$");
        }

        /// <summary>
        /// Determines whether the specified object_bool is bool.
        /// </summary>
        /// <param name="object_bool">The object_bool.</param>
        /// <returns><c>true</c> if the specified object_bool is bool; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public static bool IsBool(this object object_bool)
        {
            if (object_bool != null)
            {
                string string_bool = object_bool.ToSafeString();
                string_bool = string_bool.Trim().ToLower();
                if (string_bool == "true" || string_bool == "false" || string_bool == "1" || string_bool == "0") 
                { 
                    return true; 
                }
            }
            return false;
        }

        /// <summary>
        /// Strips the GUID.
        /// </summary>
        /// <param name="string_guid">The string_guid.</param>
        /// <returns>A stripped guid</returns>
        /// <remarks></remarks>
        private static string StripGuid(string string_guid)
        {
            try
            {
                if (!string.IsNullOrEmpty(string_guid))
                {
                    string_guid = string_guid.Replace("{", String.Empty).Replace("}", String.Empty);
                }
            }
            catch { }
            return string_guid;
        }

        /// <summary>
        /// Converts the value to a bool
        /// </summary>
        /// <param name="string_bool">The string_bool.</param>
        /// <returns>the bool, if the value is not boolean, returns false</returns>
        /// <remarks></remarks>
        public static bool ToBool(this string string_bool)
        {
            if (string_bool.IsBool())
            {
                return Convert.ToBoolean(string_bool.ToLowerInvariant());
            }
            return default(bool);
        }

        /// <summary>
        /// Strips the HTML from a string
        /// </summary>
        /// <param name="html_string">The html string.</param>
        /// <returns>string value of the html</returns>
        /// <remarks></remarks>
        public static string StripHTML(this string html_string)
        {
            char[] array = new char[html_string.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < html_string.Length; i++)
            {
                char let = html_string[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }

        /// <summary>
        /// Truncates the HTML given an amount of words to return
        /// </summary>
        /// <param name="input">The input string</param>
        /// <param name="limit">The limit of words to return</param>
        /// <returns>a number of words based on the input and the limit</returns>
        /// <remarks></remarks>
        public static string TruncateHtml(this string input, int limit)
        {
            //replace newlines and tabs
            input = input.Replace("\t", " ").Replace("\r", " ").Replace("\n", " ");

            //condense white space
            while (input.IndexOf("  ") != -1)
            {
                input = input.Replace("  ", " ");
            }

            int endPoint = -1, startPoint = -1, idx = 0, counter = 0;
            bool Continue = true;
            ArrayList list = new ArrayList();
            string str = string.Empty;
            while (Continue)
            {
                do
                {
                    counter++;
                    if (counter > 0x3e7)
                    {
                        str = "Error: HTML too complex!";
                        Continue = false;
                        break;
                        //goto Label_01BC;
                    }

                    endPoint = input.IndexOf("<", (int)(startPoint + 1));
                    if (endPoint == -1) 
                    { 
                        endPoint = input.Length; 
                    }

                    if ((idx + HtmlLengthBetween(input, startPoint + 1, endPoint)) >= limit)
                    {
                        int length = limit - idx;
                        str = input.Substring(0, startPoint + 1) + HtmlSubString(input, startPoint + 1, length);
                        Continue = false;
                        break;
                        //goto Label_01BC;
                    }
                    if (endPoint == input.Length)
                    {
                        str = input;
                        Continue = false;
                        break;
                        //goto Label_01BC;
                    }
                    idx += HtmlLengthBetween(input, startPoint + 1, endPoint);
                    startPoint = input.IndexOf(">", endPoint);
                    if (startPoint == -1)
                    {
                        str = input.Substring(0, endPoint);
                        Continue = false;
                        break;
                        //goto Label_01BC;
                    }
                }
                while (input[startPoint - 1] == '/');
                if (Continue)
                {
                    int index = input.IndexOf(" ", endPoint);
                    if ((index == -1) || (index > startPoint))
                    {
                        index = startPoint;
                    }
                    string val = input.Substring(endPoint + 1, (index - endPoint) - 1);
                    if (val.StartsWith("/"))
                    {
                        val = val.Substring(1);
                        int lidx = list.LastIndexOf(val);
                        list.RemoveRange(lidx, list.Count - lidx);
                    }
                    else
                    {
                        list.Add(val);
                    }
                }
            }
        //Label_01BC:
            str += "... ";
            list.Reverse();
            foreach (string value in list)
            {
                str += "</" + value + ">";
            }
            return str;
        }

        #region private static methods
        /// <summary>
        /// Returns the length between a start and end point
        /// </summary>
        /// <param name="input">The input string</param>
        /// <param name="start">The start point</param>
        /// <param name="end">The end point</param>
        /// <returns>integer</returns>
        /// <remarks>used internally by truncate HTML</remarks>
        private static int HtmlLengthBetween(string input, int start, int end)
        {
            string str = input.Substring(start, end - start).Trim();
            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }
            return HttpUtility.HtmlDecode(str).Length;
        }

        /// <summary>
        /// Returns a substring given a start and end point in decoded HTML
        /// </summary>
        /// <param name="input">The input string</param>
        /// <param name="start">The start point</param>
        /// <param name="length">The length to return</param>
        /// <returns>string value</returns>
        /// <remarks>used internally by truncate HTML</remarks>
        private static string HtmlSubString(string input, int start, int length)
        {
            return HttpUtility.HtmlEncode(HttpUtility.HtmlDecode(input.Substring(start).Trim()).Substring(0, length));
        }
        #endregion
    }
}


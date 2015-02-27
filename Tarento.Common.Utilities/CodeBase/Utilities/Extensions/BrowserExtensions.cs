using System;
using System.Runtime.CompilerServices;
using System.Web;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;

namespace Tarento.Common.Utilities
{
    /// <summary>
    /// Esxtensions to assist in the discovery of the browser type and version currently being used
    /// </summary>
    /// <remarks></remarks>
    public static class BrowserExtensions
    {
        /// <summary>
        /// Return the current version of the user's broswer
        /// </summary>
        /// <param name="context">Pass the <see cref="Microsoft.SharePoint.SPContext.Current"/> object</param>
        /// <returns>An integer that indicates the major version of the browser</returns>
        /// <remarks></remarks>
        /// <example>BrowserVersion example
        /// <code>
        /// int version = SPContext.Current.BrowserVersion();
        /// </code>
        /// </example>
        public static int BrowserVersion(this SPContext context)
        {
            if (context != null && context is SPContext)
            {
                return HttpContext.Current.Request.Browser.MajorVersion;
            }
            throw new ArgumentException("The context is not available");
        }

        /// <summary>
        /// Determines whether the current user browser is IE using [the specified context].
        /// </summary>
        /// <param name="context">Pass the <see cref="Microsoft.SharePoint.SPContext.Current"/> object</param>
        /// <returns><c>true</c> if the broswer is Internet Explorer otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        /// <example>IsBrowserIE example
        /// <code>
        /// if(SPContext.Current.IsBrowserIE())
        /// {
        ///     //do something
        /// }
        /// </code>
        /// </example>
        public static bool IsBrowserIE(this SPContext context)
        {
            if (context != null && context is SPContext)
            {
                int major_version = 0;
                double minor_version = 0.0;
                return IsBrowserIE(context, major_version, minor_version);
            }
            else
            {
                throw new ArgumentNullException("The context was null");
            }
        }

        /// <summary>
        /// Determines whether the browser is the correct version of IE.
        /// </summary>
        /// <param name="context">Pass the <see cref="Microsoft.SharePoint.SPContext.Current"/> object</param>
        /// <param name="major_version">The major version</param>
        /// <returns><c>true</c> if [is browser IE] [the specified context]; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        /// <example>IsBrowserIE example
        /// <code>
        /// if(SPContext.Current.IsBrowserIE(8))
        /// {
        ///     //do something
        /// }
        /// </code>
        /// </example>
        public static bool IsBrowserIE(this SPContext context, int major_version)
        {
            if (context != null)
            {
                double minor_version = 0.0;
                return IsBrowserIE(context, major_version, minor_version);
            }
            else
            {
                throw new ArgumentNullException("The context was null");
            }
        }

        /// <summary>
        /// Determines whether if the browser if the specified version of IE
        /// </summary>
        /// <param name="context">Pass the <see cref="Microsoft.SharePoint.SPContext.Current"/> object</param>
        /// <param name="major_version">The major version to match</param>
        /// <param name="minor_version">The minor version to match</param>
        /// <returns><c>true</c> if [is browser IE] [the specified context]; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        /// <example>IsBrowserIE example
        /// <code>
        /// if(SPContext.Current.IsBrowserIE(8, 1))
        /// {
        ///     //do something
        /// }
        /// </code>
        /// </example>
        public static bool IsBrowserIE(this SPContext context, int major_version, double minor_version)
        {
            bool flag = false;
            if (!context.IsNothing())
            {
                HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
                if (!browser.IsNothing() && (browser.Type.IndexOf("IE") >= 0) && browser.Win32)
                {
                    flag = true;

                    if ((major_version != 0) && major_version != browser.MajorVersion)
                    {
                        flag = false;
                    }

                    if ((minor_version != 0.0) && minor_version != browser.MinorVersion)
                    {
                        flag = false;
                    }
                }
            }
            return flag;
        }

        /// <summary>
        /// Appends the slash if it does not exist.
        /// </summary>
        /// <param name="str">The string to examine</param>
        /// <returns>the modified string</returns>
        /// <remarks></remarks>
        public static string AppendSlashIfDoesNotExist(this string str)
        {
            if (String.IsNullOrEmpty(str) || str.EndsWith("/"))
            {
                return str;
            }
            return str + "/";

        }

        /// <summary>
        /// Removes the slash if it exists.
        /// </summary>
        /// <param name="str">The string to examine</param>
        /// <returns>the modified string</returns>
        /// <remarks></remarks>
        public static string RemoveSlashIfExists(this string str)
        {
            if (String.IsNullOrEmpty(str) || !str.EndsWith("/"))
            {
                return str;
            }
            return str.TrimEnd(new char[] { '/' });
        }

        /// <summary>
        /// Encodes the URL
        /// </summary>
        /// <param name="stringToEncode">The url string to encode.</param>
        /// <returns>An encode url string</returns>
        /// <remarks></remarks>
        public static string UrlEncode(this string stringToEncode)
        {
            return SPHttpUtility.UrlPathEncode(stringToEncode, false);
        }

        /// <summary>
        /// Decodes a URL.
        /// </summary>
        /// <param name="stringToDecode">The url string to decode.</param>
        /// <returns>A decoded url string</returns>
        /// <remarks></remarks>
        public static string UrlDecode(this string stringToDecode)
        {
            return SPHttpUtility.UrlPathDecode(stringToDecode, false);
        }
    }
}
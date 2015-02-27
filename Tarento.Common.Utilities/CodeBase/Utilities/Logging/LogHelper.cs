using System.Runtime.CompilerServices;
using System.Security.Permissions;
using Microsoft.Practices.SharePoint.Common.Logging;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Security;

namespace Tarento.Common.Utilities
{
    
    /// <summary>
    /// Contains methods and properties to assist in internal messaging to the ULS logs
    /// </summary>
    /// <remarks></remarks>
    public static class LogHelper
    {
        /// <summary>
        /// The default area/category for for internal messages.
        /// </summary>
        public static string DefaultAreaCategory
        {
            get
            {
                return Constants.DefaultAreaName + Constants.CategoryPathSeparator + Constants.DefaultCategoryName;
            }
        }

        /// <summary>
        /// The default area/ for for internal messages.
        /// </summary>
        public static string DefaultArea
        {
            get
            {
                return Constants.DefaultAreaName + Constants.CategoryPathSeparator;
            }
        }

        /// <summary>
        /// Gets the default event id.
        /// </summary>
        /// <remarks>Only to be used internally</remarks>
        public static int DefaultEventId
        {
            get
            {
                return Constants.DefaultEventId;
            }
        }

        /// <summary>
        /// Sends a trace to the ULS logs using the passed parameters
        /// </summary>
        /// <param name="message">The message to add to the ULS logs</param>
        /// <param name="eventId">The event id to use</param>
        /// <param name="category">The category to use</param>
        /// <remarks></remarks>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public static void SendTrace(string message, int eventId, string category)
        {
            DiagnosticsService.Local.LogTrace(message, eventId, category);
        }

        /// <summary>
        /// Sends a trace to the ULS logs using the passed parameters
        /// </summary>
        /// <param name="message">The message to add to the ULS logs</param>
        /// <param name="eventId">The event id to use</param>
        /// <param name="severity">The severity of the event (TraceSeverity)</param>
        /// <param name="category">The category to use</param>
        /// <remarks></remarks>
        [SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true)]
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true)]
        public static void SendTrace(string message, int eventId, TraceSeverity severity, string category)
        {
            DiagnosticsService.Local.LogTrace(message, eventId, severity, category);
        }
    }
}

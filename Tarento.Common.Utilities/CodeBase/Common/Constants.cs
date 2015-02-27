using System.Runtime.CompilerServices;

namespace Tarento.Common.Utilities
{
    

    /// <summary>
    /// Class that holds the constants for the SharePoint.Common project.
    /// </summary>
    /// <remarks></remarks>
    public static class Constants
    {
        /// <summary>
        /// Constant that specifies the default category name.
        /// </summary>
        public static readonly string DefaultCategoryName = "Tarento SharePoint Common Library";

        /// <summary>
        /// Constant that defines the default area name.
        /// </summary>
        public static readonly string CachingDiagnosticCategoryName = "Tarento Caching Process";

        /// <summary>
        /// Constant that defines the default area name.
        /// </summary>
        public static readonly string DefaultAreaName = "Tarento Core Framework";

        /// <summary>
        /// Constant that specifies the name of the configuration key for searching the areas and categories section.
        /// </summary>
        public static readonly string AreasConfigKey = "Tarento.SharePoint.Diagnostics";


        /// <summary>
        /// Constant that specifies the name of the event log in which the event sources will be created for logging.
        /// </summary>
        public static readonly string EventLogName = "Tarento SharePoint Event Log";

        /// <summary>
        /// Constant that specifies the name of the event log in which the event sources will be created for logging.
        /// </summary>
        public static readonly int DefaultEventId = 0;

        /// <summary>
        /// Constant that specifies the name of the event log in which the event sources will be created for logging.
        /// </summary>
        public static readonly string InternalLogIdentifier = "Tarento Common Process";

        /// <summary>
        /// Constant that defines the path separator for categories (Area/Category).
        /// </summary>
        public static readonly char CategoryPathSeparator = '/';

        
    }
}

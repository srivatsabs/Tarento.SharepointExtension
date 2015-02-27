
using System;
using Microsoft.Practices.SharePoint.Common.Logging;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;
using Microsoft.SharePoint.Administration;

namespace Tarento.Common.Utilities.Logging
{
    /// <summary>
    /// A timer job that ensures the event logs on the WFEs are in synch.
    /// </summary>
    /// <remarks></remarks>
    public class DiagnosticsSourcesJob : SPJobDefinition
    {
        /// <summary>
        /// </summary>
        /// <remarks></remarks>
        public DiagnosticsSourcesJob() : base() { }

        /// <summary>
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="webApplication">The web application.</param>
        /// <remarks></remarks>
        public DiagnosticsSourcesJob(string name, SPWebApplication webApplication)
            : base(name, webApplication, null, SPJobLockType.None)
        {
            this.Title = "Ensure Diagnostic Event Sources";
        }

        /// <summary>
        /// Executes the job definition.
        /// </summary>
        /// <param name="targetInstanceId">For target types of <see cref="T:Microsoft.SharePoint.Administration.SPContentDatabase"/> this is the database ID of the content database being processed by the running job. This value is Guid.Empty for all other target types.</param>
        /// <remarks></remarks>
        public override void Execute(Guid targetInstanceId)
        {
            try
            {
                ILogger logger = SharePointServiceLocator.GetCurrent().GetInstance<ILogger>();
                LogHelper.SendTrace("Start Processing ensure sources job.", 103, TraceSeverity.Verbose, LogHelper.DefaultAreaCategory);
                if (this.LockType == SPJobLockType.None)
                {
                    DiagnosticsAreaEventSource.EnsureConfiguredAreasRegistered();
                }
            }
            catch (Exception ex)
            {
                LogHelper.SendTrace("Exception occurred processing ensure sources job: " + ex.Message, 103, TraceSeverity.High, LogHelper.DefaultAreaCategory);
            }
        }
    }
}

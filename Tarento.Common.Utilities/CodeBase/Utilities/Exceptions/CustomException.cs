using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Tarento.Common.Utilities
{
    /// <summary>     
    
    /// </summary>     
    [CompilerGenerated]
    internal class NamespaceDoc { }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class CustomException : Exception
    {
        private string _additionalInformation;

        /// <summary>
        /// Gets the additional information.
        /// </summary>
        /// <remarks></remarks>
        public string AdditionalInformation
        {
            get
            {
                return Microsoft.SharePoint.Utilities.SPEncode.HtmlEncode(this._additionalInformation);
            }
        }

        /// <summary>
        /// Gets the display message.
        /// </summary>
        /// <remarks></remarks>
        public string DisplayMessage { get; internal set; }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        /// <returns>
        /// The error message that explains the reason for the exception, or an empty string("").
        ///   </returns>
        /// <remarks></remarks>
        public override string Message
        {
            get
            {
                if (!string.IsNullOrEmpty(this.DisplayMessage))
                {
                    return this.DisplayMessage;
                }
                return base.Message;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        public CustomException()
        {
            this.DisplayMessage = string.Empty;
            this._additionalInformation = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <remarks></remarks>
        public CustomException(string message)
            : base(message)
        {
            this.DisplayMessage = string.Empty;
            this._additionalInformation = string.Empty;
            this.DisplayMessage = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomException"/> class.
        /// </summary>
        /// <param name="baseException">The base exception.</param>
        /// <param name="additionalInformation">The additional information.</param>
        /// <remarks></remarks>
        public CustomException(Exception baseException, string additionalInformation)
            : base(baseException.Message, baseException)
        {
            this.DisplayMessage = string.Empty;
            this._additionalInformation = string.Empty;
            this._additionalInformation = additionalInformation;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="baseException">The base exception.</param>
        /// <remarks></remarks>
        public CustomException(string message, Exception baseException)
            : base(message, baseException)
        {
            this.DisplayMessage = string.Empty;
            this._additionalInformation = string.Empty;
            this.DisplayMessage = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="baseException">The base exception.</param>
        /// <param name="additionalInformation">The additional information.</param>
        /// <remarks></remarks>
        public CustomException(string message, Exception baseException, string additionalInformation)
            : base(message, baseException)
        {
            this.DisplayMessage = string.Empty;
            this._additionalInformation = string.Empty;
            this.DisplayMessage = message;
            this._additionalInformation = additionalInformation;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="baseException">The base exception.</param>
        /// <param name="additionalInformation">The additional information.</param>
        /// <param name="trace">The trace.</param>
        /// <remarks></remarks>
        public CustomException(string message, Exception baseException, string additionalInformation, StackTrace trace)
            : base(message, baseException)
        {
            this.DisplayMessage = string.Empty;
            this._additionalInformation = string.Empty;
            this.DisplayMessage = message;
            this._additionalInformation = additionalInformation;
        }
    }

    /// <summary>
    /// Exception extension for missing settings
    /// </summary>
    /// <remarks></remarks>
    [Serializable]
    public class MissingSettingException : CustomException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MissingSettingException"/> class.
        /// </summary>
        /// <remarks></remarks>
        public MissingSettingException()
        {
            this.DisplayMessage = string.Format(Tarento.Common.Utilities.Properties.Resources.MissingSettingExceptionMessage,
                Tarento.Common.Utilities.Properties.Resources.MissingSettingExceptionTitle, "");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MissingSettingException"/> class.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <remarks></remarks>
        public MissingSettingException(string setting)
        {
            this.DisplayMessage = string.Format(Tarento.Common.Utilities.Properties.Resources.MissingSettingExceptionMessage,
                Tarento.Common.Utilities.Properties.Resources.MissingSettingExceptionTitle, ": " + setting);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MissingSettingException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="setting">The setting.</param>
        /// <remarks></remarks>
        public MissingSettingException(string message, string setting)
        {
            this.DisplayMessage = string.Format(Tarento.Common.Utilities.Properties.Resources.MissingSettingExceptionMessage, message, ": " + setting);
        }
    }

    /// <summary>
    /// Exception to be used for webparts that have not been configured propertly.
    /// </summary>
    /// <remarks></remarks>
    [Serializable]
    public class NotConfiguredException : CustomException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotConfiguredException"/> class.
        /// </summary>
        /// <remarks></remarks>
        public NotConfiguredException()
        {
            this.DisplayMessage = string.Format(Tarento.Common.Utilities.Properties.Resources.NotConfiguredExceptionNoId, "", "");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotConfiguredException"/> class.
        /// </summary>
        /// <param name="webPartId">The web part id.</param>
        /// <remarks></remarks>
        public NotConfiguredException(string webPartId)
        {
            this.DisplayMessage = string.Format(Tarento.Common.Utilities.Properties.Resources.NotConfiguredExceptionId, "", webPartId);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotConfiguredException"/> class.
        /// </summary>
        /// <param name="webPartId">The web part id.</param>
        /// <param name="message">The message.</param>
        /// <remarks></remarks>
        public NotConfiguredException(string webPartId, string message)
        {
            this.DisplayMessage = string.Format(Tarento.Common.Utilities.Properties.Resources.NotConfiguredExceptionId, ": " + message, webPartId);
        }
    }
}

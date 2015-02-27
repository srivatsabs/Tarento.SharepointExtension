using System;
using System.Runtime.CompilerServices;
using Microsoft.Office.Server.UserProfiles;
using Microsoft.SharePoint;

namespace Tarento.Common.Utilities
{
       
    /// <summary>
    /// Contains methods to assist in the creation of profile property fields
    /// </summary>
    /// <remarks></remarks>
    public class ProfilePropertyHelper
    {
        /// <summary>
        /// Selection of possible data types available for profile properties
        /// </summary>
        /// <remarks></remarks>
        public enum DataType
        {
            /// <summary>
            /// Field is of binary type
            /// </summary>
            Binary,

            /// <summary>
            /// Field is of Boolean type
            /// </summary>
            Boolean,

            /// <summary>
            /// Field is of Date type
            /// </summary>
            Date,

            /// <summary>
            /// Field is of Date type with no year 
            /// </summary>
            DateNoYear,

            /// <summary>
            /// Field is of Date and Time type
            /// </summary>
            DateTime,

            /// <summary>
            /// Field is of email type
            /// </summary>
            Email,

            /// <summary>
            /// Field is of float type (small precision number value)
            /// </summary>
            Float,

            /// <summary>
            /// Field is of unique identifier (GUID) type
            /// </summary>
            Guid,

            /// <summary>
            /// Field is of Rich Text (HTML) type
            /// </summary>
            HTML,

            /// <summary>
            /// Field is of integer (small whole number) type
            /// </summary>
            Integer,

            /// <summary>
            /// Field is of SPUser type
            /// </summary>
            Person,

            /// <summary>
            /// Field is of string type
            /// </summary>
            String,

            /// <summary>
            /// Field is of multi value string type
            /// </summary>
            StringMultiValue,

            /// <summary>
            /// Field is of single value string type
            /// </summary>
            StringSingleValue,

            /// <summary>
            /// Field is of type time zone (special)
            /// </summary>
            TimeZone,

            /// <summary>
            /// Field is of URL type
            /// </summary>
            URL
        }

        /// <summary>
        /// varible for the policies to use for this profile property
        /// </summary>
        private ProfilePropertyPolicy _policy = null;

        /// <summary>
        /// Gets or sets the property manager.
        /// </summary>
        /// <value>The property manager.</value>
        /// <remarks></remarks>
        private ProfilePropertyManager PropertyManager { get; set; }

        /// <summary>
        /// Gets or sets the property policy.
        /// </summary>
        /// <value>The property policy.</value>
        /// <remarks></remarks>
        public ProfilePropertyPolicy PropertyPolicy
        {
            get
            {
                if (this._policy == null)
                {
                    this._policy = new ProfilePropertyPolicy();
                }
                return this._policy;

            }
            set
            {
                this._policy = value;
            }
        }

        /// <summary>
        /// Gets or sets the service context.
        /// </summary>
        /// <value>The service context.</value>
        /// <remarks></remarks>
        private SPServiceContext ServiceContext { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfilePropertyHelper"/> class.
        /// </summary>
        /// <param name="context">The service context.</param>
        /// <remarks></remarks>
        public ProfilePropertyHelper(SPServiceContext context)
        {
            try
            {
                this.ServiceContext = context;
                UserProfileConfigManager configManager = new UserProfileConfigManager(context);
                this.PropertyManager = configManager.ProfilePropertyManager;
            }
            catch (Exception ex)
            {
                LogHelper.SendTrace(
                     string.Format("An exception occurred in the ProfilePropertyHelper initialization routine. Exception: {0}", ex.Message),
                     LogHelper.DefaultEventId,
                     Microsoft.SharePoint.Administration.TraceSeverity.High,
                     LogHelper.DefaultAreaCategory);
            }
        }

        /// <summary>
        /// Adds the specified property to the user profile
        /// </summary>
        /// <param name="propertyName">Internal name of the property</param>
        /// <param name="displayName">The display name of the property</param>
        /// <param name="type">The property data type (From DataType)</param>
        /// <param name="policy">The policy for the property(from ProfilePropertyPolicy)</param>
        /// <param name="length">The length for the field (available on string types)</param>
        /// <param name="isVisibleOnEditor">whether to be shown on edit profile page of user</param>
        /// <returns>true if the property has been created, false if an exception occurred</returns>
        /// <remarks></remarks>
        public bool Add(string propertyName, string displayName, DataType type, ProfilePropertyPolicy policy, int length, bool isVisibleOnEditor)
        {
            this.PropertyPolicy = policy;
            return Add(propertyName, displayName, type, length, isVisibleOnEditor);
        }

        /// <summary>
        /// Adds the specified property to the user profile
        /// </summary>
        /// <param name="propertyName">Internal name of the property</param>
        /// <param name="displayName">The display name of the property</param>
        /// <param name="type">The property data type (From DataType)</param>
        /// <param name="policy">The policy for the property(from ProfilePropertyPolicy</param>
        /// <param name="length">The length for the field (available on string types)</param>
        /// <returns>true if the property has been created, false if an exception occurred</returns>
        /// <example>Using the Add method
        /// <code>
        /// try
        /// {
        ///     ProfilePropertyHelper profileProperty = new ProfilePropertyHelper(context);
        ///     profileProperty.PropertyPolicy.DefaultPrivacy = Privacy.Public;
        ///     profileProperty.PropertyPolicy.IsUserEditable = true;
        ///     profileProperty.PropertyPolicy.CanUserOverride = true;
        ///     profileProperty.PropertyPolicy.UserPolicy = PrivacyPolicy.OptIn;
        ///     if(!profileProperty.Add("CurrentRegion", "Current Region", ProfilePropertyHelper.DataType.StringSingleValue, 50))
        ///     {
        ///         //handle property not created
        ///     }
        /// }
        /// catch (Exception ex)
        /// {
        ///     //handle exception
        /// }
        /// </code>
        /// </example>
        /// <remarks></remarks>
        public bool Add(string propertyName, string displayName, DataType type, ProfilePropertyPolicy policy, int length)
        {
            try
            {
                // create core property
                CorePropertyManager cpm = this.PropertyManager.GetCoreProperties();
                CoreProperty cp = cpm.Create(false);
                cp.Name = propertyName;
                cp.DisplayName = displayName;
                SetPropertyDataType(cp, type, length);
                cpm.Add(cp);

                // create profile type property
                ProfileTypePropertyManager ptpm = this.PropertyManager.GetProfileTypeProperties(ProfileType.User);
                ProfileTypeProperty ptp = ptpm.Create(cp);
                ptpm.Add(ptp);

                // create profile subtype property
                ProfileSubtypeManager psm = ProfileSubtypeManager.Get(this.ServiceContext);
                ProfileSubtype ps = psm.GetProfileSubtype(ProfileSubtypeManager.GetDefaultProfileName(ProfileType.User));
                ProfileSubtypePropertyManager pspm = ps.Properties;
                ProfileSubtypeProperty psp = pspm.Create(ptp);

                psp.UserOverridePrivacy = policy.CanUserOverride;
                psp.IsUserEditable = policy.IsUserEditable;
                psp.DefaultPrivacy = policy.DefaultPrivacy;

                pspm.Add(psp);
                return true;
            }
            catch (DuplicateEntryException)
            {
                LogHelper.SendTrace(
                    string.Format("Attempt to add a profile property named {0} failed because a property using that name already exists.", propertyName),
                    LogHelper.DefaultEventId,
                    Microsoft.SharePoint.Administration.TraceSeverity.Unexpected,
                    LogHelper.DefaultAreaCategory);
            }
            catch (Exception ex)
            {
                LogHelper.SendTrace(
                    string.Format("An exception occurred in ProfilePropertyHelper adding the property {0} to the profile property store. Exception: {1}", propertyName, ex.Message),
                    LogHelper.DefaultEventId,
                    Microsoft.SharePoint.Administration.TraceSeverity.Unexpected,
                    LogHelper.DefaultAreaCategory);
            }
            return false;
        }

        /// <summary>
        /// Adds the specified property name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="type">The type.</param>
        /// <param name="length">The length.</param>
        /// <param name="isVisibleOnEditor">if set to <c>true</c> [is visible on editor].</param>
        /// <returns>true if the property was added to the profile properties</returns>
        /// <remarks></remarks>
        public bool Add(string propertyName, string displayName, DataType type, int length, bool isVisibleOnEditor)
        {
            try
            {
                // create core property
                CorePropertyManager cpm = this.PropertyManager.GetCoreProperties();
                CoreProperty cp = cpm.Create(false);
                cp.Name = propertyName;
                cp.DisplayName = displayName;
                SetPropertyDataType(cp, type, length);
                cpm.Add(cp);

                // create profile type property
                ProfileTypePropertyManager ptpm = this.PropertyManager.GetProfileTypeProperties(ProfileType.User);
                ProfileTypeProperty ptp = ptpm.Create(cp);
                ptp.IsVisibleOnEditor = isVisibleOnEditor;
                ptpm.Add(ptp);

                // create profile subtype property
                ProfileSubtypeManager psm = ProfileSubtypeManager.Get(this.ServiceContext);
                ProfileSubtype ps = psm.GetProfileSubtype(ProfileSubtypeManager.GetDefaultProfileName(ProfileType.User));
                ProfileSubtypePropertyManager pspm = ps.Properties;
                ProfileSubtypeProperty psp = pspm.Create(ptp);

                psp.PrivacyPolicy = this.PropertyPolicy.UserPolicy;
                psp.UserOverridePrivacy = this.PropertyPolicy.CanUserOverride;
                psp.IsUserEditable = this.PropertyPolicy.IsUserEditable;
                psp.DefaultPrivacy = this.PropertyPolicy.DefaultPrivacy;

                pspm.Add(psp);
                return true;
            }
            catch (DuplicateEntryException)
            {
                LogHelper.SendTrace(
                    string.Format("Attempt to add a profile property named {0} failed because a property using that name already exists.", propertyName),
                    LogHelper.DefaultEventId,
                    Microsoft.SharePoint.Administration.TraceSeverity.Unexpected,
                    LogHelper.DefaultAreaCategory);
            }
            catch (Exception ex)
            {
                LogHelper.SendTrace(
                    string.Format("An exception occurred in ProfilePropertyHelper adding the property {0} to the profile property store. Exception: {1}", propertyName, ex.Message),
                    LogHelper.DefaultEventId,
                    Microsoft.SharePoint.Administration.TraceSeverity.Unexpected,
                    LogHelper.DefaultAreaCategory);
            }
            return false;
        }

        /// <summary>
        /// Check if user profile property exists already
        /// </summary>
        /// <param name="propertyName">property name to be checked</param>
        /// <returns>tru if property exist, false otherwise</returns>
        public bool IsUserProperty(string propertyName)
        {
            try
            {
                // create core property
                CorePropertyManager cpm = this.PropertyManager.GetCoreProperties();
                CoreProperty cp = cpm.GetPropertyByName(propertyName);
                return cp != null ? true : false;
            }
            catch (Exception ex)
            {
                LogHelper.SendTrace(
                    string.Format("An exception occurred in IsUserProperty checking property {0}. Exception: {1}", propertyName, ex.Message),
                    LogHelper.DefaultEventId,
                    Microsoft.SharePoint.Administration.TraceSeverity.Unexpected,
                    LogHelper.DefaultAreaCategory);
            }

            return true;
        }

        /// <summary>
        /// Sets the type of the property data.
        /// </summary>
        /// <param name="cp">The core property object</param>
        /// <param name="type">The property data type (from DataType object)</param>
        /// <param name="length">The optional length</param>
        /// <remarks></remarks>
        private static void SetPropertyDataType(CoreProperty cp, DataType type, int length)
        {
            switch (type)
            {
                case DataType.Binary:
                    cp.Type = PropertyDataType.Binary;
                    if (length <= 0)
                    {
                        length = 25;
                    }
                    cp.Length = length;
                    break;
                case DataType.Boolean:
                    cp.Type = PropertyDataType.Boolean;
                    break;
                case DataType.Date:
                    cp.Type = PropertyDataType.Date;
                    break;
                case DataType.DateNoYear:
                    cp.Type = PropertyDataType.DateNoYear;
                    break;
                case DataType.DateTime:
                    cp.Type = PropertyDataType.DateTime;
                    break;
                case DataType.Email:
                    cp.Type = PropertyDataType.Email;
                    break;
                case DataType.Float:
                    cp.Type = PropertyDataType.Float;
                    break;
                case DataType.Guid:
                    cp.Type = PropertyDataType.Guid;
                    break;
                case DataType.HTML:
                    cp.Type = PropertyDataType.HTML;
                    if (length <= 0)
                    {
                        length = 2000;
                    }
                    cp.Length = length;
                    break;
                case DataType.Integer:
                    cp.Type = PropertyDataType.Integer;
                    break;
                case DataType.Person:
                    cp.Type = PropertyDataType.Person;
                    break;
                case DataType.StringMultiValue:
                    cp.Type = PropertyDataType.StringMultiValue;
                    cp.IsMultivalued = true;
                    if (length <= 0)
                    {
                        length = 25;
                    }
                    cp.Length = length;
                    break;
                case DataType.StringSingleValue:
                    cp.Type = PropertyDataType.StringSingleValue;
                    if (length <= 0)
                    {
                        length = 25;
                    }
                    cp.Length = length;
                    break;
                case DataType.TimeZone:
                    cp.Type = PropertyDataType.TimeZone;
                    break;
                case DataType.URL:
                    cp.Type = PropertyDataType.URL;
                    break;
            }
        }
    }
}

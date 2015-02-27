
using System;
using Microsoft.Office.Server.UserProfiles;
using Microsoft.SharePoint;

namespace Tarento.Common.Utilities
{
    /// <summary>
    /// Provides access to current user's profile and the values in the profile fields.
    /// </summary>
    /// <remarks></remarks>
    public class CurrentUser
    {
        /// <summary>
        /// User profile object 
        /// </summary>
        public UserProfile Profile { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        /// <remarks></remarks>
        public CurrentUser()
        {
            try
            {
                this.Profile = ProfileLoader.GetProfileLoader().GetUserProfile();
            }
            catch { }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentUser"/> class.
        /// </summary>
        /// <param name="context">The service context.</param>
        /// <remarks></remarks>
        internal CurrentUser(SPServiceContext context)
        {
            try
            {
                this.Profile = ProfileLoader.GetProfileLoader(context).GetUserProfile();
            }
            catch { }
        }

        /// <summary>
        /// Gets the <see cref="System.Object"/> with the specified value.
        /// </summary>
        /// <remarks></remarks>
        public object this[string value]
        {
            get
            {
                return Profile[value].Value;
            }
        }

        /// <summary>
        /// Gets the value of a property from the profile
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The value of the passed property key</returns>
        /// <remarks></remarks>
        public static object GetValue(string value)
        {
            CurrentUser user = new CurrentUser();
            return user.Profile[value].Value;
        }

        /// <summary>
        /// Gets the name of the account.
        /// </summary>
        /// <remarks></remarks>
        public string AccountName
        {
            get
            {
                return Profile["AccountName"].Value.ToSafeString();
            }
        }

        /// <summary>
        /// Gets the user name of the account.
        /// </summary>
        /// <remarks></remarks>
        public string UserName
        {
            get
            {
                return Profile["UserName"].Value.ToSafeString();
            }
        }

        /// <summary>
        /// Gets or sets the current user region in the user profile
        /// </summary>
        /// <value>The current user region.</value>
        /// <remarks></remarks>
        public string Region
        {
            get
            {
                UserProfile profile = ProfileLoader.GetProfileLoader().GetUserProfile();
                try
                {
                    return Profile["CurrentRegion"].Value.ToSafeString();
                }
                catch { return string.Empty; }
            }
            set
            {
                UserProfile profile = ProfileLoader.GetProfileLoader().GetUserProfile();
                Profile["CurrentRegion"].Value = value;
                profile.Commit();
            }
        }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        /// <remarks></remarks>
        public string Location
        {
            get
            {
                UserProfile profile = ProfileLoader.GetProfileLoader().GetUserProfile();
                try
                {
                    return Profile["CurrentLocation"].Value.ToSafeString();
                }
                catch { return string.Empty; }
            }
            set
            {
                UserProfile profile = ProfileLoader.GetProfileLoader().GetUserProfile();
                Profile["CurrentLocation"].Value = value;
                profile.Commit();
            }
        }

        /// <summary>
        /// Gets the email.
        /// </summary>
        /// <remarks></remarks>
        public string WorkEmail
        {
            get
            {
                return Profile["WorkEmail"].Value.ToSafeString();
            }
        }

        /// <summary>
        /// Gets the name of the login.
        /// </summary>
        /// <remarks></remarks>
        public string LoginName
        {
            get
            {
                return Profile["AccountName"].Value.ToSafeString();
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <remarks></remarks>
        public string DisplayName
        {
            get { return Profile["PreferredName"].Value.ToSafeString(); }
        }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <remarks></remarks>
        public string Title
        {
            get
            {
                return Profile["Title"].Value.ToSafeString();
            }
        }

        /// <summary>
        /// Gets the department.
        /// </summary>
        /// <remarks></remarks>
        public string Department
        {
            get
            {
                return Profile["Department"].Value.ToSafeString();
            }
        }

        ///// <summary>
        ///// Gets the business unit.
        ///// </summary>
        ///// <remarks></remarks>
        //public string BusinessUnit
        //{
        //    get
        //    {
        //        return Profile["BusinessUnit"].Value.ToSafeString();
        //    }
        //}

        /// <summary>
        /// Gets the first name of the current user
        /// </summary>
        /// <remarks></remarks>
        public string FirstName
        {
            get
            {
                return Profile["FirstName"].Value.ToSafeString();
            }
        }

        /// <summary>
        /// Gets the last name of the current user.
        /// </summary>
        /// <remarks></remarks>
        public string LastName
        {
            get
            {
                return Profile["LastName"].Value.ToSafeString();
            }
        }

        /// <summary>
        /// Gets the manager of the current user
        /// </summary>
        /// <remarks></remarks>
        public string Manager
        {
            get
            {
                return Profile["Manager"].Value.ToSafeString();
            }
        }

        /// <summary>
        /// Gets the office.
        /// </summary>
        /// <remarks></remarks>
        public string Office
        {
            get
            {
                return Profile["Office"].Value.ToSafeString();
            }
        }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <remarks></remarks>
        public string MySiteUrl
        {
            get
            {
                return Profile["PersonalSpace"].Value.ToSafeString();
            }
        }

        /// <summary>
        /// Gets the work phone.
        /// </summary>
        /// <remarks></remarks>
        public string WorkPhone
        {
            get
            {
                return Profile["WorkPhone"].Value.ToSafeString();
            }
        }

        /// <summary>
        /// Gets the cell phone.
        /// </summary>
        /// <remarks></remarks>
        public string CellPhone
        {
            get
            {
                return Profile["CellPhone"].Value.ToSafeString();
            }
        }

        /// <summary>
        /// Gets the user profile image.
        /// </summary>
        /// <remarks></remarks>
        public string PictureURL
        {
            get
            {
                return Profile["PictureURL"].Value.ToSafeString().UrlEncode();
            }
        }

        /// <summary>
        /// Gets the user's time zone.
        /// </summary>
        /// <remarks></remarks>
        public SPTimeZone TimeZone
        {
            get
            {
                if (Profile["SPS-TimeZone"] != null)
                {
                    return Profile["SPS-TimeZone"].Value as Microsoft.SharePoint.SPTimeZone;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the current date time for the current user based on the time zone in user profiles
        /// </summary>
        /// <remarks></remarks>
        public DateTime? CurrentDateTime
        {
            get
            {
                DateTime utc = DateTime.UtcNow;
                SPTimeZone zone = this.TimeZone;
                if (zone != null)
                {
                    return utc.AddMinutes((-(zone.Information.Bias)) - (zone.Information.DaylightBias));
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the assistant.
        /// </summary>
        /// <remarks></remarks>
        public string Assistant
        {
            get
            {
                return Profile["Assistant"].Value.ToSafeString();
            }
        }

        /// <summary>
        /// Gets the current user's culture based on regional settings, if they do not exist, returns the culture on the current thread.
        /// </summary>
        /// <remarks></remarks>
        public System.Globalization.CultureInfo Culture
        {
            get
            {
                System.Globalization.CultureInfo ci;
                ci = System.Threading.Thread.CurrentThread.CurrentCulture;
                try
                {
                    ci = new System.Globalization.CultureInfo((int)SPContext.Current.Web.CurrentUser.RegionalSettings.LocaleId);
                }
                catch { }//culture is not set, use thread culture from the browser settings
                return ci;
            }
        }

        /// <summary>
        /// Checks to see if the current user has specified the type of clock to display
        /// </summary>
        /// <remarks></remarks>
        public bool? Use24HourClock
        {
            get
            {
                try
                {
                    return SPContext.Current.Web.CurrentUser.RegionalSettings.Time24;
                }
                catch { }//culture is not set, use thread culture from the browser settings
                return null;
            }
        }
    }
}

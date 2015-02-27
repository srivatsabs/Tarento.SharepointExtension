using Microsoft.Office.Server.UserProfiles;

namespace Tarento.Common.Utilities
{
    /// <summary>
    /// A support class used by the ProfilePropertyHelper to define security and visibility on profile properties.
    /// </summary>
    /// <remarks></remarks>
    public class ProfilePropertyPolicy
    {
        /// <summary>
        /// Gets or sets the default privacy.
        /// </summary>
        /// <value>The default privacy.</value>
        /// <remarks></remarks>
        public Privacy DefaultPrivacy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is user editable.
        /// </summary>
        /// <value><c>true</c> if this instance is user editable; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public bool IsUserEditable { get; set; }

        /// <summary>
        /// Gets or sets the user policy.
        /// </summary>
        /// <value>The user policy.</value>
        /// <remarks></remarks>
        public PrivacyPolicy UserPolicy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can user override.
        /// </summary>
        /// <value><c>true</c> if this instance can user override; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public bool CanUserOverride { get; set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <remarks></remarks>
        public ProfilePropertyPolicy()
        {
            this.DefaultPrivacy = Privacy.NotSet;
            this.IsUserEditable = false;
            this.UserPolicy = PrivacyPolicy.OptIn;
            this.CanUserOverride = false;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="privacy">The privacy to set</param>
        /// <param name="policy">The policy to set on the property</param>
        /// <param name="isUserEditable">if set to <c>true</c> [the property is user editable].</param>
        /// <param name="userCanOverride">if set to <c>true</c> [the user can override the property settings].</param>
        /// <remarks></remarks>
        public ProfilePropertyPolicy(Privacy privacy, PrivacyPolicy policy, bool isUserEditable, bool userCanOverride)
        {
            this.DefaultPrivacy = privacy;
            this.IsUserEditable = isUserEditable;
            this.UserPolicy = policy;
            this.CanUserOverride = userCanOverride;
        }
    }
}

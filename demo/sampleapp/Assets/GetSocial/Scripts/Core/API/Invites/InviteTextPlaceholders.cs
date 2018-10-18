namespace GetSocialSdk.Core
{
    /// <summary>
    /// Invite placeholder tags that will be replaced with their value if included in invite.
    /// </summary>
    public static class InviteTextPlaceholders
    {
        /// <summary>
        /// This tag is replaced with url to download the app.
        /// </summary>
        public const string PlaceholderAppInviteUrl = "[APP_INVITE_URL]";

        // TODO: add in the next release
//        /// <summary>
//        /// If the user is logged in tag will be replaced with the user name. Otherwise with empty string.
//        /// </summary>
//        public const string PlaceholderUserDisplayName = "[USER_NAME]";
    }
}
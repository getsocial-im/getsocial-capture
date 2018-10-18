using System;
using System.Collections.Generic;
using UnityEngine;

namespace GetSocialSdk.Core
{

    /// <summary>
    /// Listener to handle GetSocial notifications.
    /// Called if application was started by clicking on GetSocial push notification
    /// or notification was received while application is in foreground.
    /// </summary>
    /// <param name="notification">Received notification</param>
    /// <param name="wasClicked">is true, if notification was clicked by user in notification center, false if received while application was in foreground.</param>
    /// <returns>true, if you handle this notification, false otherwise</returns>
    public delegate bool NotificationListener(Notification notification, bool wasClicked);

    /// <summary>
    /// The GetSocial API
    /// </summary>
    public static partial class GetSocial
    {
        static readonly Action<GetSocialError> _globalErrorListener = OnGlobalError;

        static Action<GetSocialError> _userGlobalErrorListener;

        static IGetSocialNativeBridge _getSocialImpl;

        private static IGetSocialNativeBridge GetSocialImpl
        {
            get
            {
                if (_getSocialImpl == null)
                {
                    _getSocialImpl = GetSocialFactory.Instance;
                    Debug.LogFormat("Using GetSocial Unity SDK v{0}, underlying native SDK v{1}", UnitySdkVersion, _getSocialImpl.GetNativeSdkVersion());
                }
                return _getSocialImpl;
            }
        }

        static void OnGlobalError(GetSocialError error)
        {
            if (_userGlobalErrorListener != null)
            {
                _userGlobalErrorListener(error);
            }
        }

        /// <summary>
        /// Used for testing! NOT public API!
        /// </summary>
        internal static void InjectBridgeInternal(IGetSocialNativeBridge bridge)
        {
            _getSocialImpl = bridge;
        }

        #region meta_data

        /// <summary>
        /// Gets the GetSocial Unity SDK version.
        /// </summary>
        /// <value>The GetSocial Unity SDK version.</value>
        public static string UnitySdkVersion
        {
            get { return BuildConfig.UnitySdkVersion; }
        }

        /// <summary>
        /// Gets the native SDK (Android/iOS) version.
        /// </summary>
        /// <value>The native SDK (Android/iOS) version.</value>
        public static string NativeSdkVersion
        {
            get { return GetSocialImpl.GetNativeSdkVersion(); }
        }

        #endregion

        #region initialization

        /// <summary>
        /// Init the SDK. Use <see cref="WhenInitialized"/> to be notified when SDK is initialized. Check errors in logs or in GlobalErrorListener using <see cref="SetGlobalErrorListener"/>.
        /// </summary>
        public static void Init()
        {
            GetSocialImpl.Init(null);
        }
        
        /// <summary>
        /// Init the SDK. Use <see cref="WhenInitialized"/> to be notified when SDK is initialized. Check errors in logs or in GlobalErrorListener using <see cref="SetGlobalErrorListener"/>.
        /// </summary>
        /// <param name="appId">Application ID</param>
        public static void Init(string appId)
        {
            GetSocialImpl.Init(appId);
        }
        
        /// <summary>
        /// Set an action, which should be executed after SDK initialized.
        /// Executed immediately, if SDK is already initialized.
        /// </summary>
        /// <param name="action">Action to execute.</param>
        public static void WhenInitialized(Action action)
        {
            GetSocialImpl.WhenInitialized(action);
        }

        /// <summary>
        /// Provides the status of the GetSocial initialisation.
        /// </summary>
        /// <returns><c>true</c> if SDK has completed successfully; otherwise, <c>false</c>.</returns>
        public static bool IsInitialized
        {
            get { return GetSocialImpl.IsInitialized; }
        }

        #endregion

        /// <summary>
        /// Set the global error listener instance, that will we notified about internal exceptions in the SDK.
        /// </summary>
        /// <returns><c>true</c>, if global error listener was set, <c>false</c> otherwise.</returns>
        /// <param name="onError">On error listener.</param>
        public static bool SetGlobalErrorListener(Action<GetSocialError> onError)
        {
            Check.Argument.IsNotNull(onError, "onError");
            GetSocialImpl.SetGlobalErrorListener(_globalErrorListener);
            _userGlobalErrorListener = onError;
            return true;
        }

        /// <summary>
        /// Removes the global error listener.
        /// </summary>
        /// <returns><c>true</c>, if global error listener was removed, <c>false</c> otherwise.</returns>
        public static bool RemoveGlobalErrorListener()
        {
            GetSocialImpl.RemoveGlobalErrorListener();
            _userGlobalErrorListener = null;
            return true;
        }

        #region localization

        /// <summary>
        /// The current language of GetSocial SDK, return value must be one of the language codes provided in <see cref="LanguageCodes"/>
        /// </summary>
        /// <returns>Current language of GetSocial SDK</returns>
        public static string GetLanguage()
        {
            return GetSocialImpl.GetLanguage();
        }

        /// <summary>
        /// Set the language of GetSocial SDK. If language is incorrect, default language is set.
        /// </summary>
        /// <param name="languageCode">Must be one of language codes provided in <see cref="LanguageCodes"/></param>
        /// <returns><c>true</c> if the operation was successful; <c>false</c> otherwise</returns>
        public static bool SetLanguage(string languageCode)
        {
            Check.Argument.IsStrNotNullOrEmpty(languageCode, "languageCode");

            return GetSocialImpl.SetLanguage(languageCode);
        }

        #endregion

        #region smart_invites

        public static bool IsInviteChannelAvailable(string channelId)
        {
            return GetSocialImpl.IsInviteChannelAvailable(channelId);
        }
        
        /// <summary>
        /// Returns all supported invite channels.
        /// </summary>
        /// <returns>All supported invite channels</returns>
        public static InviteChannel[] InviteChannels
        {
            get { return GetSocialImpl.InviteChannels; }
        }

        /// <summary>
        /// Invite friends via a specific invite channel.
        /// </summary>
        /// <param name="channelId">The channel through which the invite will be sent, will be lowercased.</param>
        /// <param name="onComplete">Called when the invite process was successful.</param>
        /// <param name="onCancel">Called when the invite process was cancelled by the user.</param>
        /// <param name="onFailure">Called when the invite process failed.</param>
        public static void SendInvite(string channelId, Action onComplete, Action onCancel,
            Action<GetSocialError> onFailure)
        {
            Check.IfTrue(IsInitialized, "GetSocial must be initialized before calling SendInvite()");
            Check.Argument.IsStrNotNullOrEmpty(channelId, "channelId");
            Check.Argument.IsNotNull(onComplete, "onComplete");
            Check.Argument.IsNotNull(onCancel, "onCancel");
            Check.Argument.IsNotNull(onFailure, "onFailure");

            GetSocialImpl.SendInvite(channelId, onComplete, onCancel, onFailure);
        }

        /// <summary>
        /// Invite friends via a specific invite channel.
        /// </summary>
        /// <param name="channelId">The channel through which the invite will be sent, will be lowercased.</param>
        /// <param name="customInviteContent">Custom content to override the default content provided from the Dashboard.</param>
        /// <param name="onComplete">Called when the invite process was successful.</param>
        /// <param name="onCancel">Called when the invite process was cancelled by the user.</param>
        /// <param name="onFailure">Called when the invite process failed.</param>
        public static void SendInvite(string channelId, InviteContent customInviteContent,
            Action onComplete, Action onCancel, Action<GetSocialError> onFailure)
        {
            Check.Argument.IsStrNotNullOrEmpty(channelId, "channelId");
            Check.Argument.IsNotNull(onComplete, "onComplete");
            Check.Argument.IsNotNull(onCancel, "onCancel");
            Check.Argument.IsNotNull(onFailure, "onFailure");

            GetSocialImpl.SendInvite(channelId, customInviteContent, onComplete, onCancel,
                onFailure);
        }

        /// <summary>
        /// Invite friends via a specific invite channel.
        /// </summary>
        /// <param name="channelId">The channel through which the invite will be sent, will be lowercased.</param>
        /// <param name="customInviteContent">Custom content to override the default content provided from the Dashboard.</param>
        /// <param name="customReferralData">Custom data to be associated with this invite.</param>
        /// <param name="onComplete">Called when the invite process was successful.</param>
        /// <param name="onCancel">Called when the invite process was cancelled by the user.</param>
        /// <param name="onFailure">Called when the invite process failed.</param>
        /// Deprecated, use <see cref="SendInvite(string,InviteContent,LinkParams,System.Action,System.Action,System.Action{GetSocialSdk.Core.GetSocialError})"/> instead.
        [Obsolete("Deprecated, please use SendInvite(string channelId, InviteContent customInviteContent, " +
                  "LinkParams linkParams, " +
                  "Action onComplete, " +
                  "Action onCancel, " +
                  "Action<GetSocialError> onFailure) instead.")]
        public static void SendInvite(string channelId, InviteContent customInviteContent,
            CustomReferralData customReferralData,
            Action onComplete, Action onCancel, Action<GetSocialError> onFailure)
        {
            LinkParams linkParams = new LinkParams(customReferralData);
            SendInvite(channelId, customInviteContent, linkParams, onComplete, onCancel, onFailure);
        }

        /// <summary>
        /// Invite friends via a specific invite channel.
        /// </summary>
        /// <param name="channelId">The channel through which the invite will be sent, will be lowercased.</param>
        /// <param name="customInviteContent">Custom content to override the default content provided from the Dashboard.</param>
        /// <param name="linkParams">Link customization parameters.</param>
        /// <param name="onComplete">Called when the invite process was successful.</param>
        /// <param name="onCancel">Called when the invite process was cancelled by the user.</param>
        /// <param name="onFailure">Called when the invite process failed.</param>
        public static void SendInvite(string channelId, InviteContent customInviteContent,
            LinkParams linkParams,
            Action onComplete, Action onCancel, Action<GetSocialError> onFailure)
        {
            Check.Argument.IsStrNotNullOrEmpty(channelId, "channelId");
            Check.Argument.IsNotNull(onComplete, "onComplete");
            Check.Argument.IsNotNull(onCancel, "onCancel");
            Check.Argument.IsNotNull(onFailure, "onFailure");

            GetSocialImpl.SendInvite(channelId, customInviteContent, linkParams, onComplete, onCancel,
                onFailure);
        }

        /// <summary>
        /// Register a new instance of a plugin for a specified channel.
        /// </summary>
        /// <returns><c>true</c>, if the operation was successful, <c>false</c> otherwise.</returns>
        /// <param name="channelId">Id of the channel for the plugin implementation, one of <see cref="InviteChannelIds"/> constant.</param>
        /// <param name="inviteChannelPlugin">an instance of a plugin implementation.</param>
        public static bool RegisterInviteChannelPlugin(string channelId, InviteChannelPlugin inviteChannelPlugin)
        {
            Check.Argument.IsStrNotNullOrEmpty(channelId, "channelId");
            Check.Argument.IsNotNull(inviteChannelPlugin, "inviteChannelPlugin");

            return GetSocialImpl.RegisterInviteChannelPlugin(channelId, inviteChannelPlugin);
        }

        /// <summary>
        /// Get referral data attached to the GetSocial link that was clicked to open or install the app. 
        /// Referral data will be returned only on the first app session started by GetSocial link.
        /// </summary>
        /// <param name="onSuccess">Called when invocation was successfull.</param>
        /// <param name="onFailure">Called when failed retrieving list of referred users.</param>
        public static void GetReferralData(Action<ReferralData> onSuccess, Action<GetSocialError> onFailure)
        {
            Check.Argument.IsNotNull(onSuccess, "onSuccess");
            Check.Argument.IsNotNull(onFailure, "onFailure");

            GetSocialImpl.GetReferralData(onSuccess, onFailure);
        }

        /// <summary>
        /// Get list of users who installed the app by accepting invite of current user. 
        /// </summary>
        /// <param name="onSuccess">Called when invocation was successfull.</param>
        /// <param name="onFailure">Called when failed retrieving list of referred users.</param>
        public static void GetReferredUsers(Action<List<ReferredUser>> onSuccess, Action<GetSocialError> onFailure)
        {
            Check.Argument.IsNotNull(onSuccess, "onSuccess");
            Check.Argument.IsNotNull(onFailure, "onFailure");
            
            GetSocialImpl.GetReferredUsers(onSuccess, onFailure);
        }

        /// <summary>
        /// Creates a Smart Link with user referral data attached used for Smart Invites.
        /// </summary>
        /// <param name="linkParams">Link customization parameters. More info @see <a href="https://docs.getsocial.im/guides/smart-links/parameters/">here</a></param>
        /// <param name="onSuccess">Called when smart link was created.</param>
        /// <param name="onFailure">Called when smart link creation failed.</param>
        public static void CreateInviteLink(LinkParams linkParams, Action<string> onSuccess, Action<GetSocialError> onFailure)
        {
            Check.Argument.IsNotNull(onSuccess, "onComplete");
            Check.Argument.IsNotNull(onFailure, "onFailure");
            
            GetSocialImpl.CreateInviteLink(linkParams, onSuccess, onFailure);
        }

        #endregion

        #region push_notification

        /// <summary>
        /// If AutoRegisterForPush checkbox is unchecked in GetSocial Unity Settings,
        /// call this method to register for push notifications.
        /// </summary>
        public static void RegisterForPushNotifications()
        {
            GetSocialImpl.RegisterForPushNotifications();
        }
        /// <summary>
        /// Set a notification listener, will be invoked if application was started with clicking on GetSocial notification 
        /// or if notification was received while application is in foreground.
        /// Action is not invoked on Unity UI thread, so you can not change any scene objects directly in callback,
        /// you need to manually execute scene update on Unity UI thread.
        /// </summary>
        /// <param name="listener">Called with received action.</param>
        public static void SetNotificationListener(NotificationListener listener)
        {
            Check.Argument.IsNotNull(listener, "Notification Action Listener");

            GetSocialImpl.SetNotificationListener((notification, wasClicked) => listener(notification, wasClicked));
        }

        #endregion

        #region activity_feed

        /// <summary>
        /// Retrieve list of announcements for global feed.
        /// </summary>
        /// <param name="onSuccess">Called when list of announcements was successfully retrieved.</param>
        /// <param name="onFailure">Called when failed retrieveing list of announcements.</param>
        public static void GetGlobalFeedAnnouncements(Action<List<ActivityPost>> onSuccess,
            Action<GetSocialError> onFailure)
        {
            Check.Argument.IsNotNull(onSuccess, "onSuccess");
            Check.Argument.IsNotNull(onFailure, "onFailure");

            GetSocialImpl.GetAnnouncements(ActivitiesQuery.GlobalFeed, onSuccess, onFailure);
        }

        /// <summary>
        /// Retrieve list of announcements for feed.
        /// </summary>
        /// <param name="feed">Feed name.</param>
        /// <param name="onSuccess">Called when list of announcements was successfully retrieved.</param>
        /// <param name="onFailure">Called when failed retrieveing list of announcements.</param>
        public static void GetAnnouncements(string feed, Action<List<ActivityPost>> onSuccess,
            Action<GetSocialError> onFailure)
        {
            Check.Argument.IsStrNotNullOrEmpty(feed, "feed");
            Check.Argument.IsNotNull(onSuccess, "onSuccess");
            Check.Argument.IsNotNull(onFailure, "onFailure");

            GetSocialImpl.GetAnnouncements(feed, onSuccess, onFailure);
        }


        /// <summary>
        /// Retrieve list of activities.
        /// </summary>
        /// <param name="query">Filtering options</param>
        /// <param name="onSuccess">Called when list of announcements was successfully retrieved.</param>
        /// <param name="onFailure">Called when failed retrieveing list of announcements.</param>
        public static void GetActivities(ActivitiesQuery query, Action<List<ActivityPost>> onSuccess,
            Action<GetSocialError> onFailure)
        {
            Check.Argument.IsNotNull(query, "query");
            Check.Argument.IsNotNull(onSuccess, "onSuccess");
            Check.Argument.IsNotNull(onFailure, "onFailure");

            GetSocialImpl.GetActivities(query, onSuccess, onFailure);
        }

        /// <summary>
        /// Retrieve activity by id.
        /// </summary>
        /// <param name="activityId">Id of activity</param>
        /// <param name="onSuccess">Called when activity was successfully retrieved.</param>
        /// <param name="onFailure">Called when failed retrieveing activity.</param>
        public static void GetActivity(string activityId, Action<ActivityPost> onSuccess,
            Action<GetSocialError> onFailure)
        {
            Check.Argument.IsNotNull(onSuccess, "onSuccess");
            Check.Argument.IsNotNull(onFailure, "onFailure");

            GetSocialImpl.GetActivity(activityId, onSuccess, onFailure);
        }

        /// <summary>
        /// Post activity to global Activity Feed.
        /// </summary>
        /// <param name="content">Content of activity, that should be posted.</param>
        /// <param name="onSuccess">Called when activity was successfully posted.</param>
        /// <param name="onFailure">Called when failed posting activity.</param>
        public static void PostActivityToGlobalFeed(ActivityPostContent content,
            Action<ActivityPost> onSuccess,
            Action<GetSocialError> onFailure)
        {
            Check.Argument.IsNotNull(onSuccess, "onSuccess");
            Check.Argument.IsNotNull(onFailure, "onFailure");

            GetSocialImpl.PostActivityToFeed(ActivitiesQuery.GlobalFeed, content, onSuccess, onFailure);
        }


        /// <summary>
        /// Post activity to specified Activity Feed.
        /// </summary>
        /// <param name="feed">Activity feed to post to.</param>
        /// <param name="content">Content of activity, that should be posted.</param>
        /// <param name="onSuccess">Called when activity was successfully posted.</param>
        /// <param name="onFailure">Called when failed posting activity.</param>
        public static void PostActivityToFeed(string feed, ActivityPostContent content,
            Action<ActivityPost> onSuccess,
            Action<GetSocialError> onFailure)
        {
            Check.Argument.IsNotNull(content, "content");
            Check.Argument.IsNotNull(onSuccess, "onSuccess");
            Check.Argument.IsNotNull(onFailure, "onFailure");

            GetSocialImpl.PostActivityToFeed(feed, content, onSuccess, onFailure);
        }

        /// <summary>
        /// Post comment to activity.
        /// </summary>
        /// <param name="activityId">Identifier of activity, that we want to comment</param>
        /// <param name="comment">Content of comment, that should be posted</param>
        /// <param name="onSuccess">Called when activity comment was successfully posted.</param>
        /// <param name="onFailure">Called when failed posting activity comment.</param>
        public static void PostCommentToActivity(string activityId, ActivityPostContent comment,
            Action<ActivityPost> onSuccess,
            Action<GetSocialError> onFailure)
        {
            Check.Argument.IsNotNull(comment, "comment");
            Check.Argument.IsNotNull(onSuccess, "onSuccess");
            Check.Argument.IsNotNull(onFailure, "onFailure");

            GetSocialImpl.PostCommentToActivity(activityId, comment, onSuccess, onFailure);
        }

        /// <summary>
        /// Like or unlike activity.
        /// </summary>
        /// <param name="activityId">Identifier of activity, that we want to like or unlike</param>
        /// <param name="isLiked">Should activity be liked or not</param>
        /// <param name="onSuccess">Called when successfully liked/unliked activity.</param>
        /// <param name="onFailure">Called when liking/unliking activity failed.</param>
        public static void LikeActivity(string activityId, bool isLiked,
            Action<ActivityPost> onSuccess,
            Action<GetSocialError> onFailure)
        {
            Check.Argument.IsNotNull(onSuccess, "onSuccess");
            Check.Argument.IsNotNull(onFailure, "onFailure");

            GetSocialImpl.LikeActivity(activityId, isLiked, onSuccess, onFailure);
        }

        /// <summary>
        /// Get a list of users, that liked activity.
        /// </summary>
        /// <param name="activityId">Identifier of activity, that we want to get who liked it</param>
        /// <param name="offset">offset position</param>
        /// <param name="limit">limit of list you want to get</param>
        /// <param name="onSuccess">Called when successfully retrieved list of activity likers.</param>
        /// <param name="onFailure">Called when retrieving list of likers failed.</param>
        public static void GetActivityLikers(string activityId, int offset, int limit,
            Action<List<PublicUser>> onSuccess,
            Action<GetSocialError> onFailure)
        {
            Check.Argument.IsNotNull(onSuccess, "onSuccess");
            Check.Argument.IsNotNull(onFailure, "onFailure");

            GetSocialImpl.GetActivityLikers(activityId, offset, limit, onSuccess, onFailure);
        }

        /// <summary>
        /// Report activity because of its content.
        /// </summary>
        /// <param name="activityId">Id of activity to report</param>
        /// <param name="reportingReason">reportingReason Reason of reporting</param>
        /// <param name="onSuccess">Called when activity was successfully reported</param>
        /// <param name="onFailure">Called when reporting activity failed</param>
        ///
        public static void ReportActivity(string activityId, ReportingReason reportingReason, Action onSuccess,
            Action<GetSocialError> onFailure)
        {
            Check.Argument.IsNotNull(onSuccess, "onSuccess");
            Check.Argument.IsNotNull(onFailure, "onFailure");
            
            GetSocialImpl.ReportActivity(activityId, reportingReason, onSuccess, onFailure);
        }

        /// <summary>
        /// Delete your activity. Attempt to delete other user activity will result in failure.
        /// </summary>
        /// <param name="activityId">Id of activity to report</param>
        /// <param name="onSuccess">Called when activity was successfully deleted</param>
        /// <param name="onFailure">Called when deleting activity failed</param>
        ///
        public static void DeleteActivity(string activityId, Action onSuccess,
            Action<GetSocialError> onFailure)
        {
            Check.Argument.IsNotNull(onSuccess, "onSuccess");
            Check.Argument.IsNotNull(onFailure, "onFailure");
            
            GetSocialImpl.DeleteActivity(activityId, onSuccess, onFailure);
        }

        #endregion

        #region user_management

        /// <summary>
        /// Get a user by id.
        /// </summary>
        ///
        /// <param name="userId">Id of user.</param>
        /// <param name="onSuccess">Called if user is found.</param>
        /// <param name="onFailure">Called if operation failed or user not found.</param>
        public static void GetUserById(string userId, Action<PublicUser> onSuccess,
            Action<GetSocialError> onFailure)
        {
            Check.Argument.IsNotNull(onSuccess, "onSuccess");
            Check.Argument.IsNotNull(onFailure, "onFailure");
                
            GetSocialImpl.GetUserById(userId, onSuccess, onFailure);
        }

        /// <summary>
        /// Fetch a user by auth provider id and user id on this provider.
        /// </summary>
        ///
        /// <param name="providerId">Auth identity provider id for which user id is provided. Can be "facebook", or any custom value.</param>
        /// <param name="providerUserId">User id on the selected identity provider for which <see cref="PublicUser"/> will be returned.</param>
        /// <param name="onSuccess">Called if user is found.</param>
        /// <param name="onFailure">Called if operation failed or user not found.</param>
        public static void GetUserByAuthIdentity(string providerId, string providerUserId, Action<PublicUser> onSuccess,
            Action<GetSocialError> onFailure)
        {
            Check.Argument.IsNotNull(providerId, "providerId");
            Check.Argument.IsNotNull(providerUserId, "providerUserId");
            Check.Argument.IsNotNull(onSuccess, "onSuccess");
            Check.Argument.IsNotNull(onFailure, "onFailure");
            
            GetSocialImpl.GetUserByAuthIdentity(providerId, providerUserId, onSuccess, onFailure);
        }

        /// <summary>
        /// Fetch a list of users by their auth identities pairs. 
        /// Auth identity pair is a combination of provider id and user id on this provider.
        /// </summary>
        ///
        /// <param name="providerId">Auth identity provider id for which user id is provided. Can be "facebook", or any custom value.</param>
        /// <param name="providerUserIds">User id on the selected identity provider for which <see cref="PublicUser"/> will be returned.</param>
        /// <param name="onSuccess">Called if any user is found. Please note, that not all requested user may be returned. </param>
        /// <param name="onFailure">Called if operation failed or user not found.</param>
        public static void GetUsersByAuthIdentities(string providerId, List<string> providerUserIds, Action<Dictionary<string, PublicUser>> onSuccess,
            Action<GetSocialError> onFailure)
        {
            Check.Argument.IsNotNull(providerId, "providerId");
            Check.Argument.IsNotNull(providerUserIds, "providerUserIds");
            Check.Argument.IsNotNull(onSuccess, "onSuccess");
            Check.Argument.IsNotNull(onFailure, "onFailure");
            
            GetSocialImpl.GetUsersByAuthIdentities(providerId, providerUserIds, onSuccess, onFailure);
        }

        /// <summary>
        /// Find users matching query.
        /// </summary>
        /// <param name="query">Users query.</param>
        /// <param name="onSuccess">Called with list of users.</param>
        /// <param name="onFailure">Called if operation failed.</param>
        public static void FindUsers(UsersQuery query, Action<List<UserReference>> onSuccess,
            Action<GetSocialError> onFailure)
        {
            Check.Argument.IsNotNull(query, "query");
            Check.Argument.IsNotNull(onSuccess, "onSuccess");
            Check.Argument.IsNotNull(onFailure, "onFailure");

            GetSocialImpl.FindUsers(query, onSuccess, onFailure);
        }

        
        /// <summary>
        /// This corresponds the current user connected to GetSocial.
        /// </summary>
        public static class User
        {
            /// <summary>
            /// Unique Identifier of the user.
            /// </summary>
            /// <value>Unique Identifier of the user.</value>
            public static string Id
            {
                get { return GetSocialImpl.UserId; }
            }

            /// <summary>
            /// Indicates if the user has at least one auth identity available.
            /// </summary>
            /// <value>return false if SDK is initialized and the user has at least one auth
            /// identity, otherwise <c>true</c>.
            /// On true result, you must check if the SDK is initialized to validate the result.</value>
            public static bool IsAnonymous
            {
                get { return GetSocialImpl.IsUserAnonymous; }
            }

            /// <summary>
            /// You can add or remove identities using <see cref="AddAuthIdentity"/> and <see cref="RemoveAuthIdentity"/>.
            /// The key(providerId) is the one you've passed as a first parameter to <see cref="AuthIdentity.CreateCustomIdentity"/>
            /// or <see cref="AuthIdentityProvider.Facebook"/> if you've created Facebook identity with <see cref="AuthIdentity.CreateFacebookIdentity"/>.
            /// Read more about identities in <see href="https://docs.getsocial.im/guides/user-management/android/managing-user-identities/">the documentation</see>.
            /// The value(userId) is the second parameter in <see cref="AuthIdentity.CreateCustomIdentity"/>
            /// or automatically obtained by GetSocial if you've used Facebook identity. 
            /// </summary>
            /// <value>
            /// All auth identities added to the user or an empty map if the sdk is in an illegal state.
            /// When receiving an empty dictionary please check the state of the sdk to determine whether there are no identities or there was an error.
            /// </value>
            public static Dictionary<string, string> AuthIdentities
            {
                get { return GetSocialImpl.UserAuthIdentities; }
            }

            /// <summary>
            /// Display Name of a user.
            /// </summary>
            /// <value>Display name of a user.</value>
            public static string DisplayName
            {
                get { return GetSocialImpl.DisplayName; }
            }

            /// <summary>
            /// User's Avatar URL.
            /// </summary>
            /// <value>Valid URL of an avatar or empty value.</value>
            public static string AvatarUrl
            {
                get { return GetSocialImpl.AvatarUrl; }
            }


            public static void Reset(Action onSuccess, Action<GetSocialError> onError)
            {
                GetSocialImpl.ResetUser(onSuccess, onError);
            }
            
            /// <summary>
            /// Set a new user avatar display name.
            /// </summary>
            /// <param name="displayName">New avatar display name.</param>
            /// <param name="onComplete">Called if changing user display name was successful.</param>
            /// <param name="onFailure">Called if changing user display name failed.</param>
            public static void SetDisplayName(string displayName, Action onComplete, Action<GetSocialError> onFailure)
            {
                GetSocialImpl.SetDisplayName(displayName, onComplete, onFailure);
            }

            /// <summary>
            /// Set a new user avatar URL.
            /// </summary>
            /// <param name="avatarUrl">New avatar URL.</param>
            /// <param name="onComplete">Called if changing user avatar was successful.</param>
            /// <param name="onFailure">Called if changing user avatar failed.</param>
            public static void SetAvatarUrl(string avatarUrl, Action onComplete, Action<GetSocialError> onFailure)
            {
                GetSocialImpl.SetAvatarUrl(avatarUrl, onComplete, onFailure);
            }

            /// <summary>
            /// Set a new user avatar.
            /// </summary>
            /// <param name="avatar">New avatar.</param>
            /// <param name="onComplete">Called if changing user avatar was successful.</param>
            /// <param name="onFailure">Called if changing user avatar failed.</param>
            public static void SetAvatar(Texture2D avatar, Action onComplete, Action<GetSocialError> onFailure)
            {
                GetSocialImpl.SetAvatar(avatar, onComplete, onFailure);
            }

            /// <summary>
            /// Set the public property with specified key and value for the authenticated user.
            /// If you pass empty string as value, it will remove the property. Null values are not allowed.
            /// </summary>
            ///
            /// <param name="key">The property key (Maximum length 64 characters).</param>
            /// <param name="value">The property value (Maximum length 1024 characters).</param>
            /// <param name="onSuccess">Success action</param>
            /// <param name="onFailure">Failure action</param>
            public static void SetPublicProperty(string key, string value, Action onSuccess, Action<GetSocialError> onFailure)
            {
                GetSocialImpl.SetPublicProperty(key, value, onSuccess, onFailure);
            }

            /// <summary>
            /// Set the private property with specified key and value for the authenticated user.
            /// If you pass empty string as value, it will remove the property. Null values are not allowed.
            /// </summary>
            ///
            /// <param name="key">The property key (Maximum length 64 characters).</param>
            /// <param name="value">The property value (Maximum length 1024 characters).</param>
            /// <param name="onSuccess">Success action</param>
            /// <param name="onFailure">Failure action</param>
            public static void SetPrivateProperty(string key, string value, Action onSuccess, Action<GetSocialError> onFailure)
            {
                GetSocialImpl.SetPrivateProperty(key, value, onSuccess, onFailure);
            }

            /// <summary>
            /// Remove one of the properties of the authenticated user.
            /// </summary>
            ///
            /// <param name="key">The property key (Maximum length 64 characters).</param>
            /// <param name="onSuccess">Success action</param>
            /// <param name="onFailure">Failure action</param>
            public static void RemovePublicProperty(string key, Action onSuccess, Action<GetSocialError> onFailure)
            {
                GetSocialImpl.RemovePublicProperty(key, onSuccess, onFailure);
            }

            /// <summary>
            /// Remove one of the properties of the authenticated user.
            /// </summary>
            ///
            /// <param name="key">The property key (Maximum length 64 characters).</param>
            /// <param name="onSuccess">Success action</param>
            /// <param name="onFailure">Failure action</param>
            public static void RemovePrivateProperty(string key, Action onSuccess, Action<GetSocialError> onFailure)
            {
                GetSocialImpl.RemovePrivateProperty(key, onSuccess, onFailure);
            }


            /// <summary>
            /// Get Public user property.
            /// </summary>
            ///
            /// <param name="key">The property key (Maximum length 64 characters).</param>
            /// <returns>
            /// The property value or null if not set or sdk not initialised.
            /// If this returns null you must check if the sdk is initialised to
            /// validate the result
            /// </returns>
            public static string GetPublicProperty(string key)
            {
                return GetSocialImpl.GetPublicProperty(key);
            }

            /// <summary>
            /// Get Public user property.
            /// </summary>
            ///
            /// <param name="key">The property key (Maximum length 64 characters).</param>
            /// <returns>
            /// The property value or null if not set or sdk not initialised.
            /// If this returns null you must check if the sdk is initialised to
            /// validate the result
            /// </returns>
            public static string GetPrivateProperty(string key)
            {
                return GetSocialImpl.GetPrivateProperty(key);
            }

            /// <summary>
            /// Check if property exists for a specific key.
            /// </summary>
            /// <param name="key">The property key (Maximum length 64 characters).</param>
            /// <returns>true if exists, false otherwise</returns>
            public static bool HasPublicProperty(string key)
            {
                return GetSocialImpl.HasPublicProperty(key);
            }

            /// <summary>
            /// Check if property exists for a specific key.
            /// </summary>
            /// <param name="key">The property key (Maximum length 64 characters).</param>
            /// <returns>true if exists, false otherwise</returns>
            public static bool HasPrivateProperty(string key)
            {
                return GetSocialImpl.HasPrivateProperty(key);
            }
            
            /// <summary>
            /// Gets all public properties for current user.
            /// Returns a copy of origin user properties. 
            /// To modify properties, use <see cref="SetPublicProperty"/> and <see cref="RemovePublicProperty"/>.
            /// </summary>
            /// <value>User public properties</value>
            public static Dictionary<string, string> AllPublicProperties
            {
                get { return GetSocialImpl.AllPublicProperties; }
            }
            
            /// <summary>
            /// Gets all private properties for current user.
            /// Returns a copy of origin user properties. 
            /// To modify properties, use <see cref="SetPrivateProperty"/> and <see cref="RemovePrivateProperty"/>.
            /// </summary>
            /// <value>User private properties</value>
            public static Dictionary<string, string> AllPrivateProperties
            {
                get { return GetSocialImpl.AllPrivateProperties; }
            }

            /// <summary>
            /// Adds AuthIdentity for the specified provider.
            /// </summary>
            /// <param name="authIdentity">Identity to be added.</param>
            /// <param name="onComplete">Called if adding user identity was successful.</param>
            /// <param name="onFailure">Called if adding user identity failed.</param>
            /// <param name="onConflict">
            /// Called if identity already belongs to another user and the conflict needs to be resolved.
            /// Remote user to whom the identity belongs is passed as parameter to this callback.
            /// </param>
            public static void AddAuthIdentity(AuthIdentity authIdentity,
                Action onComplete, Action<GetSocialError> onFailure, Action<ConflictUser> onConflict)
            {
                Check.Argument.IsNotNull(authIdentity, "identity");
                Check.Argument.IsNotNull(onComplete, "onComplete");
                Check.Argument.IsNotNull(onFailure, "onFailure");
                Check.Argument.IsNotNull(onConflict, "onConflict");

                GetSocialImpl.AddAuthIdentity(authIdentity, onComplete, onFailure,
                    onConflict);
            }

            /// <summary>
            /// Removes AuthIdentity for the specified provider.
            /// </summary>
            /// <param name="providerId">
            /// The id of the provider of the auth identity to switch to.
            /// Valid providerIds are found in <see cref="AuthIdentityProvider"/> in addition to custom providers.
            /// </param>
            /// <param name="onSuccess">Called if removing user identity was successful.</param>
            /// <param name="onFailure">Called if removing user identity failed.</param>
            public static void RemoveAuthIdentity(string providerId, Action onSuccess, Action<GetSocialError> onFailure)
            {
                Check.Argument.IsStrNotNullOrEmpty(providerId, "providerId");
                Check.Argument.IsNotNull(onSuccess, "onSuccess");
                Check.Argument.IsNotNull(onFailure, "onFailure");

                GetSocialImpl.RemoveAuthIdentity(providerId, onSuccess, onFailure);
            }

            /// <summary>
            /// Switches the current user with the PublicUser corresponding to the details provided.
            /// </summary>
            /// <param name="authIdentity">Identity to be switched to.</param>
            /// <param name="onSuccess">Called if switching user was successful.</param>
            /// <param name="onFailure">Called if switching user failed.</param>
            public static void SwitchUser(AuthIdentity authIdentity,
                Action onSuccess, Action<GetSocialError> onFailure)
            {
                Check.Argument.IsNotNull(authIdentity, "identity");
                Check.Argument.IsNotNull(onSuccess, "onSuccess");
                Check.Argument.IsNotNull(onFailure, "onFailure");

                GetSocialImpl.SwitchUser(authIdentity, onSuccess, onFailure);
            }

            /// <summary>
            /// Add a friend for current user, if operation succeed - they both became friends.
            /// </summary>
            /// <param name="userId">Unique user identifier you want to become friend with.</param>
            /// <param name="onSuccess">Called if adding friend was successful. Parameter contains number of friends.</param>
            /// <param name="onFailure">Called if adding friend failed.</param>
            public static void AddFriend(string userId, Action<int> onSuccess, Action<GetSocialError> onFailure)
            {
                Check.Argument.IsStrNotNullOrEmpty(userId, "userId");
                Check.Argument.IsNotNull(onSuccess, "onSuccess");
                Check.Argument.IsNotNull(onFailure, "onFailure");

                GetSocialImpl.AddFriend(userId, onSuccess, onFailure);
            }

            /// <summary>
            /// Add a list of users to the list of current user friends.
            /// </summary>
            /// <param name="providerId">An auth identity provider id for which user ids will be provided. Can be "facebook", or any custom value.</param>
            /// <param name="providerUserIds">A list of user ids on the selected identity provider that need to be added to the current user's friends list.</param>
            /// <param name="onSuccess">Called if adding friends was successful. Parameter contains number of friends.</param>
            /// <param name="onFailure">Called if adding friends failed.</param>
            public static void AddFriendsByAuthIdentities(string providerId, List<string> providerUserIds, 
                Action<int> onSuccess, Action<GetSocialError> onFailure)
            {
                Check.Argument.IsNotNull(providerId, "providerId");
                Check.Argument.IsNotNull(providerUserIds, "providerUserIds");
                Check.Argument.IsNotNull(onSuccess, "onSuccess");
                Check.Argument.IsNotNull(onFailure, "onFailure");
            
                GetSocialImpl.AddFriendsByAuthIdentities(providerId, providerUserIds, onSuccess, onFailure);
            }

            /// <summary>
            /// Replace existing friends with the provided list of users.
            /// </summary>
            /// <param name="userIds">List of unique user identifiers.</param>
            /// <param name="onSuccess">Called if settings friends was successful.</param>
            /// <param name="onFailure">Called if settings friends failed.</param>
            public static void SetFriends(List<string> userIds, Action onSuccess, Action<GetSocialError> onFailure)
            {
                Check.Argument.IsNotNull(userIds, "providerId");
                Check.Argument.IsNotNull(onSuccess, "onSuccess");
                Check.Argument.IsNotNull(onFailure, "onFailure");
            
                GetSocialImpl.SetFriends(userIds, onSuccess, onFailure);
            }

            /// <summary>
            /// Replace existing friends with the provided list of users.
            /// </summary>
            /// <param name="providerId">A auth identity provider id for which user ids will be provided. Can be "facebook", or any custom value.</param>
            /// <param name="providerUserIds">A list of user ids on the selected identity provider that will be set as the current user's friends list.</param>
            /// <param name="onSuccess">Called if settings friends was successful.</param>
            /// <param name="onFailure">Called if settings friends failed.</param>
            public static void SetFriendsByAuthIdentities(string providerId, List<string> providerUserIds,
                Action onSuccess, Action<GetSocialError> onFailure)
            {
                Check.Argument.IsNotNull(providerId, "providerId");
                Check.Argument.IsNotNull(providerUserIds, "providerUserIds");
                Check.Argument.IsNotNull(onSuccess, "onSuccess");
                Check.Argument.IsNotNull(onFailure, "onFailure");
            
                GetSocialImpl.SetFriendsByAuthIdentities(providerId, providerUserIds, onSuccess, onFailure);
            }
            
            /// <summary>
            /// Remove a list of users for list of current user's friends.
            /// </summary>
            /// <param name="userId">Unique user identifier you want to remove from friends list.</param>
            /// <param name="onSuccess">Called if removing friend was successful. Parameter contains number of friends.</param>
            /// <param name="onFailure">Called if removing friend failed.</param>
            public static void RemoveFriend(string userId, Action<int> onSuccess, Action<GetSocialError> onFailure)
            {
                Check.Argument.IsStrNotNullOrEmpty(userId, "userId");
                Check.Argument.IsNotNull(onSuccess, "onSuccess");
                Check.Argument.IsNotNull(onFailure, "onFailure");

                GetSocialImpl.RemoveFriend(userId, onSuccess, onFailure);
            }

            /// <summary>
            /// Remove a list of users for list of current user's friends.
            /// </summary>
            /// <param name="providerId">An auth identity provider id for which user ids will be provided. Can be "facebook", or any custom value.</param>
            /// <param name="providerUserIds">A list of user ids on the selected identity provider that need to be added to the current user's friends list.</param>
            /// <param name="onSuccess">Called if removing friends was successful. Parameter contains number of friends.</param>
            /// <param name="onFailure">Called if removing friends failed.</param>
            public static void RemoveFriendsByAuthIdentities(string providerId, List<string> providerUserIds,
                Action<int> onSuccess, Action<GetSocialError> onFailure)
            {
                Check.Argument.IsNotNull(providerId, "providerId");
                Check.Argument.IsNotNull(providerUserIds, "providerUserIds");
                Check.Argument.IsNotNull(onSuccess, "onSuccess");
                Check.Argument.IsNotNull(onFailure, "onFailure");
            
                GetSocialImpl.RemoveFriendsByAuthIdentities(providerId, providerUserIds, onSuccess, onFailure);
            }

            /// <summary>
            /// Check if user is your friend.
            /// </summary>
            /// <param name="userId">Unique user identifier.</param>
            /// <param name="onSuccess">Called with true, if user is your friend, false otherwise.</param>
            /// <param name="onFailure">Called if operation failed.</param>
            public static void IsFriend(string userId, Action<bool> onSuccess, Action<GetSocialError> onFailure)
            {
                Check.Argument.IsStrNotNullOrEmpty(userId, "userId");
                Check.Argument.IsNotNull(onSuccess, "onSuccess");
                Check.Argument.IsNotNull(onFailure, "onFailure");

                GetSocialImpl.IsFriend(userId, onSuccess, onFailure);
            }

            /// <summary>
            /// Get a count of friends for current user.
            /// </summary>
            /// <param name="onSuccess">Called with number of your friends.</param>
            /// <param name="onFailure">Called if operation failed.</param>
            public static void GetFriendsCount(Action<int> onSuccess, Action<GetSocialError> onFailure)
            {
                Check.Argument.IsNotNull(onSuccess, "onSuccess");
                Check.Argument.IsNotNull(onFailure, "onFailure");

                GetSocialImpl.GetFriendsCount(onSuccess, onFailure);
            }

            /// <summary>
            /// Get a list of friends for current user.
            /// </summary>
            /// <param name="offset">Position from which start.</param>
            /// <param name="limit">Limit of users.</param>
            /// <param name="onSuccess">Called with list of your friends.</param>
            /// <param name="onFailure">Called if operation failed.</param>
            public static void GetFriends(int offset, int limit, Action<List<PublicUser>> onSuccess, Action<GetSocialError> onFailure)
            {
                Check.Argument.IsNotNull(onSuccess, "onSuccess");
                Check.Argument.IsNotNull(onFailure, "onFailure");

                GetSocialImpl.GetFriends(offset, limit, onSuccess, onFailure);
            }

            /// <summary>
            /// Get a list of friends references for current user. 
            /// <see cref="UserReference"/> is a lightweight version of <see cref="PublicUser"/>.
            /// </summary>
            /// <param name="onSuccess">Called with list of your friends.</param>
            /// <param name="onFailure">Called if operation failed.</param>
            public static void GetFriendsReferences(Action<List<UserReference>> onSuccess,
                Action<GetSocialError> onFailure)
            {
                Check.Argument.IsNotNull(onSuccess, "onSuccess");
                Check.Argument.IsNotNull(onFailure, "onFailure");

                GetSocialImpl.GetFriendsReferences(onSuccess, onFailure);
            }
            
            /// <summary>
            /// Get a list of suggested friends for current user.
            /// Read more in <a href="http://docs.getsocial.im/guides/social-graph/unity/querying-social-graph/#suggested-friends">documentation</a>.
            /// </summary>
            /// <param name="offset">Position from which start.</param>
            /// <param name="limit">Limit of users.</param>
            /// <param name="onSuccess">Called with list of your suggested friends.</param>
            /// <param name="onFailure">Called if operation failed.</param>
            public static void GetSuggestedFriends(int offset, int limit, Action<List<SuggestedFriend>> onSuccess, Action<GetSocialError> onFailure)
            {
                Check.Argument.IsNotNull(onSuccess, "onSuccess");
                Check.Argument.IsNotNull(onFailure, "onFailure");

                GetSocialImpl.GetSuggestedFriends(offset, limit, onSuccess, onFailure);
            }
            

            /// <summary>
            /// Register a listener to be notified when the user was changed.
            /// </summary>
            /// <param name="onUserChanged">
            /// Called when:<br/>
            /// - SDK initialization is finished;<br/>
            /// - <see cref="SwitchUser(AuthIdentity, System.Action, System.Action{GetSocialSdk.Core.GetSocialError})"/>
            /// method was called and user was successfully changed.
            /// </param>
            /// <returns><c>true</c>, if the operation was successful, <c>false</c> otherwise.</returns>
            public static bool SetOnUserChangedListener(Action onUserChanged)
            {
                Check.Argument.IsNotNull(onUserChanged, "onUserChanged");

                return GetSocialImpl.SetOnUserChangedListener(onUserChanged);
            }

            /// <summary>
            /// Remove current OnUserChanged listener.
            /// </summary>
            /// <returns><c>true</c>, if the operation was successful, <c>false</c> otherwise.</returns>
            public static bool RemoveOnUserChangedListener()
            {
                return GetSocialImpl.RemoveOnUserChangedListener();
            }

            #region Notifications

            /// <summary>
            /// Get a list of notifications for current user.
            /// </summary>
            /// <param name="query">Notifications query.</param>
            /// <param name="onSuccess">Called with a list of notifications.</param>
            /// <param name="onError">Called if operation failed.</param>
            public static void GetNotifications(NotificationsQuery query, Action<List<Notification>> onSuccess,
                Action<GetSocialError> onError)
            {
                Check.Argument.IsNotNull(query, "query");
                Check.Argument.IsNotNull(onSuccess, "onSuccess");
                Check.Argument.IsNotNull(onError, "onError");
                
                GetSocialImpl.GetNotifications(query, onSuccess, onError);
            }

            /// <summary>
            /// Get a number of notifications for current user.
            /// </summary>
            /// <param name="query">Notifications count query.</param>
            /// <param name="onSuccess">Called with a number of notifications.</param>
            /// <param name="onError">Called if operation failed.</param>
            public static void GetNotificationsCount(NotificationsCountQuery query, Action<int> onSuccess,
                Action<GetSocialError> onError)
            {
                Check.Argument.IsNotNull(query, "query");
                Check.Argument.IsNotNull(onSuccess, "onSuccess");
                Check.Argument.IsNotNull(onError, "onError");
                
                GetSocialImpl.GetNotificationsCount(query, onSuccess, onError);
            }

            /// <summary>
            /// Set notifications read or unread.
            /// </summary>
            /// <param name="notificationsIds">List of notifications IDs to change the read status.</param>
            /// <param name="onSuccess">Called if operation succeeded.</param>
            /// <param name="onError">Called if operation failed.</param>
            public static void SetNotificationsRead(List<string> notificationsIds, bool isRead, Action onSuccess,
                Action<GetSocialError> onError)
            {
                Check.Argument.IsNotNull(notificationsIds, "notificationsIds");
                Check.Argument.IsNotNull(onSuccess, "onSuccess");
                Check.Argument.IsNotNull(onError, "onError");
                
                GetSocialImpl.SetNotificationsRead(notificationsIds, isRead, onSuccess, onError);
            }

            /// <summary>
            /// If set to `false` - current user won't receive GetSocial notifications anymore, until called with `true`.
            /// </summary>
            /// <param name="isEnabled">Disabled or enable PNs.</param>
            /// <param name="onSuccess">Called if operation succeeded.</param>
            /// <param name="onError">Called if operation failed.</param>
            public static void SetPushNotificationsEnabled(bool isEnabled, Action onSuccess,
                Action<GetSocialError> onError)
            {
                Check.Argument.IsNotNull(onSuccess, "onSuccess");
                Check.Argument.IsNotNull(onError, "onError");
                
                GetSocialImpl.SetPushNotificationsEnabled(isEnabled, onSuccess, onError);
            }
            
            /// <summary>
            /// Check if PNs are enabled for current user.
            /// </summary>
            /// <param name="onSuccess">Called with true, if Push Notifications are enabled, false otherwise.</param>
            /// <param name="onError">Called if operation failed.</param>
            public static void IsPushNotificationsEnabled(Action<bool> onSuccess,
                Action<GetSocialError> onError)
            {
                Check.Argument.IsNotNull(onSuccess, "onSuccess");
                Check.Argument.IsNotNull(onError, "onError");
                
                GetSocialImpl.IsPushNotificationsEnabled(onSuccess, onError);
            }
            #endregion

        }

        #endregion
    }
}
#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GetSocialSdk.Core
{
    class GetSocialNativeBridgeAndroid : IGetSocialNativeBridge
    {
        private const string GetSocialClassSignature = "im.getsocial.sdk.GetSocial";
        private const string GetSocialUserClassSignature = GetSocialClassSignature + "$User";
        private const string AndroidAccessHelperClass = "im.getsocial.sdk.GetSocialAccessHelper";

        public GetSocialFactory.AvailableRuntimes[] RuntimeImplementation
        {
            get { return new[] {GetSocialFactory.AvailableRuntimes.Android}; }
        }

        static IGetSocialNativeBridge _instance;

        readonly AndroidJavaClass _getSocial;
        readonly AndroidJavaClass _user;

        public GetSocialNativeBridgeAndroid()
        {
            _getSocial = new AndroidJavaClass(GetSocialClassSignature);
            _user = new AndroidJavaClass(GetSocialUserClassSignature);
        }

        #region initialization

        public void Init(string appId)
        {
            _getSocial.CallStatic("init", appId);
        }

        public void WhenInitialized(Action action)
        {
            _getSocial.CallStatic("whenInitialized", new RunnableProxy(action));
        }

        public bool IsInitialized
        {
            get { return _getSocial.CallStaticBool("isInitialized"); }
        }

        public string GetNativeSdkVersion()
        {
            return _getSocial.CallStaticStr("getSdkVersion");
        }

        public string GetLanguage()
        {
            return _getSocial.CallStaticStr("getLanguage");
        }

        public bool SetLanguage(string languageCode)
        {
            return _getSocial.CallStaticBool("setLanguage", languageCode);
        }

        #endregion

        #region smart_invites

        public bool IsInviteChannelAvailable(string channelId)
        {
            return _getSocial.CallStaticBool("isInviteChannelAvailable", channelId);
        }

        public InviteChannel[] InviteChannels
        {
            get
            {
                var channelsJavaList = _getSocial.CallStaticAJO("getInviteChannels");
                var channelsAJOs = channelsJavaList.FromJavaList();
                var channels = channelsAJOs.ConvertAll(ajo => new InviteChannel().ParseFromAJO(ajo)).ToArray();

                return channels;
            }
        }

        public void SendInvite(string channelId, Action onComplete, Action onCancel, Action<GetSocialError> onFailure)
        {
            _getSocial.CallStatic("sendInvite", channelId, new InviteCallbackProxy(onComplete, onCancel, onFailure));
        }

        public void SendInvite(string channelId, InviteContent customInviteContent, Action onComplete, Action onCancel,
            Action<GetSocialError> onFailure)
        {
            var inviteContentAjo = customInviteContent == null ? null : customInviteContent.ToAjo();

            _getSocial.CallStatic("sendInvite", channelId, inviteContentAjo,
                new InviteCallbackProxy(onComplete, onCancel, onFailure));
        }

        public void SendInvite(string channelId, InviteContent customInviteContent,
            LinkParams linkParams,
            Action onComplete, Action onCancel, Action<GetSocialError> onFailure)
        {
            var inviteContentAjo = customInviteContent == null ? null : customInviteContent.ToAjo();
            var linkParamsAjo = linkParams == null ? null : linkParams.ToAjo();
            
            _getSocial.CallStatic("sendInvite", channelId, inviteContentAjo, linkParamsAjo,
                new InviteCallbackProxy(onComplete, onCancel, onFailure));
        }

        public bool RegisterInviteChannelPlugin(string channelId, InviteChannelPlugin inviteChannelPlugin)
        {
            return _getSocial.CallStaticBool("registerInviteChannelPlugin", channelId, CreateAdapter(inviteChannelPlugin));
        }

        public void GetReferralData(Action<ReferralData> onSuccess, Action<GetSocialError> onFailure)
        {
            _getSocial.CallStatic("getReferralData",
                new FetchReferralDataCallbackProxy(onSuccess, onFailure));
        }

        public void GetReferredUsers(Action<List<ReferredUser>> onSuccess, Action<GetSocialError> onFailure)
        {
            _getSocial.CallStatic("getReferredUsers", new ListCallbackProxy<ReferredUser>(onSuccess, onFailure));
        }

        public void CreateInviteLink(LinkParams linkParams, Action<string> onSuccess, Action<GetSocialError> onFailure)
        {
            var linkParamsAjo = linkParams == null ? null : linkParams.ToAjo();
            _getSocial.CallStatic("createInviteLink", linkParamsAjo, new StringCallbackProxy(onSuccess, onFailure));
        }

        public void RegisterForPushNotifications()
        {
            _getSocial.CallStatic("registerForPushNotifications");
        }

        public void SetNotificationListener(Func<Notification, bool, bool> listener)
        {
            _getSocial.CallStatic("setNotificationListener", new NotificationListenerProxy(listener));
        }

        public void GetNotifications(NotificationsQuery query, Action<List<Notification>> onSuccess, Action<GetSocialError> onError)
        {
            _user.CallStatic("getNotifications", query.ToAjo(), new ListCallbackProxy<Notification>(onSuccess, onError));
        }

        public void GetNotificationsCount(NotificationsCountQuery query, Action<int> onSuccess, Action<GetSocialError> onError)
        {
            _user.CallStatic("getNotificationsCount", query.ToAjo(), new IntCallbackProxy(onSuccess, onError));
        }

        public void SetNotificationsRead(List<string> notificationsIds, bool isRead, Action onSuccess, Action<GetSocialError> onError)
        {
            _user.CallStatic("setNotificationsRead", notificationsIds.ToJavaList(), isRead, new CompletionCallback(onSuccess, onError));
        }

        public void SetPushNotificationsEnabled(bool isEnabled, Action onSuccess, Action<GetSocialError> onError)
        {
            _user.CallStatic("setPushNotificationsEnabled", isEnabled, new CompletionCallback(onSuccess, onError));
        }

        public void IsPushNotificationsEnabled(Action<bool> onSuccess, Action<GetSocialError> onError)
        {
            _user.CallStatic("isPushNotificationsEnabled", new BoolCallbackProxy (onSuccess, onError));
        }

        #endregion

        public bool SetGlobalErrorListener(Action<GetSocialError> onError)
        {
            return _getSocial.CallStaticBool("setGlobalErrorListener", new GlobalErrorListenerProxy(onError));
        }

        public bool RemoveGlobalErrorListener()
        {
            return _getSocial.CallStaticBool("removeGlobalErrorListener");
        }

        #region user_management

        public string UserId
        {
            get { return _user.CallStaticStr("getId"); }
        }

        public bool IsUserAnonymous
        {
            get { return _user.CallStaticBool("isAnonymous"); }
        }

        public void ResetUser(Action onSuccess, Action<GetSocialError> onError)
        {
            _user.CallStatic("reset", new CompletionCallback(onSuccess, onError)); 
        }

        public Dictionary<string, string> UserAuthIdentities
        {
            get { return _user.CallStaticAJO("getAuthIdentities").FromJavaHashMap(); }
        }

        public Dictionary<string, string> AllPublicProperties 
        {
            get { return _user.CallStaticAJO("getAllPublicProperties").FromJavaHashMap(); }
        }

        public Dictionary<string, string> AllPrivateProperties
        {
            get { return _user.CallStaticAJO("getAllPrivateProperties").FromJavaHashMap(); }
        }

        public string DisplayName
        {
            get { return _user.CallStaticStr("getDisplayName"); }
        }

        public void SetDisplayName(string displayName, Action onComplete, Action<GetSocialError> onFailure)
        {
            _user.CallStatic("setDisplayName", displayName, new CompletionCallback(onComplete, onFailure));
        }

        public string AvatarUrl
        {
            get { return _user.CallStaticStr("getAvatarUrl"); }
        }

        public void SetAvatarUrl(string avatarUrl, Action onComplete, Action<GetSocialError> onFailure)
        {
            _user.CallStaticSafe("setAvatarUrl", avatarUrl, new CompletionCallback(onComplete, onFailure));
        }

        public void SetAvatar(Texture2D avatar, Action onComplete, Action<GetSocialError> onFailure)
        {
            _user.CallStatic("setAvatar", avatar.ToAjoBitmap(), new CompletionCallback(onComplete, onFailure));
        }

        public void SetPublicProperty(string key, string value, Action onSuccess, Action<GetSocialError> onFailure)
        {
            _user.CallStaticSafe("setPublicProperty", key, value, new CompletionCallback(onSuccess, onFailure));
        }

        public void SetPrivateProperty(string key, string value, Action onSuccess, Action<GetSocialError> onFailure)
        {
            _user.CallStaticSafe("setPrivateProperty", key, value, new CompletionCallback(onSuccess, onFailure));
        }

        public void RemovePublicProperty(string key, Action onSuccess, Action<GetSocialError> onFailure)
        {
            _user.CallStaticSafe("removePublicProperty", key, new CompletionCallback(onSuccess, onFailure));
        }

        public void RemovePrivateProperty(string key, Action onSuccess, Action<GetSocialError> onFailure)
        {
            _user.CallStaticSafe("removePrivateProperty", key, new CompletionCallback(onSuccess, onFailure));
        }

        public string GetPublicProperty(string key)
        {
            return _user.CallStaticStr("getPublicProperty", key);
        }

        public string GetPrivateProperty(string key)
        {
            return _user.CallStaticStr("getPrivateProperty", key);
        }

        public bool HasPublicProperty(string key)
        {
            return _user.CallStaticBool("hasPublicProperty", key);
        }

        public bool HasPrivateProperty(string key)
        {
            return _user.CallStaticBool("hasPrivateProperty", key);
        }

        public void AddAuthIdentity(AuthIdentity identity,
            Action onComplete,
            Action<GetSocialError> onFailure,
            Action<ConflictUser> onConflict)
        {
            _user.CallStatic("addAuthIdentity", identity.ToAjo(),
                new AddAuthIdentityCallbackProxy(onComplete, onFailure, onConflict));
        }

        public void SwitchUser(AuthIdentity identity, Action onSuccess,
            Action<GetSocialError> onFailure)
        {
            _user.CallStatic("switchUser", identity.ToAjo(), new CompletionCallback(onSuccess, onFailure));
        }

        public void RemoveAuthIdentity(string providerId, Action onSuccess, Action<GetSocialError> onFailure)
        {
            _user.CallStatic("removeAuthIdentity", providerId, new CompletionCallback(onSuccess, onFailure));
        }

        public bool SetOnUserChangedListener(Action onUserChanged)
        {
            return _user.CallStaticBool("setOnUserChangedListener", new OnUserChangedListenerProxy(onUserChanged));
        }

        public bool RemoveOnUserChangedListener()
        {
            return _user.CallStaticBool("removeOnUserChangedListener");
        }
        
        public void GetUserById(string userId, Action<PublicUser> onSuccess, Action<GetSocialError> onFailure)
        {
            _getSocial.CallStatic("getUserById", userId, new CallbackProxy<PublicUser>(onSuccess, onFailure));
        }

        public void GetUserByAuthIdentity(string providerId, string providerUserId, Action<PublicUser> onSuccess, Action<GetSocialError> onFailure)
        {
            _getSocial.CallStatic("getUserByAuthIdentity", providerId, providerUserId, new CallbackProxy<PublicUser>(onSuccess, onFailure));
        }

        public void GetUsersByAuthIdentities(string providerId, List<string> providerUserIds, Action<Dictionary<string, PublicUser>> onSuccess, Action<GetSocialError> onFailure)
        {
            _getSocial.CallStatic("getUsersByAuthIdentities", providerId, providerUserIds.ToJavaList(), new DictionaryCallbackProxy<PublicUser>(onSuccess, onFailure));
        }

        public void FindUsers(UsersQuery query, Action<List<UserReference>> onSuccess, Action<GetSocialError> onFailure)
        {
            _getSocial.CallStatic("findUsers", query.ToAjo(), new ListCallbackProxy<UserReference>(onSuccess, onFailure));
        }

        #endregion

        #region social_graph

        public void AddFriend (string userId, Action<int> onSuccess, Action<GetSocialError> onFailure)
        {
            _user.CallStatic ("addFriend", userId, new IntCallbackProxy (onSuccess, onFailure));
        }

        public void AddFriendsByAuthIdentities(string providerId, List<string> providerUserIds, Action<int> onSuccess, Action<GetSocialError> onFailure)
        {
            _user.CallStatic ("addFriendsByAuthIdentities", providerId, providerUserIds.ToJavaList(), new IntCallbackProxy (onSuccess, onFailure));
        }

        public void RemoveFriend (string userId, Action<int> onSuccess, Action<GetSocialError> onFailure)
        {
            _user.CallStatic ("removeFriend", userId, new IntCallbackProxy (onSuccess, onFailure));
        }

        public void RemoveFriendsByAuthIdentities(string providerId, List<string> providerUserIds, Action<int> onSuccess, Action<GetSocialError> onFailure)
        {
            _user.CallStatic ("removeFriendsByAuthIdentities", providerId, providerUserIds.ToJavaList(), new IntCallbackProxy (onSuccess, onFailure));
        }

        public void SetFriends(List<string> userIds, Action onSuccess, Action<GetSocialError> onFailure)
        {
            _getSocial.CallStatic("setFriends", userIds, new CompletionCallback(onSuccess, onFailure));
        }

        public void SetFriendsByAuthIdentities(string providerId, List<string> providerUserIds, Action onSuccess, Action<GetSocialError> onFailure)
        {
            _getSocial.CallStatic("setFriendsByAuthIdentities", providerId, providerUserIds.ToJavaList(), new CompletionCallback(onSuccess, onFailure));
        }

        public void IsFriend (string userId, Action<bool> onSuccess, Action<GetSocialError> onFailure)
        {
            _user.CallStatic ("isFriend", userId, new BoolCallbackProxy (onSuccess, onFailure));
        }

        public void GetFriendsCount(Action<int> onSuccess, Action<GetSocialError> onFailure)
        {
            _user.CallStatic("getFriendsCount", new IntCallbackProxy(onSuccess, onFailure));
        }

        public void GetFriends (int offset, int limit, Action<List<PublicUser>> onSuccess, Action<GetSocialError> onFailure)
        {
            _user.CallStatic ("getFriends", offset, limit, new ListCallbackProxy<PublicUser> (onSuccess, onFailure));
        }

        public void GetSuggestedFriends(int offset, int limit, Action<List<SuggestedFriend>> onSuccess, Action<GetSocialError> onFailure)
        {
            _user.CallStatic ("getSuggestedFriends", offset, limit, new ListCallbackProxy<SuggestedFriend> (onSuccess, onFailure));
        }

        public void GetFriendsReferences(Action<List<UserReference>> onSuccess, Action<GetSocialError> onFailure)
        {
            _user.CallStatic("getFriendsReferences", new ListCallbackProxy<UserReference>(onSuccess, onFailure));
        }

        #endregion

        #region activity_feed

        public void GetAnnouncements(string feed, Action<List<ActivityPost>> onSuccess,
            Action<GetSocialError> onFailure)
        {
            _getSocial.CallStatic("getAnnouncements", feed, new ListCallbackProxy<ActivityPost>(onSuccess, onFailure));
        }

        public void GetActivities(ActivitiesQuery query, Action<List<ActivityPost>> onSuccess,
            Action<GetSocialError> onFailure)
        {
            _getSocial.CallStatic("getActivities", query.ToAjo(), new ListCallbackProxy<ActivityPost>(onSuccess, onFailure));
        }

        public void GetActivity(string activityId, Action<ActivityPost> onSuccess, Action<GetSocialError> onFailure)
        {
            _getSocial.CallStatic("getActivity", activityId, new CallbackProxy<ActivityPost>(onSuccess, onFailure));
        }

        public void PostActivityToFeed(string feed, ActivityPostContent content, Action<ActivityPost> onSuccess, Action<GetSocialError> onFailure)
        {
            _getSocial.CallStatic("postActivityToFeed", feed, content.ToAjo(), new CallbackProxy<ActivityPost>(onSuccess, onFailure));
        }

        public void PostCommentToActivity(string activityId, ActivityPostContent comment, Action<ActivityPost> onSuccess,
            Action<GetSocialError> onFailure)
        {
            _getSocial.CallStatic("postCommentToActivity", activityId, comment.ToAjo(), new CallbackProxy<ActivityPost>(onSuccess, onFailure));
        }

        public void LikeActivity(string activityId, bool isLiked, Action<ActivityPost> onSuccess,
            Action<GetSocialError> onFailure)
        {
            _getSocial.CallStatic("likeActivity", activityId, isLiked, new CallbackProxy<ActivityPost>(onSuccess, onFailure));
        }

        public void GetActivityLikers(string activityId, int offset, int limit, Action<List<PublicUser>> onSuccess,
            Action<GetSocialError> onFailure)
        {
            _getSocial.CallStatic("getActivityLikers", activityId, offset, limit, new ListCallbackProxy<PublicUser>(onSuccess, onFailure));
        }

        public void ReportActivity(string activityId, ReportingReason reportingReason, Action onSuccess, Action<GetSocialError> onFailure)
        {
            _getSocial.CallStatic("reportActivity", activityId, reportingReason.ToAndroidJavaObject(), new CompletionCallback(onSuccess, onFailure));
        }

        public void DeleteActivity(string activityId, Action onSuccess, Action<GetSocialError> onFailure)
        {
            _getSocial.CallStatic("deleteActivity", activityId, new CompletionCallback(onSuccess, onFailure));
        }

        #endregion

        #region access_helpers

        public void HandleOnStartUnityEvent()
        {
            _getSocial.CallStatic("handleOnStartUnityEvent");
        }

        private static AndroidJavaObject CreateAdapter(InviteChannelPlugin plugin)
        {
            return plugin == null ? null : new AndroidJavaObject("im.getsocial.sdk.internal.unity.InviteChannelPluginAdapter", new InviteChannelPluginProxy(plugin));
        }

        public void Reset()
        {
            try
            {
                var activity = JniUtils.Activity;
                using (var ajc = new AndroidJavaClass(AndroidAccessHelperClass))
                {
                    ajc.CallStatic("reset", activity.CallAJO("getApplication"));
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Resetting user failed");
                Debug.LogException(e);
            }
        }
        #endregion
    }
}

#endif

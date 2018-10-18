using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace GetSocialSdk.Core
{
    internal partial class GetSocialNativeBridgeMock : IGetSocialNativeBridge
    {

        public GetSocialFactory.AvailableRuntimes[] RuntimeImplementation
        {
            get { return new[] {GetSocialFactory.AvailableRuntimes.Mock}; }
        }

        const string Mock = "mock";

        static IGetSocialNativeBridge _instance;

        static readonly Dictionary<string, string> EmptyIdentities = new Dictionary<string, string>();
        static readonly InviteChannel[] EmptyChannels = { };

        public static IGetSocialNativeBridge Instance
        {
            get { return _instance ?? (_instance = new GetSocialNativeBridgeMock()); }
        }

        public void Init(string appId)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), appId);
        }

        public void WhenInitialized(Action action)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), action);
        }

        public bool IsInitialized
        {
            get { return false; }
        }

        public string GetNativeSdkVersion()
        {
            return "Not available in Editor";
        }

        public string GetLanguage()
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod());
            return null;
        }

        public bool SetLanguage(string languageCode)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), languageCode);
            return false;
        }

        public string Language { get; set; }

        public bool IsInviteChannelAvailable(string channelId)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), channelId);
            return false;
        }

        public InviteChannel[] InviteChannels
        {
            get { return EmptyChannels; }
        }

        public void SendInvite(string channelId, Action onComplete, Action onCancel,
            Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), channelId, onComplete, onCancel, onFailure);
        }

        public void SendInvite(string channelId, InviteContent customInviteContent, Action onComplete, Action onCancel,
            Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), channelId, customInviteContent,
                onComplete, onCancel, onFailure);
        }

        public void SendInvite(string channelId, InviteContent customInviteContent,
            LinkParams linkParams,
            Action onComplete, Action onCancel, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), channelId, customInviteContent, linkParams,
                onComplete, onCancel, onFailure);
        }

        public bool RegisterInviteChannelPlugin(string channelId, InviteChannelPlugin inviteChannelPlugin)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), channelId, inviteChannelPlugin);
            return false;
        }

        public void GetReferralData(Action<ReferralData> onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), onSuccess, onFailure);
        }

        public void GetReferredUsers(Action<List<ReferredUser>> onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), onSuccess, onFailure);
        }

        public void CreateInviteLink(LinkParams linkParams, Action<string> onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), onSuccess, onFailure);
        }

        public void RegisterForPushNotifications()
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod());
        }

        public void SetNotificationListener(Func<Notification, bool, bool> listener)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), listener);
        }

        public void GetNotifications(NotificationsQuery query, Action<List<Notification>> onSuccess, Action<GetSocialError> onError)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), query, onSuccess, onError);
        }

        public void GetNotificationsCount(NotificationsCountQuery query, Action<int> onSuccess, Action<GetSocialError> onError)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), query, onSuccess, onError);
        }

        public void SetNotificationsRead(List<string> notificationsIds, bool isRead, Action onSuccess,
            Action<GetSocialError> onError)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), notificationsIds, isRead, onSuccess, onError);
        }

        public void SetPushNotificationsEnabled(bool isEnabled, Action onSuccess, Action<GetSocialError> onError)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), isEnabled, onSuccess, onError);
        }

        public void IsPushNotificationsEnabled(Action<bool> onSuccess, Action<GetSocialError> onError)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), onSuccess, onError);
        }

        public bool SetOnUserChangedListener(Action listener)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), listener);
            return true;
        }

        public bool RemoveOnUserChangedListener()
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod());
            return true;
        }

        public bool SetGlobalErrorListener(Action<GetSocialError> onError)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), onError);
            return false;
        }

        public bool RemoveGlobalErrorListener()
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod());
            return false;
        }

        public string UserId
        {
            get { return string.Empty; }
        }

        public bool IsUserAnonymous
        {
            get { return true; }
        }

        public void ResetUser(Action onSuccess, Action<GetSocialError> onError)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), onSuccess, onError);
            onSuccess();
        }

        public Dictionary<string, string> UserAuthIdentities
        {
            get { return EmptyIdentities; }
        }

        public Dictionary<string, string> AllPublicProperties {
            get { return new Dictionary<string, string>(); }
        }

        public Dictionary<string, string> AllPrivateProperties {
            get { return new Dictionary<string, string>(); }
        }

        public string DisplayName
        {
            get { return ""; }
        }

        public void SetDisplayName(string displayName, Action onComplete, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), displayName, onComplete, onFailure);
        }

        public string AvatarUrl
        {
            get { return ""; }
        }

        public void SetAvatarUrl(string avatarUrl, Action onComplete, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), avatarUrl, onComplete, onFailure);
        }

        public void SetAvatar(Texture2D avatar, Action onComplete, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), avatar, onComplete, onFailure);
        }

        public void SetPublicProperty(string key, string value, Action onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), key, value, onSuccess, onFailure);
        }

        public void SetPrivateProperty(string key, string value, Action onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), key, value, onSuccess, onFailure);
        }

        public void RemovePublicProperty(string key, Action onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), key, onSuccess, onFailure);
        }

        public void RemovePrivateProperty(string key, Action onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), key, onSuccess, onFailure);
        }

        public string GetPublicProperty(string key)
        {
            return "";
        }

        public string GetPrivateProperty(string key)
        {
            return "";
        }

        public bool HasPublicProperty(string key)
        {
            return false;
        }

        public bool HasPrivateProperty(string key)
        {
            return false;
        }

        public void AddAuthIdentity(AuthIdentity authIdentity, Action onComplete, Action<GetSocialError> onFailure, Action<ConflictUser> onConflict)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), authIdentity, onComplete,
                onFailure, onConflict);
        }

        public void GetUserById(string userId, Action<PublicUser> onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), userId, onSuccess, onFailure);
        }

        public void GetUserByAuthIdentity(string providerId, string providerUserId, Action<PublicUser> onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), providerId, onSuccess,
                onFailure);
        }

        public void GetUsersByAuthIdentities(string providerId, List<string> providerUserIds, Action<Dictionary<string, PublicUser>> onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), providerId, providerUserIds, onSuccess,
                onFailure);
        }

        public void FindUsers(UsersQuery query, Action<List<UserReference>> onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), query, onSuccess, onFailure);
        }

        public void RemoveAuthIdentity(string providerId, Action onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), providerId, onSuccess, onFailure);
        }

        public void SwitchUser(AuthIdentity authIdentity, Action onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(),  authIdentity, onSuccess,
                onFailure);
        }

        public void AddFriend (string userId, Action<int> onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(),  userId, onSuccess,
                onFailure);
        }

        public void AddFriendsByAuthIdentities(string providerId, List<string> providerUserIds, Action<int> onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), providerId, providerUserIds, onSuccess,
                onFailure);
        }

        public void RemoveFriend (string userId, Action<int> onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(),  userId, onSuccess,
                onFailure);
        }

        public void RemoveFriendsByAuthIdentities(string providerId, List<string> providerUserIds, Action<int> onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), providerId, providerUserIds, onSuccess,
                onFailure);
        }

        public void SetFriends(List<string> userIds, Action onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), userIds, onSuccess,
                onFailure);
        }

        public void SetFriendsByAuthIdentities(string providerId, List<string> providerUserIds, Action onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), providerId, providerUserIds, onSuccess,
                onFailure);
        }

        public void IsFriend (string userId, Action<bool> onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(),  userId, onSuccess,
                onFailure);
        }

        public void GetFriendsCount(Action<int> onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), onSuccess,
                onFailure);
        }

        public void GetFriends (int offset, int limit, Action<List<PublicUser>> onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), offset, limit, onSuccess,
                onFailure);
        }

        public void GetSuggestedFriends(int offset, int limit, Action<List<SuggestedFriend>> onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), offset, limit, onSuccess,
                onFailure);
        }

        public void GetFriendsReferences(Action<List<UserReference>> onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), onSuccess,
                onFailure);
        }

        public void GetAnnouncements(string feed, Action<List<ActivityPost>> onSuccess,
            Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), feed, onSuccess, onFailure);
        }

        public void GetActivities(ActivitiesQuery query, Action<List<ActivityPost>> onSuccess,
            Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), query, onSuccess, onFailure);
        }

        public void GetActivity(string activityId, Action<ActivityPost> onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), activityId, onSuccess, onFailure);
        }

        public void PostActivityToFeed(string feed, ActivityPostContent content, Action<ActivityPost> onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), content, onSuccess, onFailure);
        }

        public void PostCommentToActivity(string activityId, ActivityPostContent comment, Action<ActivityPost> onSuccess,
            Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), activityId, comment, onSuccess, onFailure);
        }

        public void LikeActivity(string activityId, bool isLiked, Action<ActivityPost> onSuccess,
            Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), activityId, isLiked, onSuccess, onFailure);
        }

        public void GetActivityLikers(string activityId, int offset, int limit, Action<List<PublicUser>> onSuccess,
            Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), offset, limit, onSuccess, onFailure);
        }

        public void ReportActivity(string activityId, ReportingReason reportingReason, Action onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), activityId, reportingReason, onSuccess, onFailure);
        }

        public void DeleteActivity(string activityId, Action onSuccess, Action<GetSocialError> onFailure)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), activityId, onSuccess, onFailure);
        }

        public void Reset()
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod());
        }

        public void HandleOnStartUnityEvent()
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod());
        }
    }
}
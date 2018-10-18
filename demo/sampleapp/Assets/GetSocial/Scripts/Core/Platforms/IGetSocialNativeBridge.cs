using System;
using System.Collections.Generic;
using UnityEngine;

namespace GetSocialSdk.Core
{
    partial interface IGetSocialNativeBridge
    {

        GetSocialFactory.AvailableRuntimes[] RuntimeImplementation { get; }
        
        #region initialization
        
        void Init(string appId);

        void WhenInitialized(Action action);

        bool IsInitialized { get; }

        #endregion

        #region general

        string GetNativeSdkVersion();

        string GetLanguage();

        bool SetLanguage(string languageCode);

        bool SetGlobalErrorListener(Action<GetSocialError> onError);

        bool RemoveGlobalErrorListener();

        #endregion


        #region smart_invites

        bool IsInviteChannelAvailable(string channelId);
        
        InviteChannel[] InviteChannels { get; }

        void SendInvite(string channelId, Action onComplete, Action onCancel, Action<GetSocialError> onFailure);

        void SendInvite(string channelId, InviteContent customInviteContent,
            Action onComplete, Action onCancel, Action<GetSocialError> onFailure);

        void SendInvite(string channelId, InviteContent customInviteContent, LinkParams linkParams,
            Action onComplete, Action onCancel, Action<GetSocialError> onFailure);

        bool RegisterInviteChannelPlugin(string channelId, InviteChannelPlugin inviteChannelPlugin);

        void GetReferralData(Action<ReferralData> onSuccess, Action<GetSocialError> onFailure);

        void GetReferredUsers(Action<List<ReferredUser>> onSuccess, Action<GetSocialError> onFailure);

        void CreateInviteLink(LinkParams linkParams, Action<string> onSuccess, Action<GetSocialError> onFailure);

        #endregion

        #region push_notifications

        void RegisterForPushNotifications();

        void SetNotificationListener(Func<Notification, bool, bool> listener);

        void GetNotifications(NotificationsQuery query, Action<List<Notification>> onSuccess, Action<GetSocialError> onError);

        void GetNotificationsCount(NotificationsCountQuery query, Action<int> onSuccess, Action<GetSocialError> onError);
        
        void SetNotificationsRead(List<string> notificationsIds, bool isRead, Action onSuccess,
            Action<GetSocialError> onError);
        
        void SetPushNotificationsEnabled(bool isEnabled, Action onSuccess, Action<GetSocialError> onError);
        
        void IsPushNotificationsEnabled(Action<bool> onSuccess, Action<GetSocialError> onError);
        
        #endregion

        #region user_management

        bool SetOnUserChangedListener(Action listener);

        bool RemoveOnUserChangedListener();

        string UserId { get; }

        bool IsUserAnonymous { get; }
        
        void ResetUser(Action onSuccess, Action<GetSocialError> onError);

        Dictionary<string, string> UserAuthIdentities { get; }
        
        Dictionary<string, string> AllPublicProperties { get; }
        
        Dictionary<string, string> AllPrivateProperties { get; }

        string DisplayName { get; }

        void SetDisplayName(string displayName, Action onComplete, Action<GetSocialError> onFailure);

        string AvatarUrl { get; }

        void SetAvatarUrl(string avatarUrl, Action onComplete, Action<GetSocialError> onFailure);
        
        void SetAvatar(Texture2D avatar, Action onComplete, Action<GetSocialError> onFailure);
        
        void SetPublicProperty(string key, string value, Action onSuccess, Action<GetSocialError> onFailure);

        void SetPrivateProperty(string key, string value, Action onSuccess, Action<GetSocialError> onFailure);

        void RemovePublicProperty(string key, Action onSuccess, Action<GetSocialError> onFailure);

        void RemovePrivateProperty(string key, Action onSuccess, Action<GetSocialError> onFailure);

        string GetPublicProperty(string key);

        string GetPrivateProperty(string key);

        bool HasPublicProperty(string key);

        bool HasPrivateProperty(string key);

        void AddAuthIdentity(AuthIdentity authIdentity,
            Action onComplete, Action<GetSocialError> onFailure, Action<ConflictUser> onConflict);

        void RemoveAuthIdentity(string providerId, Action onSuccess, Action<GetSocialError> onFailure);

        void SwitchUser(AuthIdentity authIdentity,
            Action onSuccess, Action<GetSocialError> onFailure);

        void GetUserById (string userId, Action<PublicUser> onSuccess, Action<GetSocialError> onFailure);

        void GetUserByAuthIdentity (string providerId, string providerUserId, Action<PublicUser> onSuccess, Action<GetSocialError> onFailure);

        void GetUsersByAuthIdentities (string providerId, List<string> providerUserIds, Action<Dictionary<string, PublicUser>> onSuccess, Action<GetSocialError> onFailure);

        void FindUsers(UsersQuery query, Action<List<UserReference>> onSuccess, Action<GetSocialError> onFailure);

        #endregion

        #region social_graph

        void AddFriend (string userId, Action<int> onSuccess, Action<GetSocialError> onFailure);

        void AddFriendsByAuthIdentities (string providerId, List<string> providerUserIds, Action<int> onSuccess, Action<GetSocialError> onFailure);

        void RemoveFriend (string userId, Action<int> onSuccess, Action<GetSocialError> onFailure);

        void RemoveFriendsByAuthIdentities (string providerId, List<string> providerUserIds, Action<int> onSuccess, Action<GetSocialError> onFailure);

        void SetFriends(List<string> userIds, Action onSuccess, Action<GetSocialError> onFailure);

        void SetFriendsByAuthIdentities(string providerId, List<string> providerUserIds, Action onSuccess, Action<GetSocialError> onFailure);

        void IsFriend (string userId, Action<bool> onSuccess, Action<GetSocialError> onFailure);

        void GetFriendsCount(Action<int> onSuccess, Action<GetSocialError> onFailure);

        void GetFriends (int offset, int limit, Action<List<PublicUser>> onSuccess, Action<GetSocialError> onFailure);
        
        void GetSuggestedFriends(int offset, int limit, Action<List<SuggestedFriend>> onSuccess, Action<GetSocialError> onFailure);
        
        void GetFriendsReferences(Action<List<UserReference>> onSuccess, Action<GetSocialError> onFailure);

        #endregion

        #region activity_feed

        void GetAnnouncements(string feed, Action<List<ActivityPost>> onSuccess, Action<GetSocialError> onFailure);

        void GetActivities(ActivitiesQuery query, Action<List<ActivityPost>> onSuccess,
            Action<GetSocialError> onFailure);

        void GetActivity(string activityId, Action<ActivityPost> onSuccess, Action<GetSocialError> onFailure);

        void PostActivityToFeed(string feed, ActivityPostContent content,
            Action<ActivityPost> onSuccess,
            Action<GetSocialError> onFailure);

        void PostCommentToActivity(string activityId, ActivityPostContent comment, Action<ActivityPost> onSuccess,
            Action<GetSocialError> onFailure);

        void LikeActivity(string activityId, bool isLiked, Action<ActivityPost> onSuccess,
            Action<GetSocialError> onFailure);

        void GetActivityLikers(string activityId, int offset, int limit, Action<List<PublicUser>> onSuccess,
            Action<GetSocialError> onFailure);

        void ReportActivity(string activityId, ReportingReason reportingReason, Action onSuccess,
            Action<GetSocialError> onFailure);

        void DeleteActivity(string activityId, Action onSuccess, Action<GetSocialError> onFailure);

        #endregion


        #region access_helpers

        void Reset();

        #endregion

        void HandleOnStartUnityEvent();
    }
}
#if UNITY_IOS
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GetSocialSdk.MiniJSON;
using UnityEngine;

namespace GetSocialSdk.Core
{
    class GetSocialNativeBridgeIOS : IGetSocialNativeBridge
    {
    
        public GetSocialFactory.AvailableRuntimes[] RuntimeImplementation
        {
            get { return new[] {GetSocialFactory.AvailableRuntimes.iOS}; }
        }

        public void Init(string appId)
        {
            _gs_init(appId);
        }

        public void WhenInitialized(Action action)
        {
            _gs_executeWhenInitialized(Callbacks.ActionCallback, action.GetPointer());
        }

        public bool IsInitialized
        {
            get { return _gs_isInitialized(); }
        }

        public string GetNativeSdkVersion()
        {
            return _gs_getNativeSdkVersion();
        }

        public string GetLanguage()
        {
            return _gs_getLanguage();
        }

        public bool SetLanguage(string languageCode)
        {
            return _gs_setLanguage(languageCode);
        }

        #region invites

        public bool IsInviteChannelAvailable(string channelId)
        {
            return _gs_isInviteChannelAvailable(channelId);
        }

        public InviteChannel[] InviteChannels
        {
            get
            {
                var channelsJson = _gs_getInviteChannels();
                GetSocialDebugLogger.D("Invite channels: " + channelsJson);
                return GSJsonUtils.ParseList<InviteChannel>(channelsJson).ToArray();
            }
        }

        public void SendInvite(string channelId, Action onComplete, Action onCancel,
            Action<GetSocialError> onFailure)
        {
           _gs_sendInvite(channelId, Callbacks.ActionCallback, onComplete.GetPointer(), onCancel.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void SendInvite(string channelId, InviteContent customInviteContent,
            Action onComplete, Action onCancel, Action<GetSocialError> onFailure)
        {
            string customInviteContentJson = null;
            if (customInviteContent != null)
            {
                customInviteContentJson = customInviteContent.ToJson();
            }
           _gs_sendInviteCustom(channelId, customInviteContentJson,
                Callbacks.ActionCallback, onComplete.GetPointer(), onCancel.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void SendInvite(string channelId, InviteContent customInviteContent,
            LinkParams linkParams,
            Action onComplete, Action onCancel, Action<GetSocialError> onFailure)
        {
            string customInviteContentJson = null;
            if (customInviteContent != null)
            {
                customInviteContentJson = customInviteContent.ToJson();
            }
            string linkParamsJson = null;
            if (linkParams != null)
            {
                linkParamsJson = linkParams.ToJson();
            }
           _gs_sendInviteCustomAndLinkParams(channelId, customInviteContentJson, linkParamsJson,
                Callbacks.ActionCallback, onComplete.GetPointer(), onCancel.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public bool RegisterInviteChannelPlugin(string channelId, InviteChannelPlugin inviteProviderPlugin)
        {
            return _gs_registerInviteProviderPlugin(channelId, inviteProviderPlugin.GetPointer(),
                InviteChannelPluginCallbacks.IsAvailableForDevice,
                InviteChannelPluginCallbacks.PresentChannelInterface);
        }
        
        #endregion

        public void GetReferralData(Action<ReferralData> onSuccess, Action<GetSocialError> onFailure)
        {
           _gs_getReferralData(
                Callbacks.FetchReferralDataCallback, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer()
            );
        }

        public void GetReferredUsers(Action<List<ReferredUser>> onSuccess, Action<GetSocialError> onFailure)
        {
            _gs_getReferredUsers(Callbacks.GetReferredUsers, onSuccess.GetPointer(), 
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void CreateInviteLink(LinkParams linkParams, Action<string> onSuccess, Action<GetSocialError> onFailure)
        {
            string linkParamsJson = null;
            if (linkParams != null)
            {
                linkParamsJson = linkParams.ToJson();
            }
            
            _gs_createInviteLink(linkParamsJson, Callbacks.StringCallback, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        } 

        public void RegisterForPushNotifications()
        {
           _gs_registerForPushNotifications();
        }

        public void SetNotificationListener(Func<Notification, bool, bool> listener)
        {
            _gs_setNotificationActionListener(listener.GetPointer(), Callbacks.NotificationListener);
        }

        public void GetNotifications(NotificationsQuery query, Action<List<Notification>> onSuccess, Action<GetSocialError> onError)
        {
            _gs_getNotifications(query.ToJson(), Callbacks.GetNotificationsCallback, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onError.GetPointer());
        }

        public void GetNotificationsCount(NotificationsCountQuery query, Action<int> onSuccess, Action<GetSocialError> onError)
        {
            _gs_getNotificationsCount(query.ToJson(), Callbacks.IntCallback, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onError.GetPointer());
        }

        public void SetNotificationsRead(List<string> notificationsIds, bool isRead, Action onSuccess,
            Action<GetSocialError> onError)
        {
            _gs_setNotificationsRead(GSJson.Serialize(notificationsIds), isRead, Callbacks.ActionCallback,
                onSuccess.GetPointer(),
                Callbacks.FailureCallback, onError.GetPointer());
        }

        public void SetPushNotificationsEnabled(bool isEnabled, Action onSuccess, Action<GetSocialError> onError)
        {
            _gs_setPushNotificationsEnabled(isEnabled, Callbacks.ActionCallback, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onError.GetPointer());
        }

        public void IsPushNotificationsEnabled(Action<bool> onSuccess, Action<GetSocialError> onError)
        {
            _gs_isPushNotificationsEnabled(Callbacks.BoolCallback, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onError.GetPointer());
        }

        public bool SetGlobalErrorListener(Action<GetSocialError> onError)
        {
            return _gs_setGlobalErrorListener(onError.GetPointer(), GlobalErrorCallback.OnGlobalError);
        }

        public bool RemoveGlobalErrorListener()
        {
            return _gs_removeGlobalErrorListener();
        }

        #region user_management

        public bool SetOnUserChangedListener(Action listener)
        {
            return _gs_setOnUserChangedListener(listener.GetPointer(), Callbacks.ActionCallback);
        }

        public bool RemoveOnUserChangedListener()
        {
            return _gs_removeOnUserChangedListener();
        }

        public string UserId
        {
            get { return _gs_getUserId(); }
        }

        public bool IsUserAnonymous
        {
            get { return _gs_isUserAnonymous(); }
        }
    
        public void ResetUser(Action onSuccess, Action<GetSocialError> onError)
        {
           _gs_resetUser(
                Callbacks.ActionCallback, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onError.GetPointer()
            );
        }

        public Dictionary<string, string> UserAuthIdentities
        {
            get
            {
                var identitiesJson = _gs_getAuthIdentities();
                return GSJsonUtils.ParseDictionary(identitiesJson);
            }
        }

        public Dictionary<string, string> AllPublicProperties {
            get
            {
                var json = _gs_getAllPublicProperties();
                return GSJsonUtils.ParseDictionary(json);
            }
        }
        
        public Dictionary<string, string> AllPrivateProperties {
            get
            {
                var json = _gs_getAllPrivateProperties();
                return GSJsonUtils.ParseDictionary(json);
            }
        }

        public string DisplayName 
        {
            get { return _gs_getUserDisplayName(); }
        }

        public void SetDisplayName(string displayName, Action onComplete, Action<GetSocialError> onFailure)
        {
           _gs_setUserDisplayName(displayName, Callbacks.ActionCallback, onComplete.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public string AvatarUrl
        {
            get { return _gs_getUserAvatarUrl(); }
        }

        public void SetAvatarUrl(string avatarUrl, Action onComplete, Action<GetSocialError> onFailure)
        {
           _gs_setUserAvatarUrl(avatarUrl, Callbacks.ActionCallback, onComplete.GetPointer(), Callbacks.FailureCallback,
                onFailure.GetPointer());
        }

        public void SetAvatar(Texture2D avatar, Action onComplete, Action<GetSocialError> onFailure)
        {
            _gs_setUserAvatar(avatar.TextureToBase64(), Callbacks.ActionCallback, onComplete.GetPointer(), 
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void SetPublicProperty(string key, string value, Action onSuccess, Action<GetSocialError> onFailure)
        {
           _gs_setPublicProperty(key, value, Callbacks.ActionCallback, onSuccess.GetPointer(), Callbacks.FailureCallback,
                onFailure.GetPointer());
        }

        public void SetPrivateProperty(string key, string value, Action onSuccess, Action<GetSocialError> onFailure)
        {
           _gs_setPrivateProperty(key, value, Callbacks.ActionCallback, onSuccess.GetPointer(), Callbacks.FailureCallback,
                onFailure.GetPointer());
        }

        public void RemovePublicProperty(string key, Action onSuccess, Action<GetSocialError> onFailure)
        {
           _gs_removePublicProperty(key, Callbacks.ActionCallback, onSuccess.GetPointer(), Callbacks.FailureCallback,
                onFailure.GetPointer());
        }

        public void RemovePrivateProperty(string key, Action onSuccess, Action<GetSocialError> onFailure)
        {
           _gs_removePrivateProperty(key, Callbacks.ActionCallback, onSuccess.GetPointer(), Callbacks.FailureCallback,
                onFailure.GetPointer());
        }

        public string GetPublicProperty(string key)
        {
            return _gs_getPublicProperty(key);
        }

        public string GetPrivateProperty(string key)
        {
            return _gs_getPrivateProperty(key);
        }

        public bool HasPublicProperty(string key)
        {
            return _gs_hasPublicProperty(key);
        }

        public bool HasPrivateProperty(string key)
        {
            return _gs_hasPrivateProperty(key);
        }

        public void AddAuthIdentity(AuthIdentity authIdentity, Action onComplete, Action<GetSocialError> onFailure, Action<ConflictUser> onConflict)
        {
           _gs_addAuthIdentity(authIdentity.ToJson(),
                Callbacks.ActionCallback, onComplete.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer(),
                UserConflictCallback.OnUserAuthConflict, onConflict.GetPointer());
        }

        public void RemoveAuthIdentity(string providerId, Action onSuccess, Action<GetSocialError> onFailure)
        {
           _gs_removeAuthIdentity(providerId,
                Callbacks.ActionCallback, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void SwitchUser(AuthIdentity authIdentity, Action onSuccess, Action<GetSocialError> onFailure)
        {
           _gs_switchUser(authIdentity.ToJson(),
                Callbacks.ActionCallback, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void GetUserById(string userId, Action<PublicUser> onSuccess, Action<GetSocialError> onFailure)
        {
            _gs_getUserById(userId, 
                Callbacks.GetPublicUser, onSuccess.GetPointer(), 
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void GetUserByAuthIdentity(string providerId, string providerUserId, Action<PublicUser> onSuccess, Action<GetSocialError> onFailure)
        {
            _gs_getUserByAuthIdentity(providerId, providerUserId, 
                Callbacks.GetPublicUser, onSuccess.GetPointer(), 
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void GetUsersByAuthIdentities(string providerId, List<string> providerUserIds, Action<Dictionary<string, PublicUser>> onSuccess, Action<GetSocialError> onFailure)
        {
            _gs_getUsersByAuthIdentities(providerId, GSJson.Serialize(providerUserIds), 
                Callbacks.GetPublicUser, onSuccess.GetPointer(), 
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void FindUsers(UsersQuery query, Action<List<UserReference>> onSuccess, Action<GetSocialError> onFailure)
        {
            _gs_findUsers(query.ToJson(),
                Callbacks.GetUserReferences, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        #endregion

        #region social_graph

        public void AddFriend (string userId, Action<int> onSuccess, Action<GetSocialError> onFailure)
        {
           _gs_addFriend (userId,
                Callbacks.IntCallback, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void AddFriendsByAuthIdentities(string providerId, List<string> providerUserIds, Action<int> onSuccess, Action<GetSocialError> onFailure)
        {
            _gs_addFriendsByAuthIdentities(providerId, GSJson.Serialize(providerUserIds), 
                Callbacks.IntCallback, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void RemoveFriend (string userId, Action<int> onSuccess, Action<GetSocialError> onFailure)
        {
           _gs_removeFriend (userId,
                Callbacks.IntCallback, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void RemoveFriendsByAuthIdentities(string providerId, List<string> providerUserIds, Action<int> onSuccess, Action<GetSocialError> onFailure)
        {
            _gs_removeFriendsByAuthIdentities(providerId, GSJson.Serialize(providerUserIds),
                Callbacks.IntCallback, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void SetFriends(List<string> userIds, Action onSuccess, Action<GetSocialError> onFailure)
        {
            _gs_setFriends(GSJson.Serialize(userIds),
                Callbacks.ActionCallback, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void SetFriendsByAuthIdentities(string providerId, List<string> providerUserIds, Action onSuccess, Action<GetSocialError> onFailure)
        {
            _gs_setFriendsByAuthIdentities(providerId, GSJson.Serialize(providerUserIds),
                Callbacks.ActionCallback, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void IsFriend (string userId, Action<bool> onSuccess, Action<GetSocialError> onFailure)
        {
           _gs_isFriend (userId,
                Callbacks.BoolCallback, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void GetFriendsCount(Action<int> onSuccess, Action<GetSocialError> onFailure)
        {
           _gs_getFriendsCount(Callbacks.IntCallback, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void GetFriends (int offset, int limit, Action<List<PublicUser>> onSuccess, Action<GetSocialError> onFailure)
        {
           _gs_getFriends (offset, limit,
                Callbacks.GetPublicUsers, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void GetSuggestedFriends(int offset, int limit, Action<List<SuggestedFriend>> onSuccess, Action<GetSocialError> onFailure)
        {
            _gs_getSuggestedFriends (offset, limit,
                Callbacks.GetSuggestedFriends, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void GetFriendsReferences(Action<List<UserReference>> onSuccess, Action<GetSocialError> onFailure)
        {
            _gs_getFriendsReferences(
                Callbacks.GetUserReferences, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        #endregion

        #region activity_feed

        public void GetAnnouncements(string feed, Action<List<ActivityPost>> onSuccess,
            Action<GetSocialError> onFailure)
        {
           _gs_getAnnouncements(feed,
                Callbacks.GetActivityPosts, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void GetActivities(ActivitiesQuery query, Action<List<ActivityPost>> onSuccess,
            Action<GetSocialError> onFailure)
        {
           _gs_getActivitiesWithQuery(query.ToJson(),
                Callbacks.GetActivityPosts, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void GetActivity(string activityId, Action<ActivityPost> onSuccess, Action<GetSocialError> onFailure)
        {
           _gs_getActivityById(activityId,
                Callbacks.GetActivityPost, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void PostActivityToFeed(string feed, ActivityPostContent content, Action<ActivityPost> onSuccess,
            Action<GetSocialError> onFailure)
        {
           _gs_postActivityToFeed(feed, content.ToJson(),
                Callbacks.GetActivityPost, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void PostCommentToActivity(string activityId, ActivityPostContent comment, Action<ActivityPost> onSuccess,
            Action<GetSocialError> onFailure)
        {
           _gs_postCommentToActivity(activityId, comment.ToJson(),
                Callbacks.GetActivityPost, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void LikeActivity(string activityId, bool isLiked, Action<ActivityPost> onSuccess,
            Action<GetSocialError> onFailure)
        {
           _gs_likeActivity(activityId, isLiked,
                Callbacks.GetActivityPost, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void GetActivityLikers(string activityId, int offset, int limit, Action<List<PublicUser>> onSuccess,
            Action<GetSocialError> onFailure)
        {
           _gs_getActivityLikers(activityId, offset, limit,
                Callbacks.GetPublicUsers, onSuccess.GetPointer(),
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void ReportActivity(string activityId, ReportingReason reportingReason, Action onSuccess, Action<GetSocialError> onFailure)
        {
            _gs_reportActivity(activityId, Convert.ToInt32(reportingReason), 
                Callbacks.ActionCallback, onSuccess.GetPointer(), 
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        public void DeleteActivity(string activityId, Action onSuccess, Action<GetSocialError> onFailure)
        {
            _gs_deleteActivity(activityId, 
                Callbacks.ActionCallback, onSuccess.GetPointer(), 
                Callbacks.FailureCallback, onFailure.GetPointer());
        }

        #endregion


        #region Access Helpers

        public void Reset()
        {
           _gs_resetInternal();
        }

        public void HandleOnStartUnityEvent()
        {
            _gs_handleOnStartUnityEvent();
        }
        #endregion


        #region external_init

        [DllImport("__Internal")]
        static extern void _gs_executeWhenInitialized(VoidCallbackDelegate action, IntPtr actionPtr);
        
        [DllImport("__Internal")]
        static extern void _gs_init(string appId);

        [DllImport("__Internal")]
        static extern bool _gs_isInitialized();

        #endregion

        [DllImport("__Internal")]
        static extern string _gs_getNativeSdkVersion();

        [DllImport("__Internal")]
        static extern bool _gs_setLanguage(string languageCode);

        [DllImport("__Internal")]
        static extern string _gs_getLanguage();

        [DllImport("__Internal")]
        static extern bool _gs_setGlobalErrorListener(IntPtr onErrorActionPtr, GlobalErrorCallbackDelegate errorCallback);

        [DllImport("__Internal")]
        static extern bool _gs_removeGlobalErrorListener();

        #region external_invites

        [DllImport("__Internal")]
        static extern bool _gs_isInviteChannelAvailable(string channelId);

        [DllImport("__Internal")]
        static extern string _gs_getInviteChannels();

        [DllImport("__Internal")]
        static extern void _gs_sendInvite(string channelId,
            VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr, IntPtr onCancelActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_sendInviteCustom(string channelId, string customInviteContentJson,
            VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr, IntPtr onCancelActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_sendInviteCustomAndLinkParams(string channelId, string customInviteContentJson,
            string linkParamsJson,
            VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr, IntPtr onCancelActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern bool _gs_registerInviteProviderPlugin(string channelId, IntPtr pluginPtr,
            InviteChannelPluginCallbacks.IsAvailableForDeviceDelegate isAvailableForDeviceDelegate,
            InviteChannelPluginCallbacks.PresentChannelInterfaceDelegate presentChannelInterfaceDelegate);

        [DllImport("__Internal")]
        static extern void _gs_getReferralData(
            FetchReferralDataCallbackDelegate fetchReferralDataCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_getReferredUsers(
            StringCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_createInviteLink(string linkParamsJson, StringCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        // Invite Callbacks
        [DllImport("__Internal")]
        public static extern void _gs_executeInviteSuccessCallback(IntPtr inviteSuccessCallbackPtr);

        [DllImport("__Internal")]
        public static extern void _gs_executeInviteCancelledCallback(IntPtr inviteCancelledCallbackPtr);

        [DllImport("__Internal")]
        public static extern void _gs_executeInviteFailedCallback(IntPtr inviteFailedCallbackPtr, int errorCode, string errorMessage);

        #endregion

        #region push_notifications

        [DllImport("__Internal")]
        static extern bool _gs_registerForPushNotifications();

        [DllImport("__Internal")]
        static extern bool _gs_setNotificationActionListener(IntPtr listenerPointer,
            NotificationListenerDelegate listener);

        [DllImport("__Internal")]
        static extern void _gs_getNotifications(string query,
            StringCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);
        
        [DllImport("__Internal")]
        static extern void _gs_getNotificationsCount(string query, 
            IntCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);
        
        [DllImport("__Internal")]
        static extern void _gs_setNotificationsRead(string ids, bool read, 
            VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);
        
        [DllImport("__Internal")]
        static extern void _gs_setPushNotificationsEnabled(bool read, 
            VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);
        
        [DllImport("__Internal")]
        static extern void _gs_isPushNotificationsEnabled( 
            BoolCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);
        
        #endregion

        #region external_user_management

        [DllImport("__Internal")]
        static extern bool _gs_setOnUserChangedListener(IntPtr listenerPointer, VoidCallbackDelegate onUserChanged);

        [DllImport("__Internal")]
        static extern bool _gs_removeOnUserChangedListener();

        [DllImport("__Internal")]
        static extern string _gs_getUserId();

        [DllImport("__Internal")]
        static extern bool _gs_isUserAnonymous();

        [DllImport("__Internal")]
        static extern void _gs_resetUser(VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr, FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);
        
        [DllImport("__Internal")]
        static extern string _gs_getAuthIdentities();

        [DllImport("__Internal")]
        static extern void _gs_setUserDisplayName(string displayName, VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern string _gs_getUserDisplayName();

        [DllImport("__Internal")]
        static extern void _gs_setUserAvatarUrl(string avatarUrl, VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);
        
        [DllImport("__Internal")]
        static extern void _gs_setUserAvatar(string avatarBase64, VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern string _gs_getUserAvatarUrl();

        [DllImport("__Internal")]
        static extern void _gs_setPublicProperty(string key, string value, VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_setPrivateProperty(string key, string value, VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_removePublicProperty(string key, VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_removePrivateProperty(string key, VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern string _gs_getPublicProperty(string key);

        [DllImport("__Internal")]
        static extern string _gs_getPrivateProperty(string key);

        [DllImport("__Internal")]
        static extern bool _gs_hasPublicProperty(string key);

        [DllImport("__Internal")]
        static extern bool _gs_hasPrivateProperty(string key);
        
        [DllImport("__Internal")]
        static extern string _gs_getAllPublicProperties();
        
        [DllImport("__Internal")]
        static extern string _gs_getAllPrivateProperties();

        [DllImport("__Internal")]
        static extern void _gs_addAuthIdentity(string identity,
            VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr,
            OnUserConflictDelegate conflictCallBack, IntPtr onConflictActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_switchUser(string identity,
            VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_removeAuthIdentity(string providerId,
            VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_getUserById(string userId,
            StringCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_getUserByAuthIdentity(string providerId, string providerUserId,
            StringCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_getUsersByAuthIdentities(string providerId, string providerUserIdsJson,
            StringCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_findUsers(string query,
            StringCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);
        
        #endregion

        #region social_graph

        [DllImport("__Internal")]
        static extern void _gs_addFriend(string userId,
            IntCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_addFriendsByAuthIdentities(string providerId, string providerUserIdsJson,
            IntCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_removeFriend(string userId,
            IntCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_removeFriendsByAuthIdentities(string providerId, string providerUserIdsJson,
            IntCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_setFriends(string userIdsJson,
            VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_setFriendsByAuthIdentities(string providerId, string providerUserIdsJson,
            VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_isFriend(string userId,
            BoolCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_getFriendsCount(IntCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_getFriends(int offset, int limit,
            StringCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);
        
        [DllImport("__Internal")]
        static extern void _gs_getSuggestedFriends(int offset, int limit,
            StringCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);
        
        [DllImport("__Internal")]
        static extern void _gs_getFriendsReferences(
            StringCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        #endregion

        #region activity_feed_internal

        [DllImport("__Internal")]
        static extern void _gs_getAnnouncements(string feed,
            StringCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_getActivitiesWithQuery(string query,
            StringCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_getActivityById(string id,
            StringCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_postActivityToFeed(string feed, string activity,
            StringCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_postCommentToActivity(string id, string comment,
            StringCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_likeActivity(string id, bool isLiked,
            StringCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_getActivityLikers(string id, int offset, int limit,
            StringCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_reportActivity(string id, int reportingReason, 
            VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        [DllImport("__Internal")]
        static extern void _gs_deleteActivity(string id, 
            VoidCallbackDelegate successCallback, IntPtr onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, IntPtr onFailureActionPtr);

        #endregion

        #region external_access_helpers

        [DllImport("__Internal")]
        static extern void _gs_handleOnStartUnityEvent();
        
        [DllImport("__Internal")]
        static extern void _gs_resetInternal();

        #endregion
    }
}

#endif
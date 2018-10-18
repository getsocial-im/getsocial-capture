#include "GetSocialBridgeUtils.h"
#include "UnityInvitePlugin.h"
#include "GetSocialJsonUtils.h"
#include "GetSocialFunctionDefs.h"
#import "NSObject+Json.h"
#import "NSDictionary+GetSocial.h"
#import "NSMutableDictionary+GetSocial.h"
#include <GetSocial/GetSocial.h>


#pragma clang diagnostic push
#pragma ide diagnostic ignored "OCUnusedGlobalDeclarationInspection"
extern "C" {
NS_ASSUME_NONNULL_BEGIN

#pragma mark - Initialization
        
    typedef void(^CompleteBlock)();
        
    typedef void(^ErrorBlock)(NSError *error);
        
    typedef void(^ObjectBlock)(id object);

    typedef void(^IntBlock)(int object);

    typedef void(^BoolBlock)(BOOL object);
        
    CompleteBlock completeBlock(VoidCallbackDelegate delegate, void *ptr)
    {
        CompleteBlock block = ^() {
            delegate(ptr);
        };
        return block;
    }

        
    ErrorBlock errorBlock(FailureCallbackDelegate delegate, void *ptr)
    {
        ErrorBlock block = ^(NSError *error) {
            delegate(ptr, [error toJsonCString]);
        };
        return block;
    }
        
    ObjectBlock objectBlock(StringCallbackDelegate delegate, void *ptr)
    {
        ObjectBlock block = ^(id object) {
            delegate(ptr, [object toJsonCString]);
        };
        return block;
    }

    IntBlock intBlock(IntCallbackDelegate delegate, void *ptr)
    {
        IntBlock block = ^(int object) {
            delegate(ptr, object);
        };
        return block;
    }
        
    BoolBlock boolBlock(BoolCallbackDelegate delegate, void *ptr)
    {
        BoolBlock block = ^(BOOL object) {
            delegate(ptr, object);
        };
        return block;
    }
        
    void _gs_executeWhenInitialized(VoidCallbackDelegate action, void *actionPtr)
    {
        [GetSocial executeWhenInitialized:completeBlock(action, actionPtr)];
    }
        
    void _gs_init(const char *appId)
    {
        NSString *appIdStr = [GetSocialBridgeUtils createNSStringFrom:appId];
        [GetSocial initWithAppId:appIdStr];
    }

    void _gs_start_init()
    {
        [GetSocial init];
    }

    bool _gs_isInitialized()
    {
        return [GetSocial isInitialized];
    }

    void _gs_getReferralData(StringCallbackDelegate fetchReferralDataCallback, void *onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, void *onFailureActionPtr)
    {
        [GetSocial referralDataWithSuccess:objectBlock(fetchReferralDataCallback, onSuccessActionPtr)
                                failure:errorBlock(failureCallback, onFailureActionPtr)];
    }

    char *_gs_getNativeSdkVersion()
    {
        return [GetSocialBridgeUtils createCStringFrom:[GetSocial sdkVersion]];
    }

    #pragma mark - Localization
    char *_gs_getLanguage()
    {
        return [GetSocialBridgeUtils createCStringFrom:[GetSocial language]];
    }

    bool _gs_setLanguage(const char *languageCode)
    {
        NSString *languageCodeStr = [GetSocialBridgeUtils createNSStringFrom:languageCode];
        return [GetSocial setLanguage:languageCodeStr];
    }

    #pragma mark - Global error handler
    bool _gs_setGlobalErrorListener(void *onErrorActionPtr, FailureCallbackDelegate errorCallback)
    {
        return [GetSocial setGlobalErrorHandler:errorBlock(errorCallback, onErrorActionPtr)];
    }

    bool _gs_removeGlobalErrorListener()
    {
        return [GetSocial removeGlobalErrorHandler];
    }

    #pragma mark - Invites
    bool _gs_isInviteChannelAvailable(const char *channelId)
    {
        NSString *channelIdStr = [GetSocialBridgeUtils createNSStringFrom:channelId];
        return [GetSocial isInviteChannelAvailable:channelIdStr];
    }
 
    char *_gs_getInviteChannels()
    {
        return [[GetSocial inviteChannels] toJsonCString];
    }

    void _gs_sendInvite(const char *channelId,
            VoidCallbackDelegate completeCallback, void *onCompletePtr, void *onCancelPtr,
            FailureCallbackDelegate failureCallback, void *onFailurePtr)
    {
        NSString *channelIdStr = [GetSocialBridgeUtils createNSStringFrom:channelId];

        [GetSocial sendInviteWithChannelId:channelIdStr
                                success:completeBlock(completeCallback, onCompletePtr)
                                    cancel:completeBlock(completeCallback, onCancelPtr)
                                failure:errorBlock(failureCallback, onFailurePtr)];
    }

    void _gs_sendInviteCustom(const char *channelId, const char *customInviteContentJson,
                        VoidCallbackDelegate completeCallback, void *onCompletePtr, void *onCancelPtr,
                        FailureCallbackDelegate failureCallback, void *onFailurePtr)
    {
        NSString *channelIdStr = [GetSocialBridgeUtils createNSStringFrom:channelId];
        NSString *customInviteContentJsonStr = [GetSocialBridgeUtils createNSStringFrom:customInviteContentJson];
        
        GetSocialMutableInviteContent *inviteContent = [GetSocialJsonUtils deserializeCustomInviteContent:customInviteContentJsonStr];
        
        [GetSocial sendInviteWithChannelId:channelIdStr
                            inviteContent:inviteContent
                                success:completeBlock(completeCallback, onCompletePtr)
                                    cancel:completeBlock(completeCallback, onCancelPtr)
                                failure:errorBlock(failureCallback, onFailurePtr)];
    }

        
    void _gs_sendInviteCustomAndLinkParams(const char *channelId, const char *customInviteContentJson, const char *linkParamsJson,
            VoidCallbackDelegate completeCallback, void *onCompletePtr, void *onCancelPtr,
            FailureCallbackDelegate failureCallback, void *onFailurePtr)
    {
        NSString *channelIdStr = [GetSocialBridgeUtils createNSStringFrom:channelId];
        NSString *customInviteContentJsonStr = [GetSocialBridgeUtils createNSStringFrom:customInviteContentJson];
        NSString *linkParamsJsonStr = [GetSocialBridgeUtils createNSStringFrom:linkParamsJson];

        GetSocialMutableInviteContent *inviteContent = [GetSocialJsonUtils deserializeCustomInviteContent:customInviteContentJsonStr];
        NSDictionary *linkParams = [GetSocialJsonUtils deserializeLinkParams:linkParamsJsonStr];

        [GetSocial sendInviteWithChannelId:channelIdStr
                            inviteContent:inviteContent
                        linkParams:linkParams
                                success:completeBlock(completeCallback, onCompletePtr)
                                    cancel:completeBlock(completeCallback, onCancelPtr)
                                failure:errorBlock(failureCallback, onFailurePtr)];
    }

    bool _gs_registerInviteProviderPlugin(const char *channelId, void *pluginPtr,
            IsAvailableForDeviceDelegate isAvailableForDevice, PresentChannelInterfaceDelegate presentProviderInterface)
    {
        NSString *channelIdStr = [GetSocialBridgeUtils createNSStringFrom:channelId];

        UnityInvitePlugin *invitePlugin = nil;
        if (pluginPtr != 0) {
            invitePlugin = [[UnityInvitePlugin alloc] initWithPluginPtr:pluginPtr inviteFriendsDelegate:presentProviderInterface isAvailableForDeviceDelegate:isAvailableForDevice];
        }

        return [GetSocial registerInviteChannelPlugin:invitePlugin forChannelId:channelIdStr];
    }

    void _gs_getReferredUsers(StringCallbackDelegate getReferredUsersCallback, void *onSuccessActionPtr,
                                FailureCallbackDelegate failureCallback, void *onFailureActionPtr)
    {
        [GetSocial referredUsersWithSuccess:objectBlock(getReferredUsersCallback, onSuccessActionPtr) failure: errorBlock(failureCallback, onFailureActionPtr)];
    }
    
    void _gs_createInviteLink(const char *linkParamsJson,
                              StringCallbackDelegate completeCallback, void *onCompletePtr,
                              FailureCallbackDelegate failureCallback, void *onFailurePtr) {
        NSString *linkParamsJsonStr = [GetSocialBridgeUtils createNSStringFrom:linkParamsJson];
        NSDictionary *linkParams = [GetSocialJsonUtils deserializeLinkParams:linkParamsJsonStr];
        
        [GetSocial createInviteLinkWithParams:linkParams success:^(NSString * _Nonnull result) {
            completeCallback(onCompletePtr, [GetSocialBridgeUtils createCStringFrom:result]);
        } failure: errorBlock(failureCallback, onFailurePtr)];
    }

    #pragma mark - Invite Callbacks
    // Invite Callbacks
    void _gs_executeInviteSuccessCallback(void *callback)
    {
        // transfer pointer ownership to arc
        // more at: http://stackoverflow.com/questions/7036350/arc-and-bridged-cast
        GetSocialInviteSuccessCallback inviteCompletedCallback = (__bridge GetSocialInviteSuccessCallback) callback;

        inviteCompletedCallback();
    }

    void _gs_executeInviteCancelledCallback(void *callback)
    {
        // transfer pointer ownership to arc
        // more at: http://stackoverflow.com/questions/7036350/arc-and-bridged-cast
        GetSocialInviteCancelCallback inviteCancelledCallback = (__bridge GetSocialInviteCancelCallback) callback;

        inviteCancelledCallback();
    }

    void _gs_executeInviteFailedCallback(void *callback, int errorCode, const char *errorMessage)
    {
        // transfer pointer ownership to arc
        // more at: http://stackoverflow.com/questions/7036350/arc-and-bridged-cast
        GetSocialFailureCallback inviteFailedCallback = (__bridge GetSocialFailureCallback) callback;

        NSError *error = [NSError errorWithDomain:@"GetSocial" code:errorCode userInfo:@{
                                                                                NSLocalizedDescriptionKey: [GetSocialBridgeUtils createNSStringFrom:errorMessage]
                                                                        }];
        inviteFailedCallback(error);
    }
        
    #pragma mark - Push Notifications
        
    void _gs_registerForPushNotifications()
    {
        [GetSocial registerForPushNotifications];
    }
        
    void _gs_setNotificationActionListener(void *listener, NotificationListener delegate)
    {
        [GetSocial setNotificationHandler:^BOOL(GetSocialNotification * _Nonnull notification, BOOL wasClicked) {
            NSDictionary *data = @{
                                @"notification": notification.toJsonString,
                                @"wasClicked": @(wasClicked)
                                };
            return delegate(listener, data.toJsonCString);
        }];
    }
        
    void _gs_getNotifications(const char* query,
                                    StringCallbackDelegate successCallback, void * onSuccessActionPtr,
                                    FailureCallbackDelegate failureCallback, void * onFailureActionPtr)
    {
        NSString *queryStr = [GetSocialBridgeUtils createNSStringFrom:query];
        GetSocialNotificationsQuery *notificationsQuery = [GetSocialJsonUtils deserializeNotificationsQuery:queryStr];

        [GetSocialUser notificationsWithQuery:notificationsQuery success:objectBlock(successCallback, onSuccessActionPtr) failure:errorBlock(failureCallback, onFailureActionPtr)];
                                                        
    }
        
    void _gs_getNotificationsCount(const char* query,
                                    IntCallbackDelegate successCallback, void * onSuccessActionPtr,
                                    FailureCallbackDelegate failureCallback, void * onFailureActionPtr)
    {
        NSString *queryStr = [GetSocialBridgeUtils createNSStringFrom:query];
        GetSocialNotificationsCountQuery *notificationsQuery = [GetSocialJsonUtils deserializeNotificationsCountQuery:queryStr];
        
        [GetSocialUser notificationsCountWithQuery:notificationsQuery success:intBlock(successCallback, onSuccessActionPtr) failure:errorBlock(failureCallback, onFailureActionPtr)];
    }

    void _gs_setNotificationsRead(const char* ids, bool read,
                                    VoidCallbackDelegate successCallback, void * onSuccessActionPtr,
                                    FailureCallbackDelegate failureCallback, void * onFailureActionPtr)
    {
        NSString *idsStr = [GetSocialBridgeUtils createNSStringFrom:ids];
        NSArray *notificationsIds = [GetSocialJsonUtils deserializeList:idsStr];
        
        [GetSocialUser setNotificationsRead:notificationsIds read:read success:completeBlock(successCallback, onSuccessActionPtr) failure:errorBlock(failureCallback, onFailureActionPtr)];
    }
        
    void _gs_setPushNotificationsEnabled(bool isEnabled,
                                        VoidCallbackDelegate successCallback, void * onSuccessActionPtr,
                                        FailureCallbackDelegate failureCallback, void * onFailureActionPtr)
    {
        [GetSocialUser setPushNotificationsEnabled:isEnabled success:completeBlock(successCallback, onSuccessActionPtr) failure:errorBlock(failureCallback, onFailureActionPtr)];
    }
        
    void _gs_isPushNotificationsEnabled(BoolCallbackDelegate successCallback, void * onSuccessActionPtr,
                                        FailureCallbackDelegate failureCallback, void * onFailureActionPtr)
    {
        [GetSocialUser isPushNotificationsEnabledWithSuccess:boolBlock(successCallback, onSuccessActionPtr) failure:errorBlock(failureCallback, onFailureActionPtr)];
    }
    #pragma mark - User Management

    bool _gs_setOnUserChangedListener(void *listener, VoidCallbackDelegate delegate)
    {
        return [GetSocialUser setOnUserChangedHandler:completeBlock(delegate, listener)];
    }

    bool _gs_removeOnUserChangedListener()
    {
        return [GetSocialUser removeOnUserChangedHandler];
    }

    char *_gs_getUserId()
    {
        return [GetSocialBridgeUtils createCStringFrom:[GetSocialUser userId]];
    }

    char *_gs_getUserDisplayName()
    {
        return [GetSocialBridgeUtils createCStringFrom:[GetSocialUser displayName]];
    }

    void _gs_setUserDisplayName(const char *displayName, VoidCallbackDelegate completeCallback, void *onCompletePtr,
            FailureCallbackDelegate failureCallback, void *onFailurePtr)
    {
        [GetSocialUser setDisplayName:[GetSocialBridgeUtils createNSStringFrom:displayName]
                            success:completeBlock(completeCallback, onCompletePtr)
                            failure:errorBlock(failureCallback, onFailurePtr)];
    }
        
    void _gs_setPublicProperty(const char * key, const char * value, VoidCallbackDelegate successCallback, void * onSuccessActionPtr, FailureCallbackDelegate failureCallback, void * onFailureActionPtr)
    {
        [GetSocialUser setPublicPropertyValue:[GetSocialBridgeUtils createNSStringFrom:value]
                                    forKey:[GetSocialBridgeUtils createNSStringFrom:key]
                                    success:completeBlock(successCallback, onSuccessActionPtr)
                                    failure:errorBlock(failureCallback, onFailureActionPtr)];
    }
        

    void _gs_setPrivateProperty(const char * key, const char * value, VoidCallbackDelegate successCallback, void * onSuccessActionPtr, FailureCallbackDelegate failureCallback, void * onFailureActionPtr)
    {
        [GetSocialUser setPrivatePropertyValue:[GetSocialBridgeUtils createNSStringFrom:value]
                                        forKey:[GetSocialBridgeUtils createNSStringFrom:key]
                                    success:completeBlock(successCallback, onSuccessActionPtr)
                                    failure:errorBlock(failureCallback, onFailureActionPtr)];
    }
        
    void _gs_removePublicProperty(const char * key, VoidCallbackDelegate successCallback, void * onSuccessActionPtr, FailureCallbackDelegate failureCallback, void * onFailureActionPtr)
    {
        [GetSocialUser removePublicPropertyForKey:[GetSocialBridgeUtils createNSStringFrom:key]
                                        success:completeBlock(successCallback, onSuccessActionPtr)
                                        failure:errorBlock(failureCallback, onFailureActionPtr)];
    }


    void _gs_removePrivateProperty(const char * key, VoidCallbackDelegate successCallback, void * onSuccessActionPtr, FailureCallbackDelegate failureCallback, void * onFailureActionPtr)
    {
        [GetSocialUser removePrivatePropertyForKey:[GetSocialBridgeUtils createNSStringFrom:key]
                                        success:completeBlock(successCallback, onSuccessActionPtr)
                                        failure:errorBlock(failureCallback, onFailureActionPtr)];
    }

    char * _gs_getPublicProperty(const char * key)
    {
        return [GetSocialBridgeUtils createCStringFrom:[GetSocialUser publicPropertyValueForKey:[GetSocialBridgeUtils createNSStringFrom:key]]];
    }
    char * _gs_getPrivateProperty(const char * key)
    {
        return [GetSocialBridgeUtils createCStringFrom:[GetSocialUser privatePropertyValueForKey:[GetSocialBridgeUtils createNSStringFrom:key]]];
    }

    BOOL _gs_hasPublicProperty(const char * key)
    {
        return [GetSocialUser hasPublicPropertyForKey:[GetSocialBridgeUtils createNSStringFrom:key]];
    }

    BOOL _gs_hasPrivateProperty(const char * key)
    {
        return [GetSocialUser hasPrivatePropertyForKey:[GetSocialBridgeUtils createNSStringFrom:key]];
    }

    char *_gs_getUserAvatarUrl()
    {
        return [GetSocialBridgeUtils createCStringFrom:[GetSocialUser avatarUrl]];
    }

    void _gs_setUserAvatarUrl(const char *avatarUrl, VoidCallbackDelegate completeCallback, void *onCompletePtr,
            FailureCallbackDelegate failureCallback, void *onFailurePtr)
    {
        [GetSocialUser setAvatarUrl:[GetSocialBridgeUtils createNSStringFrom:avatarUrl]
                            success:completeBlock(completeCallback, onCompletePtr)
                            failure:errorBlock(failureCallback, onFailurePtr)];
    }
        
    void _gs_setUserAvatar(const char *avatarBase64, VoidCallbackDelegate completeCallback, void *onCompletePtr,
                            FailureCallbackDelegate failureCallback, void *onFailurePtr)
    {
        NSString *b64String = [GetSocialBridgeUtils createNSStringFrom:avatarBase64];
        [GetSocialUser setAvatar:[GetSocialBridgeUtils decodeUIImageFrom:b64String]
                        success:completeBlock(completeCallback, onCompletePtr)
                        failure:errorBlock(failureCallback, onFailurePtr)];
    }
    
    bool _gs_isUserAnonymous()
    {
        return [GetSocialUser isAnonymous];
    }
        
    void _gs_resetUser(VoidCallbackDelegate completeCallback, void *onCompletePtr,
                    FailureCallbackDelegate failureCallback, void *onFailurePtr)
    {
        [GetSocialUser resetWithSuccess:completeBlock(completeCallback, onCompletePtr)
                                failure:errorBlock(failureCallback, onFailurePtr)];
    }

    char *_gs_getAuthIdentities()
    {
        return [[GetSocialUser authIdentities] toJsonCString];
    }

    char *_gs_getAllPublicProperties()
    {
        return [[GetSocialUser allPublicProperties] toJsonCString];
    }

    char *_gs_getAllPrivateProperties()
    {
        return [[GetSocialUser allPrivateProperties] toJsonCString];
    }

    void _gs_addAuthIdentity(const char *identity,
            VoidCallbackDelegate successCallback, void *onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, void *onFailureActionPtr,
            StringCallbackDelegate conflictCallBack, void *onConflictActionPtr)
    {
        NSString *identityStr = [GetSocialBridgeUtils createNSStringFrom:identity];
        GetSocialAuthIdentity *gsIdentity = [GetSocialJsonUtils deserializeIdentity:identityStr];
        [GetSocialUser addAuthIdentity:gsIdentity
                            success:completeBlock(successCallback, onSuccessActionPtr)
                            conflict:objectBlock(conflictCallBack, onConflictActionPtr)
                            failure:errorBlock(failureCallback, onFailureActionPtr)];
    }

    void _gs_switchUser(const char *identity,
            VoidCallbackDelegate successCallback, void *onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, void *onFailureActionPtr)
    {
        NSString *identityStr = [GetSocialBridgeUtils createNSStringFrom:identity];
        GetSocialAuthIdentity *gsIdentity = [GetSocialJsonUtils deserializeIdentity:identityStr];
        [GetSocialUser switchUserToIdentity:gsIdentity
                                    success:completeBlock(successCallback, onSuccessActionPtr)
                                    failure:errorBlock(failureCallback, onFailureActionPtr)];
    }

    void _gs_removeAuthIdentity(const char *providerId,
            VoidCallbackDelegate successCallback, void *onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, void *onFailureActionPtr)
    {
        NSString *providerIdStr = [GetSocialBridgeUtils createNSStringFrom:providerId];

        [GetSocialUser removeAuthIdentityWithProviderId:providerIdStr
                                                success:completeBlock(successCallback, onSuccessActionPtr)
                                                failure:errorBlock(failureCallback, onFailureActionPtr)];
    }
        
    void _gs_getUserById(const char * userId,
                        StringCallbackDelegate successCallback, void *onSuccessActionPtr,
                        FailureCallbackDelegate failureCallback, void *onFailureActionPtr) {

        NSString *userIdStr = [GetSocialBridgeUtils createNSStringFrom:userId];

        [GetSocial userWithId:userIdStr
                    success:objectBlock(successCallback, onSuccessActionPtr)
                    failure:errorBlock(failureCallback, onFailureActionPtr)];
    }
        
    void _gs_getUserByAuthIdentity(const char * providerId, const char * providerUserId,
                                    StringCallbackDelegate successCallback, void *onSuccessActionPtr,
                                    FailureCallbackDelegate failureCallback, void *onFailureActionPtr) {
            
        NSString *providerIdStr = [GetSocialBridgeUtils createNSStringFrom:providerId];
        NSString *providerUserIdStr = [GetSocialBridgeUtils createNSStringFrom:providerUserId];
        
        [GetSocial userWithId:providerUserIdStr forProvider:providerIdStr success:objectBlock(successCallback, onSuccessActionPtr) failure:errorBlock(failureCallback, onFailureActionPtr)];
    }
        
    void _gs_getUsersByAuthIdentities(const char * providerId, const char * providerUserIdsJson,
                                    StringCallbackDelegate successCallback, void *onSuccessActionPtr,
                                    FailureCallbackDelegate failureCallback, void *onFailureActionPtr) {
            
        NSString *providerIdStr = [GetSocialBridgeUtils createNSStringFrom:providerId];
        NSString *providerUserIdsJsonStr = [GetSocialBridgeUtils createNSStringFrom:providerUserIdsJson];
        NSArray *providerUserIds = [GetSocialJsonUtils deserializeList:providerUserIdsJsonStr];
        
        [GetSocial usersWithIds:providerUserIds forProvider:providerIdStr success:objectBlock(successCallback, onSuccessActionPtr) failure:errorBlock(failureCallback, onFailureActionPtr)];
            
    }

    void _gs_findUsers(const char * query,
                    StringCallbackDelegate successCallback, void *onSuccessActionPtr,
                    FailureCallbackDelegate failureCallback, void *onFailureActionPtr)
    {
        NSString *queryStr = [GetSocialBridgeUtils createNSStringFrom:query];
        GetSocialUsersQuery *usersQuery = [GetSocialJsonUtils deserializeUsersQuery:queryStr];
        
        [GetSocial findUsers:usersQuery
                    success:objectBlock(successCallback, onSuccessActionPtr)
                    failure:errorBlock(failureCallback, onFailureActionPtr)];
    }
        
    #pragma mark - SocialGraph

    void _gs_addFriend(const char *userId,
            IntCallbackDelegate successCallback, void *onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, void *onFailureActionPtr)
    {
        NSString *userIdStr = [GetSocialBridgeUtils createNSStringFrom:userId];

        [GetSocialUser addFriend:userIdStr
                        success:intBlock(successCallback, onSuccessActionPtr)
                        failure:errorBlock(failureCallback, onFailureActionPtr)];
    }

    void _gs_addFriendsByAuthIdentities(const char *providerId, const char* providerUserIdsJson,
                                            IntCallbackDelegate successCallback, void *onSuccessActionPtr,
                                            FailureCallbackDelegate failureCallback, void *onFailureActionPtr)
    {
        NSString *providerIdStr = [GetSocialBridgeUtils createNSStringFrom:providerId];
        NSString *providerUserIdsJsonStr = [GetSocialBridgeUtils createNSStringFrom:providerUserIdsJson];
        NSArray *providerUserIds = [GetSocialJsonUtils deserializeList:providerUserIdsJsonStr];
        
        [GetSocialUser addFriendsWithIds:providerUserIds forProvider:providerIdStr success:intBlock(successCallback, onSuccessActionPtr) failure:errorBlock(failureCallback, onFailureActionPtr)];
    }

    void _gs_removeFriend(const char *userId,
            IntCallbackDelegate successCallback, void *onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, void *onFailureActionPtr)
    {
        NSString *userIdStr = [GetSocialBridgeUtils createNSStringFrom:userId];

        [GetSocialUser removeFriend:userIdStr
                            success:intBlock(successCallback, onSuccessActionPtr)
                            failure:errorBlock(failureCallback, onFailureActionPtr)];
    }
        
    void _gs_removeFriendsByAuthIdentities(const char *providerId, const char* providerUserIdsJson,
                                        IntCallbackDelegate successCallback, void *onSuccessActionPtr,
                                        FailureCallbackDelegate failureCallback, void *onFailureActionPtr)
    {
        NSString *providerIdStr = [GetSocialBridgeUtils createNSStringFrom:providerId];
        NSString *providerUserIdsJsonStr = [GetSocialBridgeUtils createNSStringFrom:providerUserIdsJson];
        NSArray *providerUserIds = [GetSocialJsonUtils deserializeList:providerUserIdsJsonStr];
            
        [GetSocialUser removeFriendsWithIds:providerUserIds forProvider:providerIdStr success:intBlock(successCallback, onSuccessActionPtr) failure:errorBlock(failureCallback, onFailureActionPtr)];
    }

    void _gs_setFriends(const char *userIdsJson,
                        VoidCallbackDelegate successCallback, void *onSuccessActionPtr,
                            FailureCallbackDelegate failureCallback, void *onFailureActionPtr)
    {
        NSString *userIdsJsonStr = [GetSocialBridgeUtils createNSStringFrom:userIdsJson];
        NSArray *userIds = [GetSocialJsonUtils deserializeList:userIdsJsonStr];
        
        [GetSocialUser setFriendsWithIds:userIds success:completeBlock(successCallback, onSuccessActionPtr) failure:errorBlock(failureCallback, onFailureActionPtr)];
    }

    void _gs_setFriendsByAuthIdentities(const char * providerId, const char *providerUserIdsJson,
                            VoidCallbackDelegate successCallback, void *onSuccessActionPtr,
                            FailureCallbackDelegate failureCallback, void *onFailureActionPtr)
    {
        NSString *providerIdStr = [GetSocialBridgeUtils createNSStringFrom:providerId];
        NSString *providerUserIdsJsonStr = [GetSocialBridgeUtils createNSStringFrom:providerUserIdsJson];
        NSArray *providerUserIds = [GetSocialJsonUtils deserializeList:providerUserIdsJsonStr];

        [GetSocialUser setFriendsWithIds:providerUserIds forProvider:providerIdStr success:completeBlock(successCallback, onSuccessActionPtr) failure:errorBlock(failureCallback, onFailureActionPtr)];
    }

    void _gs_isFriend(const char *userId,
            BoolCallbackDelegate successCallback, void *onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, void *onFailureActionPtr)
    {
        NSString *userIdStr = [GetSocialBridgeUtils createNSStringFrom:userId];

        [GetSocialUser isFriend:userIdStr
                        success:boolBlock(successCallback, onSuccessActionPtr)
                        failure:errorBlock(failureCallback, onFailureActionPtr)];
    }

    void _gs_getFriendsCount(IntCallbackDelegate successCallback, void *onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, void *onFailureActionPtr)
    {
        [GetSocialUser friendsCountWithSuccess:intBlock(successCallback, onSuccessActionPtr)
                                    failure:errorBlock(failureCallback, onFailureActionPtr)];
    }
        
    void _gs_getFriends(int offset, int limit,
                        StringCallbackDelegate successCallback, void *onSuccessActionPtr,
                        FailureCallbackDelegate failureCallback, void *onFailureActionPtr)
    {
        [GetSocialUser friendsWithOffset:offset
                                limit:limit
                                success:objectBlock(successCallback, onSuccessActionPtr)
                                failure:errorBlock(failureCallback, onFailureActionPtr)];
    }


    void _gs_getSuggestedFriends(int offset, int limit,
                        StringCallbackDelegate successCallback, void *onSuccessActionPtr,
                        FailureCallbackDelegate failureCallback, void *onFailureActionPtr)
    {
        [GetSocialUser suggestedFriendsWithOffset:offset
                                            limit:limit
                                        success:objectBlock(successCallback, onSuccessActionPtr)
                                        failure:errorBlock(failureCallback, onFailureActionPtr)];
    }

    void _gs_getFriendsReferences(StringCallbackDelegate successCallback, void *onSuccessActionPtr,
                                FailureCallbackDelegate failureCallback, void *onFailureActionPtr)
    {
        [GetSocialUser friendsReferencesWithSuccess:objectBlock(successCallback, onSuccessActionPtr)
                                            failure:errorBlock(failureCallback, onFailureActionPtr)];
    }
        
    #pragma mark - Activity Feed API

    void _gs_getAnnouncements(const char *feed,
            StringCallbackDelegate successCallback, void *onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, void *onFailureActionPtr)
    {
        NSString *feedId = [GetSocialBridgeUtils createNSStringFrom:feed];

        [GetSocial announcementsForFeed:feedId
                                success:objectBlock(successCallback, onSuccessActionPtr)
                                failure:errorBlock(failureCallback, onFailureActionPtr)];
    }

    void _gs_getActivitiesWithQuery(const char *query,
            StringCallbackDelegate successCallback, void *onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, void *onFailureActionPtr)
    {
        NSString *queryStr = [GetSocialBridgeUtils createNSStringFrom:query];
        GetSocialActivitiesQuery *activitiesQuery = [GetSocialJsonUtils deserializeActivitiesQuery:queryStr];

        [GetSocial activitiesWithQuery:activitiesQuery
                            success:objectBlock(successCallback, onSuccessActionPtr)
                            failure:errorBlock(failureCallback, onFailureActionPtr)];
    }

    void _gs_getActivityById(const char *activityId,
            StringCallbackDelegate successCallback, void *onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, void *onFailureActionPtr)
    {
        NSString *activityIdStr = [GetSocialBridgeUtils createNSStringFrom:activityId];
        [GetSocial activityWithId:activityIdStr
                        success:objectBlock(successCallback, onSuccessActionPtr)
                        failure:errorBlock(failureCallback, onFailureActionPtr)];
    }

    void _gs_postActivityToFeed(const char *feedId, const char *activityContent,
            StringCallbackDelegate successCallback, void *onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, void *onFailureActionPtr)
    {
        NSString *feedIdStr = [GetSocialBridgeUtils createNSStringFrom:feedId];
        NSString *activityContentStr = [GetSocialBridgeUtils createNSStringFrom:activityContent];
        GetSocialActivityPostContent *content = [GetSocialJsonUtils deserializeActivityContent:activityContentStr];

        [GetSocial postActivity:content
                        toFeed:feedIdStr
                        success:objectBlock(successCallback, onSuccessActionPtr)
                        failure:errorBlock(failureCallback, onFailureActionPtr)];
    }

    void _gs_postCommentToActivity(const char *activityId, const char *comment,
            StringCallbackDelegate successCallback, void *onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, void *onFailureActionPtr)
    {
        NSString *commentStr = [GetSocialBridgeUtils createNSStringFrom:comment];
        GetSocialActivityPostContent *commentContent = [GetSocialJsonUtils deserializeActivityContent:commentStr];

        NSString *activityIdStr = [GetSocialBridgeUtils createNSStringFrom:activityId];

        [GetSocial postComment:commentContent
            toActivityWithId:activityIdStr
                    success:objectBlock(successCallback, onSuccessActionPtr)
                    failure:errorBlock(failureCallback, onFailureActionPtr)];
    }

    void _gs_likeActivity(const char *activityId, bool isLiked,
            StringCallbackDelegate successCallback, void *onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, void *onFailureActionPtr)
    {
        NSString *activityIdStr = [GetSocialBridgeUtils createNSStringFrom:activityId];
        [GetSocial likeActivityWithId:activityIdStr
                            isLiked:isLiked
                            success:objectBlock(successCallback, onSuccessActionPtr)
                            failure:errorBlock(failureCallback, onFailureActionPtr)];
    }

    void _gs_getActivityLikers(const char *activityId, int offset, int limit,
            StringCallbackDelegate successCallback, void *onSuccessActionPtr,
            FailureCallbackDelegate failureCallback, void *onFailureActionPtr)
    {
        NSString *activityIdStr = [GetSocialBridgeUtils createNSStringFrom:activityId];
        [GetSocial activityLikers:activityIdStr
                        offset:offset
                            limit:limit
                        success:objectBlock(successCallback, onSuccessActionPtr)
                        failure:errorBlock(failureCallback, onFailureActionPtr)];
    }
        
    void _gs_reportActivity(const char *activityId, int reportingReason,
        VoidCallbackDelegate successCallback, void *onSuccessActionPtr,
                            FailureCallbackDelegate failureCallback, void *onFailureActionPtr)
    {
        NSString *activityIdStr = [GetSocialBridgeUtils createNSStringFrom:activityId];
        [GetSocial reportActivity:activityIdStr
                        reason:(GetSocialReportingReason)reportingReason
                        success:completeBlock(successCallback, onSuccessActionPtr)
                        failure:errorBlock(failureCallback, onFailureActionPtr)];
    }
        
    void _gs_deleteActivity(const char *activityId,
                VoidCallbackDelegate successCallback, void *onSuccessActionPtr,
                FailureCallbackDelegate failureCallback, void *onFailureActionPtr)
    {
        NSString *activityIdStr = [GetSocialBridgeUtils createNSStringFrom:activityId];
        [GetSocial deleteActivity:activityIdStr
                        success:completeBlock(successCallback, onSuccessActionPtr)
                        failure:errorBlock(failureCallback, onFailureActionPtr)]; 
    }

#pragma mark - Internal
    void _gs_handleOnStartUnityEvent()
    {
        [GetSocial performSelector:NSSelectorFromString(@"handleOnStartUnityEvent")];
    }

    void _gs_resetInternal()
    {
        [GetSocial performSelector:NSSelectorFromString(@"reset")];
    }
NS_ASSUME_NONNULL_END
}

#pragma clang diagnostic pop

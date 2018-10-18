//
//  UnityInvitePlugin_UnityInvitePlugin.h
//  Unity-iPhone
//
//  Created by Taras Leskiv on 19/12/2016.
//
//

#import "UnityInvitePlugin.h"
#import "GetSocialJsonUtils.h"
#import "GetSocialBridgeUtils.h"
#import "NSObject+Json.h"

@implementation UnityInvitePlugin

- (id)     initWithPluginPtr:(void *)pluginPtr
       inviteFriendsDelegate:(PresentChannelInterfaceDelegate *)inviteFriends
isAvailableForDeviceDelegate:(IsAvailableForDeviceDelegate *)isAvailableForDevice
{
    self = [super init];

    if (self != nil)
    {
        _pluginPtr = pluginPtr;
        _presentProviderInterfaceDelegate = inviteFriends;
        _isAvailableForDeviceDelegate = isAvailableForDevice;
    }

    return self;
}

- (BOOL)isAvailableForDevice:(GetSocialInviteChannel *)inviteChannel
{
    NSString *serializedInviteProvider = [inviteChannel toJsonString];

    return inviteChannel.enabled && _isAvailableForDeviceDelegate(_pluginPtr, [GetSocialBridgeUtils createCStringFrom:serializedInviteProvider]);
}

- (void)presentPluginWithInviteChannel:(GetSocialInviteChannel *)inviteChannel
                         invitePackage:(GetSocialInvitePackage *)invitePackage
                      onViewController:(UIViewController *)viewController
                               success:(GetSocialInviteSuccessCallback)successCallback
                                cancel:(GetSocialInviteCancelCallback)cancelCallback
                               failure:(GetSocialFailureCallback)failureCallback
{
    NSString *serializedInviteProvider = [inviteChannel toJsonString];
    NSString *serializedInvitePackage = [invitePackage toJsonString];

    // transfer pointer ownership to us
    // more at: http://stackoverflow.com/questions/7036350/arc-and-bridged-cast
    void *onSuccessPtr = (__bridge_retained void *) [successCallback copy];
    void *onCancelPtr = (__bridge_retained void *) [cancelCallback copy];
    void *onFailurePtr = (__bridge_retained void *) [failureCallback copy];

    _presentProviderInterfaceDelegate(_pluginPtr, serializedInviteProvider.UTF8String, serializedInvitePackage.UTF8String, onSuccessPtr, onCancelPtr, onFailurePtr);
}
@end

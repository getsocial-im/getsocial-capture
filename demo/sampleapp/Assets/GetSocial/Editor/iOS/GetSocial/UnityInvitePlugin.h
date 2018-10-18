//
//  UnityInvitePlugin.m
//  Unity-iPhone
//
//  Created by Taras Leskiv on 19/12/2016.
//
//

#import <Foundation/Foundation.h>
#import <GetSocial/GetSocial.h>

typedef void(PresentChannelInterfaceDelegate)(void *pluginPtr, const char *inviteChannelJson, const char *invitePackageJson, void *onCompletePtr, void *onCancelPtr, void *onFailurePtr);

typedef bool(IsAvailableForDeviceDelegate)(void *pluginPtr, const char *inviteChannelJson);

@interface UnityInvitePlugin : GetSocialInviteChannelPlugin
{
    void *_pluginPtr;
    PresentChannelInterfaceDelegate *_presentProviderInterfaceDelegate;
    IsAvailableForDeviceDelegate *_isAvailableForDeviceDelegate;
}

- (id)     initWithPluginPtr:(void *)pluginPtr
       inviteFriendsDelegate:(PresentChannelInterfaceDelegate *)inviteFriendsDelegate
isAvailableForDeviceDelegate:(IsAvailableForDeviceDelegate *)isAvailableForDeviceDelegate;
@end

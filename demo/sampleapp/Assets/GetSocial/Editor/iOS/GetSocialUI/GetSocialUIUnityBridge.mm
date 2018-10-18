//
// Created by Taras Leskiv on 05/01/2017.
//

#include <GetSocialUI/GetSocialUI.h>
#include <GetSocialUI/GetSocialUIPublicConstants.h>
#include "GetSocialBridgeUtils.h"
#include "GetSocialJsonUtils.h"
#import "GetSocialFunctionDefs.h"
#import "NSObject+Json.h"

typedef void(ActivityActionButtonClickedDelegate)(void *actionPtr, const char *buttonId, const char *serializedActivityPost);

typedef void(UiActionListenerDelegate)(void *listenerPtr, int actionType);

typedef void(AvatarClickListenerDelegate)(void *listenerPtr, const char *serializedPublicUser);

typedef void(MentionClickListenerDelegate)(void *listenerPtr, const char *userId);

static GetSocialUIPendingAction sPendingAction = nil;

#pragma clang diagnostic push
#pragma ide diagnostic ignored "OCUnusedGlobalDeclarationInspection"
extern "C" {
NS_ASSUME_NONNULL_BEGIN

bool _gs_showSmartInvitesView(
        const char *title,
        const char *serializedInviteContent,
        const char *serializedLinkParams,
        StringCallbackDelegate stringCallback /* with providerId */, void *onInviteCompletePtr, void *onInviteCancelPtr,
        FailureWithDataCallbackDelegate failureCallback, void *onFailurePtr,
        VoidCallbackDelegate onOpenAction, void *onOpenActionPtr,
        VoidCallbackDelegate onCloseAction, void *onCloseActionPtr,
        UiActionListenerDelegate uiActionListener, void *uiActionListenerPtr) {

    GetSocialUIInvitesView *invitesView = [GetSocialUI createInvitesView];

    if (title) {
        NSString *titleStr = [GetSocialBridgeUtils createNSStringFrom:title];
        invitesView.windowTitle = titleStr;
    }

    if (serializedInviteContent) {
        NSString *serializedInviteContentStr = [GetSocialBridgeUtils createNSStringFrom:serializedInviteContent];
        GetSocialMutableInviteContent *customInviteContent = [GetSocialJsonUtils deserializeCustomInviteContent:serializedInviteContentStr];
        [invitesView setCustomInviteContent:customInviteContent];
    }

    if (serializedLinkParams) {
        NSString *serializedLinkParamsStr = [GetSocialBridgeUtils createNSStringFrom:serializedLinkParams];
        NSDictionary *linkParams = [GetSocialJsonUtils deserializeLinkParams:serializedLinkParamsStr];
        [invitesView setLinkParams:linkParams];
    }
    
    if (uiActionListenerPtr) {
        [invitesView setUiActionHandler:^(GetSocialUIActionType actionType, GetSocialUIPendingAction pendingAction) {
            sPendingAction = pendingAction;
            uiActionListener(uiActionListenerPtr, (int) actionType);
        }];
    }
    
    if (onInviteCompletePtr) {
        [invitesView setHandlerForInvitesSent:^(NSString *_Nonnull providerId) {
            stringCallback(onInviteCompletePtr, providerId.UTF8String);

        }                              cancel:^(NSString *_Nonnull providerId) {
            stringCallback(onInviteCancelPtr, providerId.UTF8String);
        }                             failure:^(NSString *_Nonnull providerId, NSError *_Nonnull error) {
            failureCallback(onFailurePtr, providerId.UTF8String, [error toJsonCString]);
        }];
    }
    
    if (onOpenActionPtr || onCloseActionPtr) {
        [invitesView setHandlerForViewOpen:^{
            onOpenAction(onOpenActionPtr);
        } close:^{
            onCloseAction(onCloseActionPtr);
        }];
    }

    return [invitesView show];
}

#pragma mark UI Configuration

bool _gs_loadConfiguration(const char *filePath) {
    NSString *filePathStr = [GetSocialBridgeUtils createNSStringFrom:filePath];
    return [GetSocialUI loadConfiguration:filePathStr];
}

bool _gs_loadDefaultConfiguration(const char *filePath) {
    return [GetSocialUI loadDefaultConfiguration];
}

#pragma mark Close-Open

bool _gs_closeView(bool saveViewState) {
    return [GetSocialUI closeView:saveViewState];
}

bool _gs_restoreView() {
    return [GetSocialUI restoreView];
}
    
#pragma mark - Activity Feed

bool _gs_showActivityFeedView(const char *windowTitle,
        const char *feed, const char *filterUserId, BOOL readOnly, BOOL friendsFeed, const char *tags,
        ActivityActionButtonClickedDelegate callback, void *onButtonClickPtr,
        VoidCallbackDelegate onOpenAction, void *onOpenActionPtr,
        VoidCallbackDelegate onCloseAction, void *onCloseActionPtr,
        UiActionListenerDelegate uiActionListener, void *uiActionListenerPtr,
        AvatarClickListenerDelegate avatarClickListener, void *avatarClickListenerPtr,
        AvatarClickListenerDelegate mentionClickListener, void *mentionClickListenerPtr,
        AvatarClickListenerDelegate tagClickListener, void *tagClickListenerPtr) {
    NSString *feedStr = [GetSocialBridgeUtils createNSStringFrom:feed];

    GetSocialUIActivityFeedView *view = [GetSocialUI createActivityFeedView:feedStr];
    
    if (windowTitle) {
        NSString *titleStr = [GetSocialBridgeUtils createNSStringFrom:windowTitle];
        view.windowTitle = titleStr;
    }

    if (onButtonClickPtr) {
        [view setActionButtonHandler:^(NSString *action, GetSocialActivityPost *post) {
            callback(onButtonClickPtr, action.UTF8String, post.toJsonCString);
        }];
    }
    
    if (uiActionListenerPtr) {
        [view setUiActionHandler:^(GetSocialUIActionType actionType, GetSocialUIPendingAction pendingAction) {
            sPendingAction = pendingAction;
            uiActionListener(uiActionListenerPtr, (int) actionType);
        }];
    }
    
    if (avatarClickListener) {
        [view setAvatarClickHandler:^(GetSocialPublicUser *user) {
            avatarClickListener(avatarClickListenerPtr, user.toJsonCString);
        }];
    }
    
    if (mentionClickListener) {
        [view setMentionClickHandler:^(GetSocialId userId) {
            mentionClickListener(mentionClickListenerPtr, [GetSocialBridgeUtils createCStringFrom:userId]);
        }];
    }

    if (tagClickListener) {
        [view setTagClickHandler:^(NSString *tag) {
            tagClickListener(tagClickListenerPtr, [GetSocialBridgeUtils createCStringFrom:tag]);
        }];
    }
    
    if (onOpenActionPtr || onCloseActionPtr) {
        [view setHandlerForViewOpen:^{
            onOpenAction(onOpenActionPtr);
        } close:^{
            onCloseAction(onCloseActionPtr);
        }];
    }
    if (filterUserId) {
        [view setFilterByUser:[GetSocialBridgeUtils createNSStringFrom:filterUserId]];
    }
    if (tags) {
        NSString *tagsStr = [GetSocialBridgeUtils createNSStringFrom:tags];
        NSArray *tagsArray = [GetSocialJsonUtils deserializeList:tagsStr];
        [view setFilterByTags:tagsArray];
    }
    [view setReadOnly:readOnly];
    [view setShowFriendsFeed:friendsFeed];
    return [view show];
}
    
BOOL _gs_showActivityDetailsView(const char *windowTitle,
                              const char *activityId,
                              BOOL showFeedView, BOOL readOnly, const char *commentId,
                              ActivityActionButtonClickedDelegate callback, void *onButtonClickPtr,
                              VoidCallbackDelegate onOpenAction, void *onOpenActionPtr,
                              VoidCallbackDelegate onCloseAction, void *onCloseActionPtr,
                              UiActionListenerDelegate uiActionListener, void *uiActionListenerPtr,
                              AvatarClickListenerDelegate avatarClickListener, void *avatarClickListenerPtr,
                              AvatarClickListenerDelegate mentionClickListener, void *mentionClickListenerPtr,
                              AvatarClickListenerDelegate tagClickListener, void *tagClickListenerPtr) {
    NSString *activityIdStr = [GetSocialBridgeUtils createNSStringFrom:activityId];
    
    GetSocialUIActivityDetailsView *view = [GetSocialUI createActivityDetailsView:activityIdStr];
    
    if (windowTitle) {
        NSString *titleStr = [GetSocialBridgeUtils createNSStringFrom:windowTitle];
        view.windowTitle = titleStr;
    }
    
    NSString *commentIdStr = [GetSocialBridgeUtils createNSStringFrom:commentId];
    if (commentIdStr.length) {
        [view setCommentId:commentIdStr];
    }
    
    if (onButtonClickPtr) {
        [view setActionButtonHandler:^(NSString *action, GetSocialActivityPost *post) {
            callback(onButtonClickPtr, action.UTF8String, [post toJsonCString]);
        }];
    }
    
    if (uiActionListenerPtr) {
        [view setUiActionHandler:^(GetSocialUIActionType actionType, GetSocialUIPendingAction pendingAction) {
            sPendingAction = pendingAction;
            uiActionListener(uiActionListenerPtr, (int) actionType);
        }];
    }
    
    if (onOpenActionPtr || onCloseActionPtr) {
        [view setHandlerForViewOpen:^{
            onOpenAction(onOpenActionPtr);
        } close:^{
            onCloseAction(onCloseActionPtr);
        }];
    }
    if (avatarClickListener) {
        [view setAvatarClickHandler:^(GetSocialPublicUser *user) {
            avatarClickListener(avatarClickListenerPtr, user.toJsonCString );
        }];
    }
    
    if (mentionClickListener) {
        [view setMentionClickHandler:^(GetSocialId userId) {
            mentionClickListener(mentionClickListenerPtr, [GetSocialBridgeUtils createCStringFrom:userId]);
        }];
    }

    if (tagClickListener) {
        [view setTagClickHandler:^(NSString *tag) {
            tagClickListener(tagClickListenerPtr, [GetSocialBridgeUtils createCStringFrom:tag]);
        }];
    }
    
    [view setReadOnly:readOnly];
    
    [view setShowActivityFeedView:showFeedView];
    return [view show];
}
    
void _gs_doPendingAction() {
    if (sPendingAction) {
        sPendingAction();
        sPendingAction = nil;
    }
}

NS_ASSUME_NONNULL_END
}

#pragma clang diagnostic pop

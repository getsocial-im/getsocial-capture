//
//  GetSocialJsonUtils.h
//  Unity-iPhone
//
//  Created by Taras Leskiv on 19/12/2016.
//
//

#import <Foundation/Foundation.h>
#import <GetSocial/GetSocial.h>

@interface GetSocialJsonUtils : NSObject

#pragma mark - Deserialize - received as strings FROM Unity

+ (GetSocialMutableInviteContent *)deserializeCustomInviteContent:(NSString *)customInviteContentJson;

+ (NSDictionary *)deserializeLinkParams:(NSString *)customLinkParamsJson;

+ (GetSocialActivitiesQuery *)deserializeActivitiesQuery:(NSString *)serializedQuery;

+ (GetSocialActivityPostContent *)deserializeActivityContent:(NSString *)content;

+ (GetSocialAuthIdentity *)deserializeIdentity:(NSString *)identity;

+ (GetSocialUsersQuery *)deserializeUsersQuery:(NSString *)query;

+ (GetSocialNotificationsCountQuery *)deserializeNotificationsCountQuery:(NSString *)serializedQuery;

+ (GetSocialNotificationsQuery *)deserializeNotificationsQuery:(NSString *)serializedQuery;

+ (NSArray *)deserializeList:(NSString *)jsonList;

@end

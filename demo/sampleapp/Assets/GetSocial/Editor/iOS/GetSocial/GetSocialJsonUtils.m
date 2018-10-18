//
//  GetSocialJsonUtils.m
//  Unity-iPhone
//
//  Created by Taras Leskiv on 19/12/2016.
//
//

#import "GetSocialJsonUtils.h"
#import "GetSocialBridgeUtils.h"

@implementation GetSocialJsonUtils

#pragma mark - Deserialize - received as strings FROM Unity

+ (GetSocialMutableInviteContent *)deserializeCustomInviteContent:(NSString *)customInviteContentJson
{
    NSDictionary *json = [self deserializeDictionary:customInviteContentJson];

    GetSocialMutableInviteContent *content = [[GetSocialMutableInviteContent alloc] init];
    content.subject = json[@"Subject"];
    content.imageUrl = json[@"ImageUrl"];
    content.text = json[@"Text"];
    content.image = [GetSocialBridgeUtils decodeUIImageFrom:json[@"Image"]];
    content.video = [GetSocialBridgeUtils decodeNSDataFrom:json[@"Video"]];

    return content;
}

+ (NSDictionary *)deserializeLinkParams:(NSString *)customLinkParamsJson
{

#if DEBUG
    NSLog(@"JSON Input: %@", customLinkParamsJson);
#endif
    if (customLinkParamsJson == nil)
    {
        return nil;
    }

    NSDictionary<NSString*, id> *json = [self deserializeDictionary:customLinkParamsJson];
    id rawImage = json[@"$image"];
    if (rawImage != nil)
    {
        UIImage* image = [GetSocialBridgeUtils decodeUIImageFrom:(NSString*)rawImage];
        if (image)
        {
            [json setValue:image forKey:@"$image"];
        }
    }
    return json;
}

+ (GetSocialActivitiesQuery *)deserializeActivitiesQuery:(NSString *)serializedQuery
{
    NSDictionary *json = [self deserializeDictionary:serializedQuery];
    NSString *feed = json[@"Feed"];
    GetSocialActivitiesQuery *query = feed == nil
    ? [GetSocialActivitiesQuery commentsToPost:json[@"ParentActivityId"]]
    : [GetSocialActivitiesQuery postsForFeed:feed];

    // Limit
    int limit = [json[@"Limit"] intValue];
    [query setLimit:limit];
    [query setFilterByUser:json[@"FilterUserId"]];

    // Filtering
    int filter = [json[@"Filter"] intValue];
    if (filter != 0)
    {
        [query setFilter:(GetSocialActivitiesFilter)filter activityId:json[@"FilteringActivityId"]];
    }

    NSArray *tags = json[@"Tags"];
    if (tags) 
    {
        [query setTags:tags];
    }
    BOOL isFriendsFeed = [json[@"FriendsFeed"] boolValue];
    [query setIsFriendsFeed:isFriendsFeed];

    return query;
}

+ (GetSocialActivityPostContent *)deserializeActivityContent:(NSString *)content
{
    NSDictionary *dictionary = [self deserializeDictionary:content];

    GetSocialActivityPostContent *postContent = [GetSocialActivityPostContent new];
    postContent.text = dictionary[@"Text"];
    postContent.buttonTitle = dictionary[@"ButtonTitle"];
    postContent.buttonAction = dictionary[@"ButtonAction"];
    postContent.image = [GetSocialBridgeUtils decodeUIImageFrom:dictionary[@"Image"]];
    postContent.video = [GetSocialBridgeUtils decodeNSDataFrom:dictionary[@"Video"]];

    return postContent;
}

+ (GetSocialAuthIdentity *)deserializeIdentity:(NSString *)identity
{
    NSDictionary *dictionary = [self deserializeDictionary:identity];

    return [GetSocialAuthIdentity customIdentityForProvider:dictionary[@"ProviderId"]
                                                     userId:dictionary[@"ProviderUserId"]
                                                accessToken:dictionary[@"AccessToken"]];
}

+ (GetSocialUsersQuery *)deserializeUsersQuery:(NSString *)query
{
    NSDictionary *dictionary = [self deserializeDictionary:query];
    
    GetSocialUsersQuery *usersQuery = [GetSocialUsersQuery usersByDisplayName:dictionary[@"Query"]];
    [usersQuery setLimit:[dictionary[@"Limit"] intValue]];
    return usersQuery;
}

+ (GetSocialNotificationsQuery *)deserializeNotificationsQuery:(NSString *)serializedQuery
{
    NSDictionary *json = [self deserializeDictionary:serializedQuery];
    NSNumber *isRead = json[@"IsRead"];
    
    GetSocialNotificationsQuery *query = isRead == nil
    ? [GetSocialNotificationsQuery readAndUnread]
    : [isRead boolValue] ? [GetSocialNotificationsQuery read] : [GetSocialNotificationsQuery unread];
    
    // Limit
    int limit = [json[@"Limit"] intValue];
    [query setLimit:limit];
    
    // Filtering
    int filter = [json[@"Filter"] intValue];
    if (filter != 0)
    {
        [query setFilter:(GetSocialNotificationsFilter)filter notificationId:json[@"FilteringNotificationId"]];
    }
    id types = json[@"Types"];
    [query setTypes:types];
    
    return query;
}

+ (GetSocialNotificationsCountQuery *)deserializeNotificationsCountQuery:(NSString *)serializedQuery
{
    NSDictionary *json = [self deserializeDictionary:serializedQuery];
    NSNumber *isRead = json[@"IsRead"];
    GetSocialNotificationsCountQuery *query = isRead == nil
    ? [GetSocialNotificationsCountQuery readAndUnread]
    : [isRead boolValue] ? [GetSocialNotificationsCountQuery read] : [GetSocialNotificationsCountQuery unread];
    
    
    id types = json[@"Types"];
    [query setTypes:types];
    
    return query;
}
#pragma mark - Helpers

+ (NSDictionary *)deserializeDictionary:(NSString *)jsonDic
{
    NSError *e = nil;
    NSDictionary *dictionary = [NSJSONSerialization JSONObjectWithData:[jsonDic dataUsingEncoding:NSUTF8StringEncoding] options:NSJSONReadingMutableContainers error:&e];
    if (dictionary != nil)
    {
        NSMutableDictionary *prunedDict = [NSMutableDictionary dictionary];
        [dictionary enumerateKeysAndObjectsUsingBlock:^(NSString *key, id obj, BOOL *stop) {
            if (![obj isKindOfClass:[NSNull class]]) {
                prunedDict[key] = obj;
            }
        }];
        return prunedDict;
    }
    return dictionary;
}

+ (NSArray *)deserializeList:(NSString *)jsonList
{
    NSError* localError = nil;
    NSArray *array = [NSJSONSerialization JSONObjectWithData:[jsonList dataUsingEncoding:NSUTF8StringEncoding] options:NSJSONReadingMutableContainers error:&localError];

    return array;
}

@end

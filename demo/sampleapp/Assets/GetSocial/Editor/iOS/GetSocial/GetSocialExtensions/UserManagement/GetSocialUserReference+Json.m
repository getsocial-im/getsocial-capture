//
//  GetSocialUserReference+Json.m
//  Unity-iPhone
//
//  Created by Orest Savchak on 11/3/17.
//

#import "GetSocialUserReference+Json.h"
#import "NSMutableDictionary+GetSocial.h"
@implementation GetSocialUserReference (Json)

- (NSMutableDictionary *)toJsonDictionary
{
    NSMutableDictionary *dictionary = [@{} mutableCopy];
    [dictionary gs_setValueOrNSNull:self.userId forKey:@"UserId"];
    [dictionary gs_setValueOrNSNull:self.displayName forKey:@"DisplayName"];
    [dictionary gs_setValueOrNSNull:self.avatarUrl forKey:@"AvatarUrl"];
    return dictionary;
}

@end

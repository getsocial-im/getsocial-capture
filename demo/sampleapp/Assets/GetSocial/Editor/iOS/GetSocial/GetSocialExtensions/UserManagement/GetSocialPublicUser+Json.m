//
// Created by Orest Savchak on 6/2/17.
//

#import "GetSocialPublicUser+Json.h"
#import "NSMutableDictionary+GetSocial.h"

@implementation GetSocialPublicUser (Json)

- (NSMutableDictionary *)toJsonDictionary
{
    NSMutableDictionary *dictionary = [[NSMutableDictionary alloc] init];
    [dictionary gs_setValueOrNSNull:self.userId forKey:@"Id"];
    [dictionary gs_setValueOrNSNull:self.displayName forKey:@"DisplayName"];
    [dictionary gs_setValueOrNSNull:self.avatarUrl forKey:@"AvatarUrl"];
    [dictionary gs_setValueOrNSNull:[self.authIdentities mutableCopy] forKey:@"Identities"];
    [dictionary gs_setValueOrNSNull:[self.allPublicProperties mutableCopy] forKey:@"PublicProperties"];
    return dictionary;
}

@end

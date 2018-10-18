//
// Created by Orest Savchak on 6/2/17.
//

#import "GetSocialInviteChannel+Json.h"
#import "NSMutableDictionary+GetSocial.h"
#import "GetSocialInviteContent+Json.h"

@implementation GetSocialInviteChannel (Json)

- (NSMutableDictionary *)toJsonDictionary
{
    NSMutableDictionary *dictionary = [[NSMutableDictionary alloc] init];
    [dictionary gs_setValueOrNSNull:self.channelId forKey:@"Id"];
    [dictionary gs_setValueOrNSNull:self.name forKey:@"Name"];
    [dictionary gs_setValueOrNSNull:self.iconUrl forKey:@"IconImageUrl"];
    [dictionary gs_setValueOrNSNull:self.description forKey:@"Description"];
    [dictionary setValue:@(self.displayOrder) forKey:@"DisplayOrder"];
    [dictionary setValue:@(self.enabled) forKey:@"IsEnabled"];
    [dictionary setValue:[self.inviteContent toJsonDictionary] forKey:@"InviteContent"];
    return dictionary;
}

@end
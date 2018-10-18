//
// Created by Orest Savchak on 6/2/17.
//

#import "GetSocialNotification+Json.h"
#import "NSMutableDictionary+GetSocial.h"

@implementation GetSocialNotification (Json)

- (NSMutableDictionary *)toJsonDictionary
{
    NSMutableDictionary *dictionary = @{}.mutableCopy;
    [dictionary gs_setValueOrNSNull:self.title forKey:@"Title"];
    [dictionary gs_setValueOrNSNull:self.text forKey:@"Text"];
    [dictionary gs_setValueOrNSNull:self.actionData forKey:@"Data"];
    [dictionary gs_setValueOrNSNull:self.notificationId forKey:@"Id"];
    [dictionary gs_setValueOrNSNull:@(self.type) forKey:@"Type"];;
    [dictionary gs_setValueOrNSNull:@(self.action) forKey:@"ActionType"];
    [dictionary gs_setValueOrNSNull:@(self.wasRead) forKey:@"WasRead"];
    [dictionary gs_setValueOrNSNull:@(self.createdAt) forKey:@"CreatedAt"];
    return dictionary;
}

@end

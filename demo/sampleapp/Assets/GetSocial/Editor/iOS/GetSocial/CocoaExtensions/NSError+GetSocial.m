//
// Created by Orest Savchak on 6/2/17.
//

#import "NSError+GetSocial.h"
#import "NSMutableDictionary+GetSocial.h"


@implementation NSError (GetSocial)

- (NSMutableDictionary *)toJsonDictionary
{
    NSMutableDictionary *dictionary = [[NSMutableDictionary alloc] init];

    [dictionary gs_setValueOrNSNull:self.localizedDescription forKey:@"Message"];
    [dictionary setValue:@(self.code) forKey:@"ErrorCode"];


    if (self.userInfo)
    {
        NSMutableDictionary *userInfo = [[NSMutableDictionary alloc] init];
        for (NSString *key in self.userInfo)
        {
            userInfo[key] = [self.userInfo[key] description];
        }
        [dictionary gs_setValueOrNSNull:userInfo forKey:@"UserInfo"];
    }

    return dictionary;
}

@end
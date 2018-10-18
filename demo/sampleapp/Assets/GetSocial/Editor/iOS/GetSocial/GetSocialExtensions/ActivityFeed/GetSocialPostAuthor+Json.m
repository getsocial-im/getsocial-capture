//
// Created by Orest Savchak on 6/2/17.
//

#import "GetSocialPostAuthor+Json.h"
#import "GetSocialPublicUser+Json.h"


@implementation GetSocialPostAuthor (Json)

- (NSMutableDictionary *)toJsonDictionary
{
    NSMutableDictionary *dictionary = [super toJsonDictionary];
    dictionary[@"IsVerified"]       = @(self.verified);
    
    return dictionary;
}

@end

//
//  GetSocialMention+Json.m
//  Unity-iPhone
//
//  Created by Orest Savchak on 11/3/17.
//

#import "GetSocialMention+Json.h"

@implementation GetSocialMention (Json)

- (NSMutableDictionary *)toJsonDictionary
{
    NSMutableDictionary *dictionary = [@{} mutableCopy];
    dictionary[@"UserId"]       = self.userId;
    dictionary[@"StartIndex"]   = @(self.startIndex);
    dictionary[@"EndIndex"]     = @(self.endIndex);
    dictionary[@"Type"]         = self.type;
    return dictionary;
}

@end

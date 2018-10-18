//
//  GetSocialReferredUser+Json.m
//  Unity-iPhone
//
//

#import "GetSocialReferredUser+Json.h"
#import "GetSocialPublicUser+Json.h"

@implementation GetSocialReferredUser(Json)

- (NSMutableDictionary *)toJsonDictionary
{
    NSMutableDictionary *dictionary = [super toJsonDictionary];
    dictionary[@"InstallationDate"] = @(self.installationDate);
    dictionary[@"InstallationChannel"] = self.installationChannel;
    return dictionary;
}

@end

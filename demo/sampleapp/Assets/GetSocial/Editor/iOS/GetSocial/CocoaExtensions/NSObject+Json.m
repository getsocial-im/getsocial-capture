//
// Created by Orest Savchak on 6/2/17.
//

#import "NSObject+Json.h"
#import "Json.h"
#import "NSDictionary+GetSocial.h"
#import "GetSocialBridgeUtils.h"
#import <Foundation/Foundation.h>

@implementation NSObject (Json)

- (NSString *)toJsonString
{
    if ([self conformsToProtocol:NSProtocolFromString(@"Json")]) {
        id<Json> _self = (id <Json>) self;
        return [[_self toJsonDictionary] toJson];
    } else {
        [NSException raise:NSInvalidArgumentException format:@"Class %@ should conform to Json protocol for using toJsonString.", [self class] ];
        return nil;
    }
}

- (char *)toJsonCString
{
    return [GetSocialBridgeUtils createCStringFrom:[self toJsonString] ];
}

@end

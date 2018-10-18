//
// Created by Orest Savchak on 6/2/17.
//

#import "NSDictionary+GetSocial.h"


@implementation NSDictionary (GetSocial)

- (NSString *)toJson
{
    NSError *writeError = nil;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:self options:nil error:&writeError];

    NSString *jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];

    NSLog(@"JSON Output: %@", jsonString);

    return jsonString;
}

- (NSString *)toJsonString
{
    return [self toJson];
}

@end
//
// Created by Orest Savchak on 6/2/17.
//

#import "NSArray+GetSocial.h"
#import "Json.h"

@implementation NSArray (GetSocial)

- (NSArray<id> *)gs_map:(id (^)(id))block
{
    NSMutableArray *array = [NSMutableArray arrayWithCapacity:self.count];
    for (id it in self) {
        [array addObject:block(it)];
    }
    return [NSArray arrayWithArray:array];
}

- (NSString *)toJson
{
    NSError *writeError = nil;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:self options:nil error:&writeError];

    NSString *jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];

#if DEBUG
    NSLog(@"JSON Output: %@", jsonString);
#endif

    return jsonString;
}

- (NSString *)toJsonArrayString
{
    return [[self gs_map:^id(id<Json> it) {
        if ([it respondsToSelector:@selector(toJsonDictionary)]) {
            return [it toJsonDictionary];
        } else if ([it isKindOfClass:[NSString class] ]) {
            return it;
        } else if ([it isKindOfClass:[NSDictionary class] ]) {
            return it;
        } else {
            return it;
        }
    }] toJson];
}

- (NSString *)toJsonString
{
    return [self toJsonArrayString];
}

@end

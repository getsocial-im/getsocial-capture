//
// Created by Orest Savchak on 6/2/17.
//

#import "NSMutableDictionary+GetSocial.h"

@implementation NSMutableDictionary (GetSocial)

- (void)gs_setValueOrNSNull:(id)value forKey:(NSString *)key
{
    if (value) {
        self[key] = value;
    } else {
        self[key] = [NSNull null];
    }
}

@end
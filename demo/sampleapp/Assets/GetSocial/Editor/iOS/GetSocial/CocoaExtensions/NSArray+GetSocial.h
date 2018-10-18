//
// Created by Orest Savchak on 6/2/17.
//

#import <Foundation/Foundation.h>

@interface NSArray<__covariant ObjectType> (GetSocial)

- (NSArray<id> *)gs_map:(id (^)(ObjectType it))block;

@end
#import <Foundation/Foundation.h>
#import <UIKit/UIImage.h>

@interface GetSocialBridgeUtils : NSObject

+ (NSString *)createNSStringFrom:(const char *)cstring;

+ (char *)createCStringFrom:(NSString *)string;

+ (char *)cStringCopy:(const char *)string;

+ (UIImage *)decodeUIImageFrom:(NSString *)base64String;

+ (NSData *)decodeNSDataFrom:(NSString *)base64String;

@end

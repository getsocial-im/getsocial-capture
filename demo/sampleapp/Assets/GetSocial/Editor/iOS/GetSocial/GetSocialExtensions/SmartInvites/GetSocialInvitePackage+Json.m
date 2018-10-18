//
// Created by Orest Savchak on 6/2/17.
//

#import "GetSocialInvitePackage+Json.h"
#import "NSMutableDictionary+GetSocial.h"
#import "UIImage+GetSocial.h"


@implementation GetSocialInvitePackage (Json)

- (NSMutableDictionary *)toJsonDictionary
{
    NSMutableDictionary *dictionary = [NSMutableDictionary new];
    [dictionary gs_setValueOrNSNull:self.subject forKey:@"Subject"];
    [dictionary gs_setValueOrNSNull:self.text forKey:@"Text"];
    [dictionary gs_setValueOrNSNull:self.userName forKey:@"UserName"];
    [dictionary gs_setValueOrNSNull:self.referralUrl forKey:@"ReferralDataUrl"];
    [dictionary gs_setValueOrNSNull:[self.image toBase64] forKey:@"Image"];
    [dictionary gs_setValueOrNSNull:self.imageUrl forKey:@"ImageUrl"];
    return dictionary;
}

@end

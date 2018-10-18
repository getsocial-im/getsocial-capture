//
// Created by Orest Savchak on 6/2/17.
//

#import "GetSocialInviteContent+Json.h"
#import "NSMutableDictionary+GetSocial.h"
#import "UIImage+GetSocial.h"


@implementation GetSocialInviteContent (Json)

- (NSMutableDictionary *)toJsonDictionary
{
    NSMutableDictionary *dictionary = [[NSMutableDictionary alloc] init];
    [dictionary gs_setValueOrNSNull:self.imageUrl forKey:@"ImageUrl"];
    [dictionary gs_setValueOrNSNull:self.subject forKey:@"Subject"];
    [dictionary gs_setValueOrNSNull:self.text forKey:@"Text"];
    [dictionary gs_setValueOrNSNull:[self.image toBase64] forKey:@"Image"];
    [dictionary gs_setValueOrNSNull:self.gifUrl forKey:@"GifUrl"];
    [dictionary gs_setValueOrNSNull:self.videoUrl forKey:@"VideoUrl"];
    return dictionary;
}

@end

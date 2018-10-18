//
// Created by Orest Savchak on 6/2/17.
//

#import "UIImage+GetSocial.h"


@implementation UIImage (GetSocial)

- (NSString *)toBase64
{
    return [UIImagePNGRepresentation(self) base64EncodedStringWithOptions:NSDataBase64Encoding64CharacterLineLength];
}

@end
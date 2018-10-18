//
// Created by Orest Savchak on 6/2/17.
//

#import "GetSocialActivityPost+Json.h"
#import "NSMutableDictionary+GetSocial.h"
#import "GetSocialPostAuthor+Json.h"
#import "NSObject+Json.h"

@implementation GetSocialActivityPost (Json)

- (NSMutableDictionary *)toJsonDictionary
{
    NSMutableDictionary *dictionary = [NSMutableDictionary new];

    [dictionary gs_setValueOrNSNull:self.activityId forKey:@"Id"];
    [dictionary gs_setValueOrNSNull:self.text forKey:@"Text"];
    [dictionary gs_setValueOrNSNull:self.imageUrl forKey:@"ImageUrl"];
    [dictionary gs_setValueOrNSNull:@(self.createdAt) forKey:@"CreatedAt"];
    [dictionary gs_setValueOrNSNull:@(self.stickyStart) forKey:@"StickyStart"];
    [dictionary gs_setValueOrNSNull:@(self.stickyEnd) forKey:@"StickyEnd"];
    [dictionary gs_setValueOrNSNull:self.buttonTitle forKey:@"ButtonTitle"];
    [dictionary gs_setValueOrNSNull:self.buttonAction forKey:@"ButtonAction"];
    [dictionary gs_setValueOrNSNull:@(self.commentsCount) forKey:@"CommentsCount"];
    [dictionary gs_setValueOrNSNull:@(self.likesCount) forKey:@"LikesCount"];
    [dictionary gs_setValueOrNSNull:@(self.isLikedByMe) forKey:@"IsLikedByMe"];
    [dictionary gs_setValueOrNSNull:[self.author toJsonDictionary] forKey:@"Author"];
    [dictionary gs_setValueOrNSNull:self.feedId forKey:@"FeedId"];
    [dictionary gs_setValueOrNSNull:self.mentions.toJsonString forKey:@"Mentions"];
    [dictionary gs_setValueOrNSNull:self.videoUrl forKey:@"VideoUrl"];

    return dictionary;
}

@end

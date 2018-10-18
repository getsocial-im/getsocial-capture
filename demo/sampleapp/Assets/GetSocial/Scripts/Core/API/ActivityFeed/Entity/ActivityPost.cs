using System;
using System.Collections.Generic;

#if UNITY_ANDROID
using System.Linq;
using UnityEngine;
#endif

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Activity post entity. Contains all information about post, its author and content.
    /// </summary>
    public sealed class ActivityPost : IConvertableFromNative<ActivityPost>
    {
        /// <summary>
        /// Type of Activity Feed content
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// Activity Feed Post
            /// </summary>
            Post = 0,
            /// <summary>
            /// Activity Feed Comment
            /// </summary>
            Comment = 1
        }

        /// <summary>
        /// Gets the activity identifier.
        /// </summary>
        /// <value>The activity identifier.</value>
        public string Id { get; private set; }

        /// <summary>
        /// Gets the activity text.
        /// </summary>
        /// <value>The activity text.</value>
        public string Text { get; private set; }

        /// <summary>
        /// Gets a value indicating whether activity has text.
        /// </summary>
        /// <value><c>true</c> if activity has text; otherwise, <c>false</c>.</value>
        public bool HasText
        {
            get { return !string.IsNullOrEmpty(Text); }
        }

        /// <summary>
        /// Gets the image URL or null if activity has no image.
        /// </summary>
        /// <value>The image URL or null if activity has no image.</value>
        public string ImageUrl { get; private set; }

        /// <summary>
        /// Gets a value indicating whether activity has image.
        /// </summary>
        /// <value><c>true</c> if activity has image; otherwise, <c>false</c>.</value>
        public bool HasImage
        {
            get { return !string.IsNullOrEmpty(ImageUrl); }
        }

        /// <summary>
        /// Date of post creation.
        /// </summary>
        /// <value>Date of post creation.</value>
        public DateTime CreatedAt { get; private set; }

        /// <summary>
        /// Gets the button title.
        /// </summary>
        /// <value>The button title or <c>null</c> if post has no button.</value>
        public string ButtonTitle { get; private set; }

        /// <summary>
        /// Gets the button action id.
        /// </summary>
        /// <value>The button action id or <c>null</c> if post has no button.</value>
        public string ButtonAction { get; private set; }

        /// <summary>
        /// Gets a value indicating whether activity has button.
        /// </summary>
        /// <value><c>true</c> if activity has button; otherwise, <c>false</c>.</value>
        public bool HasButton
        {
            get { return ButtonTitle != null; }
        }

        /// <summary>
        /// Gets the post author.
        /// </summary>
        /// <value>The post author.</value>
        public PostAuthor Author { get; private set; }

        /// <summary>
        /// Gets the number of comments.
        /// </summary>
        /// <value>The number of cemments.</value>
        public int CommentsCount { get; private set; }

        /// <summary>
        /// Gets the likes count.
        /// </summary>
        /// <value>The likes count.</value>
        public int LikesCount { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this post is liked by me.
        /// </summary>
        /// <value><c>true</c> if this post is liked by me; otherwise, <c>false</c>.</value>
        public bool IsLikedByMe { get; private set; }

        /// <summary>
        /// Gets the sticky post start date.
        /// </summary>
        /// <value>The sticky post start date.</value>
        public DateTime StickyStart { get; private set; }

        /// <summary>
        /// Gets the sticky post end date.
        /// </summary>
        /// <value>The sticky post end date.</value>
        public DateTime StickyEnd { get; private set; }

        /// <summary>
        /// Determines if the post is sticky at the specified dateTime.
        /// </summary>
        /// <returns><c>true</c> if post is sticky at the specified dateTime; otherwise, <c>false</c>.</returns>
        /// <param name="dateTime">Datetime to check.</param>
        public bool IsStickyAt(DateTime dateTime)
        {
            return dateTime.Ticks > StickyStart.Ticks && dateTime.Ticks < StickyEnd.Ticks;
        }

        /// <summary>
        /// List of mentions in activity post.
        /// </summary>
        public List<Mention> Mentions { get; private set; }

        /// <summary>
        /// Feed name that this activity belongs to.
        /// </summary>
        public string FeedId { get; private set;  }

        public override string ToString()
        {
            return string.Format(
                "Id: {0}, Text: {1}, HasText: {2}, ImageUrl: {3}, HasImage: {4}, CreatedAt: {5}, ButtonTitle: {6}, ButtonAction: {7}, HasButton: {8}, Author: {9}, CommentsCount: {10}, LikesCount: {11}, IsLikedByMe: {12}, StickyStart: {13}, StickyEnd: {14}, FeedId: {15}, Mentions: {16}",
                Id, Text, HasText, ImageUrl, HasImage, CreatedAt, ButtonTitle, ButtonAction, HasButton, Author,
                CommentsCount, LikesCount, IsLikedByMe, StickyStart, StickyEnd, FeedId, Mentions.ToDebugString());
        }

        public ActivityPost()
        {
            
        }

        internal ActivityPost(string id, string text, string imageUrl, DateTime createdAt, string buttonTitle, string buttonAction, PostAuthor author, int commentsCount, int likesCount, bool isLikedByMe, DateTime stickyStart, DateTime stickyEnd, List<Mention> mentions, string feedId)
        {
            Id = id;
            Text = text;
            ImageUrl = imageUrl;
            CreatedAt = createdAt;
            ButtonTitle = buttonTitle;
            ButtonAction = buttonAction;
            Author = author;
            CommentsCount = commentsCount;
            LikesCount = likesCount;
            IsLikedByMe = isLikedByMe;
            StickyStart = stickyStart;
            StickyEnd = stickyEnd;
            Mentions = mentions;
            FeedId = feedId;
        }

        private bool Equals(ActivityPost other)
        {
            return string.Equals(Id, other.Id) && string.Equals(Text, other.Text) && string.Equals(ImageUrl, other.ImageUrl) && CreatedAt.Equals(other.CreatedAt) && string.Equals(ButtonTitle, other.ButtonTitle) && string.Equals(ButtonAction, other.ButtonAction) && Equals(Author, other.Author) && CommentsCount == other.CommentsCount && LikesCount == other.LikesCount && IsLikedByMe == other.IsLikedByMe && StickyStart.Equals(other.StickyStart) && StickyEnd.Equals(other.StickyEnd) && Mentions.ListEquals(other.Mentions) && string.Equals(FeedId, other.FeedId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is ActivityPost && Equals((ActivityPost) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Id != null ? Id.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Text != null ? Text.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ImageUrl != null ? ImageUrl.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ CreatedAt.GetHashCode();
                hashCode = (hashCode * 397) ^ (ButtonTitle != null ? ButtonTitle.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ButtonAction != null ? ButtonAction.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Author != null ? Author.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ CommentsCount;
                hashCode = (hashCode * 397) ^ LikesCount;
                hashCode = (hashCode * 397) ^ IsLikedByMe.GetHashCode();
                hashCode = (hashCode * 397) ^ StickyStart.GetHashCode();
                hashCode = (hashCode * 397) ^ StickyEnd.GetHashCode();
                hashCode = (hashCode * 397) ^ (Mentions != null ? Mentions.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (FeedId != null ? FeedId.GetHashCode() : 0);
                return hashCode;
            }
        }

#if UNITY_ANDROID

        public ActivityPost ParseFromAJO(AndroidJavaObject ajo)
        {
            using (ajo)
            {
                Id = ajo.CallStr("getId");
                Text = ajo.CallStr("getText");
                ImageUrl = ajo.CallStr("getImageUrl");
                ButtonTitle = ajo.CallStr("getButtonTitle");
                ButtonAction = ajo.CallStr("getButtonAction");
                CreatedAt = DateUtils.FromUnixTime(ajo.CallLong("getCreatedAt"));
                Author = new PostAuthor().ParseFromAJO(ajo.CallAJO("getAuthor"));
                CommentsCount = ajo.CallInt("getCommentsCount");
                LikesCount = ajo.CallInt("getLikesCount");
                IsLikedByMe = ajo.CallBool("isLikedByMe");

                StickyStart = DateUtils.FromUnixTime(ajo.CallLong("getStickyStart"));
                StickyEnd = DateUtils.FromUnixTime(ajo.CallLong("getStickyEnd"));
                FeedId = ajo.CallStr("getFeedId");
                Mentions = ajo.CallAJO("getMentions").FromJavaList().ConvertAll(mentionAjo =>
                {
                    using (mentionAjo)
                    {
                        return new Mention().ParseFromAJO(mentionAjo);
                    }
                }).ToList();
            }
            return this;
        }

#elif UNITY_IOS
        public ActivityPost ParseFromJson(Dictionary<string, object> json)
        {
            Id = (string) json["Id"];
            Text = json["Text"] as string;

            ImageUrl = json["ImageUrl"] as string;
            ButtonTitle = json["ButtonTitle"] as string;
            ButtonAction = json["ButtonAction"] as string;
            CreatedAt = DateUtils.FromUnixTime((long) json["CreatedAt"]);

            var authorDic = json["Author"] as Dictionary<string, object>;
            Author = new PostAuthor().ParseFromJson(authorDic);

            CommentsCount = (int) (long) json["CommentsCount"];
            LikesCount = (int) (long) json["LikesCount"];
            IsLikedByMe = (bool) json["IsLikedByMe"];

            StickyStart = DateUtils.FromUnixTime((long) json["StickyStart"]);
            StickyEnd = DateUtils.FromUnixTime((long) json["StickyEnd"]);
            Mentions = GSJsonUtils.ParseList<Mention>(json["Mentions"] as string);
            FeedId = json["FeedId"] as string;
            
            return this;
        }
#endif
    }
}
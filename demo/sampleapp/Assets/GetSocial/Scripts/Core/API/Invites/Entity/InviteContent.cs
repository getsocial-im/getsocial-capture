using UnityEngine;

#if UNITY_IOS
using System.Collections.Generic;
using GetSocialSdk.MiniJSON;
#endif

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Invite content being sent along with smart invite.
    /// </summary>
    public sealed class InviteContent : IConvertableToNative
    {
        public static Builder CreateBuilder()
        {
            return new Builder();
        }

        /// <summary>
        /// Gets the image URL.
        /// </summary>
        /// <value>The image URL.</value>
        public string ImageUrl { get; private set; }

        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <value>Invite content image.</value>
        public Texture2D Image { get; private set; }

        /// <summary>
        /// Gets the subject of ivite.
        /// </summary>
        /// <value>The subject of invite.</value>
        public string Subject { get; private set; }

        /// <summary>
        /// Gets the invite text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; private set; }
        
        /// <summary>
        /// Gets the gif url.
        /// </summary>
        /// <value>Invite content gif url.</value>
        public string GifUrl { get; private set; }
        
        /// <summary>
        /// Gets the video url.
        /// </summary>
        /// <value>Invite content video url.</value>
        public string VideoUrl { get; private set; }

        /// <summary>
        /// Gets the video content.
        /// </summary>
        /// <value>Invite video content.</value>
        public byte[] Video { get; private set; }

        public override string ToString()
        {
            return string.Format("[InviteContent: ImageUrl={0}, Subject={1}, Text={2}, HasImage={3}, GifUrl={4}, VideoUrl={5}]", ImageUrl, Subject, Text, Image != null, GifUrl, VideoUrl);
        }

        /// <summary>
        /// Builder to create <see cref="InviteContent" instance/>.
        /// </summary>
        public class Builder
        {
            readonly InviteContent _inviteContent;

            protected internal Builder()
            {
                _inviteContent = new InviteContent();
            }

            /// <summary>
            /// Sets the invite subject.
            /// </summary>
            /// <param name="subject">Invite subject.</param>
            /// <returns>The builder instance.</returns>
            public Builder WithSubject(string subject)
            {
                _inviteContent.Subject = subject;
                return this;
            }

            /// <summary>
            /// Sets the invite text.
            /// </summary>
            /// <param name="text">Invite text.</param>
            /// <returns>The builder instance.</returns>
            public Builder WithText(string text)
            {
                _inviteContent.Text = text;
                return this;
            }

            /// <summary>
            /// Sets the invite image url.
            /// </summary>
            /// <param name="imageUrl">Invite image url.</param>
            /// <returns>The builder instance.</returns>
            public Builder WithImageUrl(string imageUrl)
            {
                _inviteContent.ImageUrl = imageUrl;
                return this;
            }

            /// <summary>
            /// Sets the invite image. 
            /// </summary>
            /// <param name="image">Invite image</param>
            /// <returns>The builder instance.</returns>
            public Builder WithImage(Texture2D image)
            {
                _inviteContent.Image = image;
                return this;
            }

            /// <summary>
            /// Sets the invite video.
            /// </summary>
            /// <param name="videoBytes">Invite video</param>
            /// <returns>The builder instance.</returns>
            public Builder WithVideo(byte[] videoBytes)
            {
                _inviteContent.Video = videoBytes;
                return this;
            }

            /// <summary>
            /// Build this instance.
            /// </summary>
            public InviteContent Build()
            {
                return new InviteContent
                {
                    ImageUrl = _inviteContent.ImageUrl,
                    Image = _inviteContent.Image,
                    Subject = _inviteContent.Subject,
                    Text = _inviteContent.Text,
                    GifUrl =  _inviteContent.GifUrl,
                    VideoUrl = _inviteContent.VideoUrl,
                    Video = _inviteContent.Video
                };
            }
        }


#if UNITY_ANDROID
        public AndroidJavaObject ToAjo()
        {
            var inviteContentBuilderAJO = new AndroidJavaObject("im.getsocial.sdk.invites.InviteContent$Builder");

            if (Subject != null)
            {
                inviteContentBuilderAJO.CallAJO("withSubject", Subject);
            }
            if (ImageUrl != null)
            {
                inviteContentBuilderAJO.CallAJO("withImageUrl", ImageUrl);
            }
            if (Image != null)
            {
                inviteContentBuilderAJO.CallAJO("withImage", Image.ToAjoBitmap());
            }
            if (Text != null)
            {
                inviteContentBuilderAJO.CallAJO("withText", Text);
            }
            if (Video != null)
            {
                inviteContentBuilderAJO.CallAJO("withVideo", Video);
            }
            return inviteContentBuilderAJO.CallAJO("build");
        }
#elif UNITY_IOS
        public string ToJson()
        {
            var jsonDic = new Dictionary<string, object>
            {
                {"Subject", Subject},
                {"Text", Text},
                {"ImageUrl", ImageUrl},
                {"Image", Image.TextureToBase64()},
                {"GifUrl", GifUrl},
                {"VideoUrl", VideoUrl},
                {"Video", Video.ByteArrayToBase64()}
            };
            return GSJson.Serialize(jsonDic);
        }
#endif
    }
}
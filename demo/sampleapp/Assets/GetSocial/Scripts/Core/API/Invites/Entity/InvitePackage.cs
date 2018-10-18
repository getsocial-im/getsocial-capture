using System;
using UnityEngine;

#if UNITY_IOS
using System.Collections.Generic;
using GetSocialSdk.MiniJSON;
#endif

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Invite package containing the invite data.
    /// </summary>
    public sealed class InvitePackage : IConvertableFromNative<InvitePackage>
    {
        /// <summary>
        /// Gets the invite subject.
        /// </summary>
        /// <value>The invite subject.</value>
        public string Subject { get; private set; }

        /// <summary>
        /// Gets the invite text.
        /// </summary>
        /// <value>The invite text.</value>
        public string Text { get; private set; }

        /// <summary>
        /// Gets the name of the user that sent the invite.
        /// </summary>
        /// <value>The name of the user that sent the invite.</value>
        public string UserName { get; private set; }

        /// <summary>
        /// Gets the referral data URL.
        /// </summary>
        /// <value>The referral data URL.</value>
        public string ReferralDataUrl { get; private set; }

        /// <summary>
        /// Gets the image of the invite.
        /// </summary>
        /// <value>The invite image.</value>
        public Texture2D Image { get; private set; }

        /// <summary>
        /// Gets the url of the invite image.
        /// </summary>
        /// <value>The url of invite image.</value>
        public string ImageUrl { get; private set; }

        public InvitePackage()
        {
            
        }
        
        internal InvitePackage(string subject, string text, string userName, string referralDataUrl, Texture2D image, string imageUrl)
        {
            Subject = subject;
            Text = text;
            UserName = userName;
            ReferralDataUrl = referralDataUrl;
            Image = image;
            ImageUrl = imageUrl;
        }

        public override string ToString()
        {
            return string.Format(
                "[InvitePackage: Subject={0}, Text={1}, UserName={2}, HasImage={3}, ImageUrl={4}, ReferralDataUrl={5}]",
                Subject, Text, UserName, Image != null, ImageUrl, ReferralDataUrl);
        }

        private bool Equals(InvitePackage other)
        {
            return string.Equals(Subject, other.Subject) && string.Equals(Text, other.Text) && string.Equals(UserName, other.UserName) && string.Equals(ReferralDataUrl, other.ReferralDataUrl) && Image.Texture2DEquals(other.Image) && string.Equals(ImageUrl, other.ImageUrl);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is InvitePackage && Equals((InvitePackage) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Subject != null ? Subject.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Text != null ? Text.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (UserName != null ? UserName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ReferralDataUrl != null ? ReferralDataUrl.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Image != null ? Image.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ImageUrl != null ? ImageUrl.GetHashCode() : 0);
                return hashCode;
            }
        }

#if UNITY_ANDROID
        public InvitePackage ParseFromAJO(AndroidJavaObject ajo)
        {
            JniUtils.CheckIfClassIsCorrect(ajo, "InvitePackage");

            using (ajo)
            {
                Subject = ajo.CallStr("getSubject");
                Text = ajo.CallStr("getText");
                UserName = ajo.CallStr("getUserName");
                ReferralDataUrl = ajo.CallStr("getReferralUrl");
                Image = ajo.CallAJO("getImage").FromAndroidBitmap();
                ImageUrl = ajo.CallStr("getImageUrl");
            }
            return this;
        }
#elif UNITY_IOS
        public InvitePackage ParseFromJson(Dictionary<string, object> json)
        {
            Subject = json["Subject"] as string;
            Text = json["Text"] as string;
            UserName = json["UserName"] as string;
            ReferralDataUrl = json["ReferralDataUrl"] as string;
            Image = (json["Image"] as string).FromBase64();
            ImageUrl = json["ImageUrl"] as string;
            return this;
        }
#endif
    }
}
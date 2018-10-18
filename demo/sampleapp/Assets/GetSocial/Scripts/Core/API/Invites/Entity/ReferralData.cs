using System;

#if UNITY_ANDROID
using UnityEngine;
#endif

#if UNITY_IOS
using System.Collections.Generic;
#endif

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Referral data of the smart invite.
    /// </summary>
    public sealed class ReferralData : IConvertableFromNative<ReferralData>
    {
        /// <summary>
        /// The unique Smart Invite link token. There is unique association between
        /// token and attached referral data.
        /// </summary>
        public string Token { get; private set; }

        /// <summary>
        /// The referrer user identifier.
        /// </summary>
        public string ReferrerUserId { get; private set; }

        /// <summary>
        /// The id of the channel where Smart Link was shared.
        /// </summary>
        public string ReferrerChannelId { get; private set; }

        /// <summary>
        /// Returns true if the app is installed for the first time on this device. False otherwise.
        /// </summary>
        public bool IsFirstMatch { get; private set; }
        
        /// <summary>
        ///  If GetSocial is able to retrieve extra meta information (e.g. from Google Play, Facebook or depplink) we provide 100% guarantee
        ///  that received data corresponds to the user. In other cases we use fingerprinting to find a best match.
        /// </summary>
        /// <value>true if GetSocial can give 100% guarantee that received referral data corresponds to the user, false in case of the best match.</value>
        public bool IsGuaranteedMatch { get; private set; }

        /// <summary>
        /// Returns true if the app is reinstalled on this device. False otherwise.
        /// </summary>
        public bool IsReinstall { get; private set; }

        /// <summary>
        /// Returns true if the app is opened for this link the first time on this device. False otherwise.
        /// </summary>
        public bool IsFirstMatchLink { get; private set; }
        
        /// <summary>
        /// Gets the custom referral data with the parameter overrides from the Smart Link.
        /// </summary>
        /// <value>The custom referral data.</value>
        /// Deprecated, use <see cref="LinkParams"/> instead.
        [Obsolete("Deprecated, use LinkParams instead.")]
#pragma warning disable 0618
        public CustomReferralData CustomReferralData { get; private set; }
#pragma warning restore 0618

        /// <summary>
        /// Gets the overriden link parameters assigned to the Smart Link.
        /// </summary>
        /// <value>The custom link parameters.</value>
        public LinkParams LinkParams { get ; private set; }

        /// <summary>
        /// Gets the original custom referral data. Overrides from the Smart Link are ignored.
        /// </summary>
        /// <value>The original custom referral data.</value>
        /// Deprecated, use <see cref="OriginalLinkParams"/> instead.
        [Obsolete("Deprecated, use OriginalLinkParams instead.")]
#pragma warning disable 0618
        public CustomReferralData OriginalCustomReferralData { get; private set; }
#pragma warning restore 0618

        /// <summary>
        /// Gets the original link parameters. Overrides from the Smart Link are ignored.
        /// </summary>
        /// <value>The original link parameters.</value>
        public LinkParams OriginalLinkParams { get; private set; }

        public ReferralData()
        {
        }

#pragma warning disable 0618
        internal ReferralData(string token, string referrerUserId, string referrerChannelId, bool isFirstMatch, bool isGuaranteedMatch, bool isReinstall, bool isFirstMatchLink, CustomReferralData customReferralData, LinkParams linkParams, CustomReferralData originalCustomReferralData, LinkParams originalLinkParams)
        {
            Token = token;
            ReferrerUserId = referrerUserId;
            ReferrerChannelId = referrerChannelId;
            IsFirstMatch = isFirstMatch;
            IsGuaranteedMatch = isGuaranteedMatch;
            IsReinstall = isReinstall;
            IsFirstMatchLink = isFirstMatchLink;
            CustomReferralData = customReferralData;
            OriginalCustomReferralData = originalCustomReferralData;
            LinkParams = linkParams;
            OriginalLinkParams = originalLinkParams;
        }
        
        private bool Equals(ReferralData other)
        {
            return string.Equals(Token, other.Token) && string.Equals(ReferrerUserId, other.ReferrerUserId) && string.Equals(ReferrerChannelId, other.ReferrerChannelId) && IsFirstMatch == other.IsFirstMatch && IsGuaranteedMatch == other.IsGuaranteedMatch && IsReinstall == other.IsReinstall && IsFirstMatchLink == other.IsFirstMatchLink && Equals(CustomReferralData, other.CustomReferralData) && Equals(LinkParams, other.LinkParams) && Equals(OriginalCustomReferralData, other.OriginalCustomReferralData) && Equals(OriginalLinkParams, other.OriginalLinkParams);
        }
        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is ReferralData && Equals((ReferralData) obj);
        }
#pragma warning restore 0618

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Token != null ? Token.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ReferrerUserId != null ? ReferrerUserId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ReferrerChannelId != null ? ReferrerChannelId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsFirstMatch.GetHashCode();
                hashCode = (hashCode * 397) ^ IsGuaranteedMatch.GetHashCode();
                hashCode = (hashCode * 397) ^ IsReinstall.GetHashCode();
                hashCode = (hashCode * 397) ^ IsFirstMatchLink.GetHashCode();
                
#pragma warning disable 0618
                hashCode = (hashCode * 397) ^ (CustomReferralData != null ? CustomReferralData.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (OriginalCustomReferralData != null ? OriginalCustomReferralData.GetHashCode() : 0);
#pragma warning restore 0618
                hashCode = (hashCode * 397) ^ (LinkParams != null ? LinkParams.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (OriginalLinkParams != null ? OriginalLinkParams.GetHashCode() : 0);
                return hashCode;
            }
        }

        public override string ToString()
        {
            return string.Format("[ReferralData: Token: {0}, ReferrerUserId={1}, ReferrerChannelId={2}, IsFirstMatch={3}, IsGuaranteedMatch={4}, LinkParams={5}, " +
                                 ", OriginalLinkParams={6}]",
                Token, ReferrerUserId, ReferrerChannelId, IsFirstMatch, IsGuaranteedMatch, LinkParams.ToDebugString(), OriginalLinkParams.ToDebugString());
        }

#if UNITY_ANDROID
        public ReferralData ParseFromAJO(AndroidJavaObject ajo)
        {
            if (ajo.IsJavaNull())
            {
                return null;
            }

            using (ajo)
            {
                Token = ajo.CallStr("getToken");
                ReferrerUserId = ajo.CallStr("getReferrerUserId");
                ReferrerChannelId = ajo.CallStr("getReferrerChannelId");
                IsFirstMatch = ajo.CallBool("isFirstMatch");
                IsGuaranteedMatch = ajo.CallBool("isGuaranteedMatch");
                IsReinstall = ajo.CallBool("isReinstall");
                IsFirstMatchLink = ajo.CallBool("isFirstMatchLink");
                var linkParamsDict = ajo.CallAJO("getLinkParams").FromJavaHashMap();
                LinkParams = new LinkParams(linkParamsDict);
                var originalLinkParamsDict = ajo.CallAJO("getOriginalLinkParams").FromJavaHashMap();
                OriginalLinkParams = new LinkParams(originalLinkParamsDict);
#pragma warning disable 0618
                CustomReferralData = new CustomReferralData(LinkParams);
                OriginalCustomReferralData = new CustomReferralData(OriginalLinkParams);
#pragma warning restore 0618
            }
            return this;
        }
#elif UNITY_IOS

        public ReferralData ParseFromJson(Dictionary<string, object> json)
        {
            Token = json["Token"] as string;
            ReferrerUserId = json["ReferrerUserId"] as string;
            ReferrerChannelId = json["ReferrerChannelId"] as string;
            IsFirstMatch = (bool) json["IsFirstMatch"];
            IsGuaranteedMatch = (bool) json["IsGuaranteedMatch"];
            IsReinstall = (bool) json["IsReinstall"];
            IsFirstMatchLink = (bool) json["IsFirstMatchLink"];
            LinkParams = new LinkParams(json["LinkParams"] as Dictionary<string, object>);
            OriginalLinkParams =
                new LinkParams(json["OriginalLinkParams"] as Dictionary<string, object>);
#pragma warning disable 0618
            CustomReferralData = new CustomReferralData(LinkParams);
            OriginalCustomReferralData = new CustomReferralData(OriginalLinkParams);
#pragma warning restore 0618
            return this;
        }
#endif
    }
}
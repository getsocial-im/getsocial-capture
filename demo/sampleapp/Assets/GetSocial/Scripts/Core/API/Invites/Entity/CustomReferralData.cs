using System;
using System.Collections.Generic;

#if UNITY_ANDROID
using UnityEngine;
#endif

#if UNITY_IOS
using GetSocialSdk.MiniJSON;
#endif

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Custom referral data attached to the invite.
    /// </summary>
    [Obsolete("Deprecated. Use LinkParams class instead.")]
    public sealed class CustomReferralData : Dictionary<string, string>, IConvertableFromNative<CustomReferralData>, IConvertableToNative
    {
        public CustomReferralData()
        {
        }

        public CustomReferralData(Dictionary<string, string> data) : base(data)
        {
        }

        public CustomReferralData(IDictionary<string, object> data)
        {
            if (data == null) return;
            foreach (var kv in data)
            {
                this[kv.Key] = kv.Value as string;
            }
        }

        public override string ToString()
        {
            return string.Format("[CustomReferralData: {0}]", this.ToDebugString());
        }

        private bool Equals(CustomReferralData other)
        {
            return this.DictionaryEquals(other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is CustomReferralData && Equals((CustomReferralData) obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
#if UNITY_ANDROID
        public AndroidJavaObject ToAjo()
        {
            return new AndroidJavaObject("im.getsocial.sdk.invites.CustomReferralData", this.ToJavaHashMap());
        }

        public CustomReferralData ParseFromAJO(AndroidJavaObject ajo)
        {
            using (ajo)
            {
                return new CustomReferralData(ajo.FromJavaHashMap());
            }
        }
#elif UNITY_IOS
        public string ToJson()
        {
            return GSJson.Serialize(this);
        }

        public CustomReferralData ParseFromJson(Dictionary<string, object> json)
        {
            return new CustomReferralData(json);
        }
#endif
    }
}
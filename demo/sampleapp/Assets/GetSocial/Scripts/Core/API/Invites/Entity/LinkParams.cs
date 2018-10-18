using System.Collections.Generic;
using UnityEngine;

#if UNITY_IOS
using GetSocialSdk.MiniJSON;
#endif

namespace GetSocialSdk.Core
{

    /// <summary>
    /// Link parameters attached to an invite link.
    /// </summary>
    public sealed class LinkParams : Dictionary<string, object>, IConvertableFromNative<LinkParams>, IConvertableToNative
    {
        public const string KeyCustomTitle = "$title";
        public const string KeyCustomDescription = "$description";
        public const string KeyCustomImage = "$image";
        public const string KeyCustomYouTubeVideo = "$youtube_video";

        public LinkParams()
        {
        }

        public LinkParams(Dictionary<string, string> data)
        {
            if (data == null) return;
            foreach (var kv in data)
            {
                this[kv.Key] = kv.Value;
            }
        }

        public LinkParams(Dictionary<string, object> data)
        {
            if (data == null) return;
            foreach (var kv in data)
            {
                this[kv.Key] = kv.Value;
            }
        }

        public override string ToString()
        {
            return string.Format("[LinkParams: {0}]", this.ToDebugString());
        }

        private bool Equals(LinkParams other)
        {
            return this.DictionaryEquals(other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is LinkParams && Equals((LinkParams) obj);
        }
        
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
#if UNITY_ANDROID
        public AndroidJavaObject ToAjo()
        {
            var retValue = new Dictionary<string, object>(this);
            if (retValue.ContainsKey(KeyCustomImage))
            {
                var image = retValue[KeyCustomImage];
                var texture2D = image as Texture2D;
                if (texture2D != null)
                {
                    retValue[KeyCustomImage] = texture2D.ToAjoBitmap();
                }
            }
            return new AndroidJavaObject("im.getsocial.sdk.invites.LinkParams", retValue.ToJavaHashMap());
        }

        public LinkParams ParseFromAJO(AndroidJavaObject ajo)
        {
            return new LinkParams(ajo.FromJavaHashMap());
        }
#elif UNITY_IOS
        public string ToJson()
        {
            var retValue = new Dictionary<string, object>(this);
            if (retValue.ContainsKey(KeyCustomImage))
            {
                var image = retValue[KeyCustomImage];
                var texture2D = image as Texture2D;
                if (texture2D != null)
                {
                    retValue[KeyCustomImage] = texture2D.TextureToBase64();
                }
            }
            return GSJson.Serialize(retValue);
        }

        public LinkParams ParseFromJson(Dictionary<string, object> json)
        {
            return new LinkParams(json);
        }
#endif
    } 
}
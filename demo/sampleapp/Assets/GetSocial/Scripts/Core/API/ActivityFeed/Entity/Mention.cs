using System;

#if UNITY_ANDROID
using UnityEngine;
#endif

#if UNITY_IOS
using System.Collections.Generic;
#endif

namespace GetSocialSdk.Core
{
    public class Mention : IConvertableFromNative<Mention>
    {
        public string UserId { get; private set; }
        public int StartIndex { get; private set; }
        public int EndIndex { get; private set; }
        
        /// <summary>
        /// Listed in <see cref="MentionTypes"/>.
        /// </summary>
        public string Type { get; private set; }
        
        public override string ToString()
        {
            return string.Format("{0} ({1}, {2}) - {3}", UserId, StartIndex, EndIndex, Type);
        }

        public Mention()
        {
            
        }

        internal Mention(string userId, int startIndex, int endIndex, string type)
        {
            UserId = userId;
            StartIndex = startIndex;
            EndIndex = endIndex;
            Type = type;
        }

        protected bool Equals(Mention other)
        {
            return string.Equals(UserId, other.UserId) && StartIndex == other.StartIndex && EndIndex == other.EndIndex && string.Equals(Type, other.Type);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Mention) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (UserId != null ? UserId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ StartIndex;
                hashCode = (hashCode * 397) ^ EndIndex;
                hashCode = (hashCode * 397) ^ (Type != null ? Type.GetHashCode() : 0);
                return hashCode;
            }
        }

#if UNITY_IOS
        public Mention ParseFromJson(Dictionary<string, object> json)
        {
            UserId = json["UserId"] as string;
            StartIndex = (int) (long) json["StartIndex"];
            EndIndex = (int) (long) json["EndIndex"];
            Type = json["Type"] as string;
            return this;
        }
#elif UNITY_ANDROID
        public Mention ParseFromAJO(AndroidJavaObject ajo)
        {
            UserId = ajo.CallStr("getUserId");
            StartIndex = ajo.CallInt("getStartIndex");
            EndIndex = ajo.CallInt("getEndIndex");
            Type = ajo.CallStr("getType");
            return this;
        }
#endif
    }
}
using System.Collections.Generic;
#if UNITY_ANDROID
using UnityEngine;
#endif

namespace GetSocialSdk.Core
{
    /// <summary>
    /// The author of <see cref="ActivityPost"/>.
    /// </summary>
    public sealed class PostAuthor : PublicUser, IConvertableFromNative<PostAuthor>
    {
        /// <summary>
        /// Gets a value indicating whether this user is verified.
        /// </summary>
        /// <value><c>true</c> if this user is verified; otherwise, <c>false</c>.</value>
        public bool IsVerified { get; private set; }
        
        public override string ToString()
        {
            return string.Format("[PostAuthor: Id={0}, DisplayName={1}, Identities={2}, IsVerified={3}]", Id, DisplayName, Identities.ToDebugString(), IsVerified);
        }

        public PostAuthor()
        {
            
        }

        internal PostAuthor(Dictionary<string, string> publicProperties, string id, string displayName, string avatarUrl, Dictionary<string, string> identities, bool isVerified) : base(publicProperties, id, displayName, avatarUrl, identities)
        {
            IsVerified = isVerified;
        }

        private bool Equals(PostAuthor other)
        {
            return base.Equals(other) && IsVerified == other.IsVerified;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is PostAuthor && Equals((PostAuthor) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ IsVerified.GetHashCode();
            }
        }

#if UNITY_ANDROID
        public new PostAuthor ParseFromAJO(AndroidJavaObject ajo)
        {
            using (ajo)
            {
                base.ParseFromAJO(ajo);
                IsVerified = ajo.CallBool("isVerified");
            }
            return this;
        }
#elif UNITY_IOS
        public new PostAuthor ParseFromJson(Dictionary<string, object> jsonDic)
        {
            base.ParseFromJson(jsonDic);
            IsVerified = (bool) jsonDic["IsVerified"];
            return this;
        }

#endif
    }
}
using System;
using System.Collections.Generic;

#if UNITY_ANDROID
using UnityEngine;
#endif

namespace GetSocialSdk.Core
{
    /// <summary>
    /// User that is received in the case of conflict when adding auth identity.
    /// </summary>
    public class ConflictUser : PublicUser, IConvertableFromNative<ConflictUser>
    {
        internal ConflictUser()
        {
        }
        
        internal ConflictUser(Dictionary<string, string> publicProperties, string id, string displayName, string avatarUrl, Dictionary<string, string> identities) : base(publicProperties, id, displayName, avatarUrl, identities)
        {
        }
#if UNITY_ANDROID
        public new ConflictUser ParseFromAJO(AndroidJavaObject ajo)
        {
            using (ajo)
            {
                base.ParseFromAJO(ajo);
            }
            return this;
        }
#elif UNITY_IOS

        public new ConflictUser ParseFromJson(Dictionary<string, object> json)
        {
            base.ParseFromJson(json);
            return this;
        }
#endif
    }
}
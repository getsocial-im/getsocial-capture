using System;
using System.Collections.Generic;

#if UNITY_ANDROID
using UnityEngine;
#endif

namespace GetSocialSdk.Core
{
    /// <summary>
    /// Immutable properties for a public user.
    /// </summary>
    public class PublicUser : IConvertableFromNative<PublicUser>
    {
        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public string Id { get; protected set; }

        /// <summary>
        /// Gets the user display name.
        /// </summary>
        /// <value>The user display name.</value>
        public string DisplayName { get; protected set; }

        /// <summary>
        /// Gets the user avatar URL.
        /// </summary>
        /// <value>The user avatar URL.</value>
        public string AvatarUrl { get; protected set; }
        
        /// <summary>
        /// You can add or remove identities using <see cref="GetSocial.User.AddAuthIdentity"/> and <see cref="GetSocial.User.RemoveAuthIdentity"/>.
        /// The key(providerId) is the one you've passed as a first parameter to <see cref="AuthIdentity.CreateCustomIdentity"/>
        /// or <see cref="AuthIdentityProvider.Facebook"/> if you've created Facebook identity with <see cref="AuthIdentity.CreateFacebookIdentity"/>.
        /// Read more about identities in <see href="https://docs.getsocial.im/guides/user-management/android/managing-user-identities/">the documentation</see>.
        /// The value(userId) is the second parameter in <see cref="AuthIdentity.CreateCustomIdentity"/>
        /// or automatically obtained by GetSocial if you've used Facebook identity. 
        /// </summary>
        /// <value>
        /// All auth identities added to the user or an empty map if the sdk is in an illegal state.
        /// When receiving an empty dictionary please check the state of the sdk to determine whether there are no identities or there was an error.
        /// </value>
        public Dictionary<string, string> Identities { get; protected set; }

        /// <summary>
        /// Gets all public properties.
        /// Returns a copy of origin user properties.
        /// </summary>
        /// <value>User public properties</value>
        public Dictionary<string, string> AllPublicProperties
        {
            get { return new Dictionary<string, string>(_publicProperties); }
        }

#pragma warning disable 414, 649      
        private Dictionary<string, string> _publicProperties;
#pragma warning restore 414, 649

        public override string ToString()
        {
            return string.Format("[PublicUser: Id={0}, DisplayName={1}, Identities={2}, PublicProperties={3}]", Id, DisplayName, Identities.ToDebugString(), _publicProperties.ToDebugString());
        }

        public PublicUser()
        {
            
        }
        
        internal PublicUser(Dictionary<string, string> publicProperties, string id, string displayName, string avatarUrl, Dictionary<string, string> identities)
        {
            _publicProperties = publicProperties;
            Id = id;
            DisplayName = displayName;
            AvatarUrl = avatarUrl;
            Identities = identities;
        }

        protected bool Equals(PublicUser other)
        {
            return _publicProperties.DictionaryEquals(other._publicProperties) && string.Equals(Id, other.Id) &&
                   string.Equals(DisplayName, other.DisplayName) && string.Equals(AvatarUrl, other.AvatarUrl) &&
                   Identities.DictionaryEquals(other.Identities);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PublicUser) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (_publicProperties != null ? _publicProperties.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Id != null ? Id.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (DisplayName != null ? DisplayName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (AvatarUrl != null ? AvatarUrl.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Identities != null ? Identities.GetHashCode() : 0);
                return hashCode;
            }
        }

#if UNITY_ANDROID
        public PublicUser ParseFromAJO(AndroidJavaObject ajo)
        {
            // NOTE: Don't forget to call Dispose() in subclasses to avoid leaks!!
            Id = ajo.CallStr("getId");
            DisplayName = ajo.CallStr("getDisplayName");
            AvatarUrl = ajo.CallStr("getAvatarUrl");
            Identities = ajo.CallAJO("getIdentities").FromJavaHashMap();
            _publicProperties = ajo.CallAJO("getAllPublicProperties").FromJavaHashMap();
            return this;
        }

#elif UNITY_IOS
        public PublicUser ParseFromJson(Dictionary<string, object> json)
        {
            Id = (string) json["Id"];
            DisplayName = (string) json["DisplayName"];
            AvatarUrl = (string) json["AvatarUrl"];

            var identitiesDictionary = json["Identities"] as Dictionary<string, object>;
            Identities = identitiesDictionary.ToStrStrDict();

            var publicProperties = json["PublicProperties"] as Dictionary<string, object>;
            _publicProperties = publicProperties.ToStrStrDict();

            return this;
        }
#endif
    }
}
using System;

#if UNITY_ANDROID
using UnityEngine;
#endif

#if UNITY_IOS
using System.Collections.Generic;
using GetSocialSdk.MiniJSON;
#endif

namespace GetSocialSdk.Core
{
    /// <summary>
    /// This class is representation of User Auth Identity, that is used by GetSocial framework to identify user
    /// and to manage his accounts.
    /// </summary>
    public class AuthIdentity : IConvertableToNative
    {
#pragma warning disable 414
        internal readonly string ProviderId;
        internal readonly string ProviderUserId;
        internal readonly string AccessToken;
#pragma warning restore 414
        
        private AuthIdentity(string providerName, string userId, string accessToken)
        {
            ProviderId = providerName;
            ProviderUserId = userId;
            AccessToken = accessToken;
        }

        /// <summary>
        /// Create a Facebook identity with specified access token.
        /// </summary>
        /// <param name="accessToken">Token of Facebook user returned from FB SDK.</param>
        /// <value>The instance of AuthIdentity for Facebook user with specified access token</value>
        public static AuthIdentity CreateFacebookIdentity(string accessToken)
        {
            return CreateCustomIdentity(AuthIdentityProvider.Facebook, null, accessToken);
        }

        /// <summary>
        /// Create custom identity.
        /// </summary>
        /// <param name="providerName">Your custom provider name.</param>
        /// <param name="userId">Unique user identifier for your custom provider.</param>
        /// <param name="accessToken">Password of the user for your custom provider.
        /// It's a string, provided by the developer and it will be
        /// required by the GetSocial SDK to validate any future
        /// intent to add this same identity to another user.</param>
        /// <value>The instance of AuthIdentity for your custom provider</value>
        public static AuthIdentity CreateCustomIdentity(string providerName, string userId, string accessToken)
        {
            return new AuthIdentity(providerName, userId, accessToken);
        }

#if UNITY_ANDROID
        public AndroidJavaObject ToAjo()
        {
            var identityClass = new AndroidJavaClass("im.getsocial.sdk.usermanagement.AuthIdentity");
            return identityClass.CallStaticAJO("createCustomIdentity", ProviderId, ProviderUserId, AccessToken);
        }
#elif UNITY_IOS
        public string ToJson()
        {
            var jsonDic = new Dictionary<string, object>
            {
                {"ProviderId", ProviderId},
                {"ProviderUserId", ProviderUserId},
                {"AccessToken", AccessToken}
            };
            return GSJson.Serialize(jsonDic);
        }
#endif
    }
}
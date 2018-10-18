#if UNITY_ANDROID
using System;
using UnityEngine;
using System.Diagnostics.CodeAnalysis;

namespace GetSocialSdk.Core
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    internal class FetchReferralDataCallbackProxy : JavaInterfaceProxy
    {
        private readonly Action<ReferralData> _onSuccess;
        private readonly Action<GetSocialError> _onFailure;

        public FetchReferralDataCallbackProxy(Action<ReferralData> onSuccess, Action<GetSocialError> onFailure)
            : base("im.getsocial.sdk.invites.FetchReferralDataCallback")
        {
            _onSuccess = onSuccess;
            _onFailure = onFailure;
        }

        void onSuccess(AndroidJavaObject referralDataAJO)
        {
            var referralData = new ReferralData().ParseFromAJO(referralDataAJO);

            GetSocialDebugLogger.D("On success: " + referralData);

            HandleValue(referralData, _onSuccess);
        }

        void onFailure(AndroidJavaObject throwable)
        {
            HandleError(throwable, _onFailure);
        }
    }
}

#endif
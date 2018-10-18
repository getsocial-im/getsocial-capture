#if UNITY_ANDROID
using UnityEngine;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GetSocialSdk.Core
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    internal class BoolCallbackProxy : JavaInterfaceProxy
    {
        readonly Action<bool> _onSuccess;
        readonly Action<GetSocialError> _onFailure;

        public BoolCallbackProxy(Action<bool> onSuccess, Action<GetSocialError> onFailure)
            : base("im.getsocial.sdk.Callback")
        {
            _onSuccess = onSuccess;
            _onFailure = onFailure;
        }

        void onSuccess(bool value)
        {
            HandleValue(value, _onSuccess);
        }

        void onFailure(AndroidJavaObject throwable)
        {
            HandleError(throwable, _onFailure);
        }
    }
}

#endif
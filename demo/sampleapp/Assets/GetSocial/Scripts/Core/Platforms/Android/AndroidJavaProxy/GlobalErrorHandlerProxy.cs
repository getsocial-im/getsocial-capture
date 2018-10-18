#if UNITY_ANDROID
using UnityEngine;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GetSocialSdk.Core
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    internal class GlobalErrorListenerProxy : JavaInterfaceProxy
    {
        readonly Action<GetSocialError> _onError;

        public GlobalErrorListenerProxy(Action<GetSocialError> onError)
            : base("im.getsocial.sdk.GlobalErrorListener")
        {
            _onError = onError;
        }

        void onError(AndroidJavaObject throwable)
        {
            HandleError(throwable, _onError);
        }
    }
}

#endif
#if UNITY_ANDROID
using UnityEngine;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GetSocialSdk.Core
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    internal class CompletionCallback : JavaInterfaceProxy
    {
        readonly Action _onSuccess;
        readonly Action<GetSocialError> _onFailure;

        public CompletionCallback(Action onSuccess, Action<GetSocialError> onFailure)
            : base("im.getsocial.sdk.CompletionCallback")
        {
            _onSuccess = onSuccess;
            _onFailure = onFailure;
        }

        void onSuccess()
        {
            GetSocialDebugLogger.D("CompletionCallback success");
            ExecuteOnMainThread(_onSuccess);
        }

        void onFailure(AndroidJavaObject throwable)
        {
            HandleError(throwable, _onFailure);
        }
    }
}

#endif
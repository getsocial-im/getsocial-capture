#if UNITY_ANDROID
using UnityEngine;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GetSocialSdk.Core
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    internal class CallbackProxy<T> : JavaInterfaceProxy where T : IConvertableFromNative<T>, new()
    {
        readonly Action<T> _onSuccess;
        readonly Action<GetSocialError> _onFailure;

        public CallbackProxy(Action<T> onSuccess, Action<GetSocialError> onFailure)
            : base("im.getsocial.sdk.Callback")
        {
            _onSuccess = onSuccess;
            _onFailure = onFailure;
        }

        void onSuccess(AndroidJavaObject resultAJO)
        {
            var res = new T().ParseFromAJO(resultAJO);

            GetSocialDebugLogger.D("On success: " + res);

            HandleValue(res, _onSuccess);
        }

        void onFailure(AndroidJavaObject throwable)
        {
            HandleError(throwable, _onFailure);
        }
    }
}

#endif
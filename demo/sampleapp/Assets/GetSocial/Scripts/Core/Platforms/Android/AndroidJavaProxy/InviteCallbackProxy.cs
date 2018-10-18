#if UNITY_ANDROID
using UnityEngine;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GetSocialSdk.Core
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    internal class InviteCallbackProxy : JavaInterfaceProxy
    {
        readonly Action _onComplete;
        readonly Action _onCancel;
        readonly Action<GetSocialError> _onFailure;

        public InviteCallbackProxy(Action onComplete, Action onCancel, Action<GetSocialError> onFailure)
            : base("im.getsocial.sdk.invites.InviteCallback")
        {
            _onComplete = onComplete;
            _onCancel = onCancel;
            _onFailure = onFailure;
        }

        void onComplete()
        {
            Debug.Log("Complete");
            ExecuteOnMainThread(_onComplete);
        }

        void onCancel()
        {
            Debug.Log("Cancel");
            ExecuteOnMainThread(_onCancel);
        }

        void onError(AndroidJavaObject throwable)
        {
            HandleError(throwable, _onFailure);
        }
    }
}

#endif
#if UNITY_ANDROID && USE_GETSOCIAL_UI
using System;
using System.Diagnostics.CodeAnalysis;
using GetSocialSdk.Core;
using UnityEngine;

namespace GetSocialSdk.Ui
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class InviteUiCallbackProxy : JavaInterfaceProxy
    {
        readonly Action<string> _onComplete;
        readonly Action<string> _onCancel;
        readonly Action<string, GetSocialError> _onFailure;

        public InviteUiCallbackProxy(Action<string> onComplete, Action<string> onCancel, Action<string, GetSocialError> onFailure)
            : base("im.getsocial.sdk.ui.invites.InviteUiCallback")
        {
            _onComplete = onComplete;
            _onCancel = onCancel;
            _onFailure = onFailure;
        }

        void onComplete(string channelId)
        {
            Debug.Log("Complete");
            ExecuteOnMainThread(() => _onComplete(channelId));
        }

        void onCancel(string channelId)
        {
            Debug.Log("Cancel");
            ExecuteOnMainThread(() => _onCancel(channelId));
        }

        void onError(string channelId, AndroidJavaObject throwable)
        {
            Debug.Log("Failure");
            ExecuteOnMainThread(() => _onFailure(channelId, throwable.ToGetSocialError()));
        }
    }
}
#endif

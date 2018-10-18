#if UNITY_ANDROID
using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace GetSocialSdk.Core
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    class InviteChannelPluginProxy : JavaInterfaceProxy
    {
        readonly InviteChannelPlugin _invitePlugin;

        public InviteChannelPluginProxy(InviteChannelPlugin invitePlugin)
            : base("im.getsocial.sdk.internal.unity.InviteChannelPluginAdapter$InviteChannelPluginInterface")
        {
            _invitePlugin = invitePlugin;
        }

        bool isAvailableForDevice(AndroidJavaObject inviteChannelAJO)
        {
            var inviteChannel = new InviteChannel().ParseFromAJO(inviteChannelAJO);

            return _invitePlugin.IsAvailableForDevice(inviteChannel);
        }

        void presentChannelInterface(
            AndroidJavaObject inviteChannelAJO,
            AndroidJavaObject invitePackageAJO,
            AndroidJavaObject callbackAJO)
        {
            ExecuteOnMainThread(() =>
            {
                var inviteChannel = new InviteChannel().ParseFromAJO(inviteChannelAJO);
                var invitePackage = new InvitePackage().ParseFromAJO(invitePackageAJO);

                Action onComplete = () => { callbackAJO.Call("onComplete"); };
                Action onCancel = () => { callbackAJO.Call("onCancel"); };
                Action<GetSocialError> onFailure = err =>
                {
                    callbackAJO.Call("onError", JniUtils.NewJavaThrowable(err.Message));
                };

                try
                {
                    _invitePlugin.PresentChannelInterface(inviteChannel, invitePackage, onComplete, onCancel,
                        onFailure);
                }
                catch (Exception e)
                {
                    onFailure(new GetSocialError(e.Message));
                }
            });
        }
    }
}

#endif
#if UNITY_ANDROID && USE_GETSOCIAL_UI
using System;
using System.Diagnostics.CodeAnalysis;
using GetSocialSdk.Core;
using UnityEngine;

namespace GetSocialSdk.Ui
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class AvatarClickListenerProxy : JavaInterfaceProxy
    {
        readonly Action<PublicUser> _avatarClickListener;

        public AvatarClickListenerProxy(Action<PublicUser> avatarClickListener)
            : base("im.getsocial.sdk.ui.AvatarClickListener")
        {
            _avatarClickListener = avatarClickListener;
        }

        void onAvatarClicked(AndroidJavaObject publicUserAjo)
        {
            Debug.Log(">>>>>>> XXXX");
            var publicUser = new PublicUser().ParseFromAJO(publicUserAjo);
            ExecuteOnMainThread(() => _avatarClickListener(publicUser));
        }
    }
}
#endif
#if UNITY_IOS && USE_GETSOCIAL_UI
using System;
using GetSocialSdk.Core;

namespace GetSocialSdk.Ui
{
    delegate void AvatarClickListenerDelegate(IntPtr avatarClickListenerPtr, string serializedPublicUser);

    public static class AvatarClickListenerCallback
    {
        [AOT.MonoPInvokeCallback(typeof(AvatarClickListenerDelegate))]
        public static void OnAvatarClicked(IntPtr onAvatarClickedPtr, string serializedPublicUser)
        {
            GetSocialDebugLogger.D(string.Format("OnAvatarClicked for user {0}", serializedPublicUser));

            if (onAvatarClickedPtr != IntPtr.Zero)
            {
                var publicUser = new PublicUser().ParseFromJson(serializedPublicUser.ToDict());
                onAvatarClickedPtr.Cast<Action<PublicUser>>().Invoke(publicUser);
            }
        }
    }
}
#endif

#if UNITY_IOS
using System;
using AOT;

namespace GetSocialSdk.Core
{
    public static class InviteChannelPluginCallbacks
    {
        public delegate bool IsAvailableForDeviceDelegate(IntPtr instancePtr, string inviteChannelJson);

        public delegate void PresentChannelInterfaceDelegate(
            IntPtr instancePtr, string inviteChannelJson, string invitePackageJson,
            IntPtr onCompletePtr, IntPtr onCancelPtr, IntPtr onFailurePtr);

        [AOT.MonoPInvokeCallback(typeof(IsAvailableForDeviceDelegate))]
        public static bool IsAvailableForDevice(IntPtr instancePtr, string inviteChannelJson)
        {
            var channel = new InviteChannel().ParseFromJson(inviteChannelJson.ToDict());
            return instancePtr.Cast<InviteChannelPlugin>().IsAvailableForDevice(channel);
        }

        [AOT.MonoPInvokeCallback(typeof(PresentChannelInterfaceDelegate))]
        public static void PresentChannelInterface(IntPtr instancePtr, string inviteChannelJson,
            string invitePackageJson,
            IntPtr onCompletePtr, IntPtr onCancelPtr, IntPtr onFailurePtr)
        {
            var channel = new InviteChannel().ParseFromJson(inviteChannelJson.ToDict());
            var package = new InvitePackage().ParseFromJson(invitePackageJson.ToDict());
            var onFailure = new Action<GetSocialError>(exception =>
            {
                GetSocialNativeBridgeIOS._gs_executeInviteFailedCallback(onFailurePtr, exception.ErrorCode,
                    exception.Message);
            });
            var plugin = instancePtr.Cast<InviteChannelPlugin>();
            try
            {
                plugin.PresentChannelInterface(channel, package,
                    () => { GetSocialNativeBridgeIOS._gs_executeInviteSuccessCallback(onCompletePtr); },
                    () => { GetSocialNativeBridgeIOS._gs_executeInviteCancelledCallback(onCancelPtr); },
                    onFailure);
            }
            catch (Exception e)
            {
                onFailure(new GetSocialError(e.Message));
            }
        }
    }
}

#endif
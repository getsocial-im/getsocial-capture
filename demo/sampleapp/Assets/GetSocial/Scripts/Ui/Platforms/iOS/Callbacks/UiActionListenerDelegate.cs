#if UNITY_IOS && USE_GETSOCIAL_UI
using System;
using System.Runtime.InteropServices;
using GetSocialSdk.Core;

namespace GetSocialSdk.Ui
{
    delegate void UiActionListenerDelegate(IntPtr ptr, int actionType);

    public static class UiActionListenerCallback
    {
        [AOT.MonoPInvokeCallback(typeof(UiActionListenerDelegate))]
        public static void OnUiAction(IntPtr ptr, int actionTypeOrdinal)
        {
            if (ptr != IntPtr.Zero)
            {
                UiAction actionType = (UiAction) actionTypeOrdinal;
                ptr.Cast<UiActionListener>()(actionType, () =>
                {
                    _gs_doPendingAction();
                });
            }

        }

        [DllImport("__Internal")]
        static extern void _gs_doPendingAction();
    }
}
#endif

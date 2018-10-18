#if UNITY_ANDROID && USE_GETSOCIAL_UI
using System;
using System.Diagnostics.CodeAnalysis;
using GetSocialSdk.Core;
using UnityEngine;

namespace GetSocialSdk.Ui
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class UiActionListenerProxy : JavaInterfaceProxy
    {
        private UiActionListener _uiActionListener;

        public UiActionListenerProxy(UiActionListener uiActionListener) : base("im.getsocial.sdk.ui.UiActionListener")
        {
            _uiActionListener = uiActionListener;
        }

        void onUiAction(AndroidJavaObject actionType, AndroidJavaObject pendingAction)
        {
            _uiActionListener(toUIActionEnum(actionType), () => pendingAction.Call("proceed"));
        }

        private UiAction toUIActionEnum(AndroidJavaObject actionType)
        {
            return (UiAction) actionType.CallInt("ordinal");
        }
    }
}
#endif
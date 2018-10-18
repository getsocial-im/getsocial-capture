#if UNITY_ANDROID && USE_GETSOCIAL_UI
using System;
using System.Diagnostics.CodeAnalysis;
using GetSocialSdk.Core;

namespace GetSocialSdk.Ui
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class ViewStateListener : JavaInterfaceProxy
    {
        private Action _onOpen, _onClose;

        public ViewStateListener(Action onOpen, Action onClose) : base("im.getsocial.sdk.ui.ViewStateListener")
        {
            _onOpen = onOpen;
            _onClose = onClose;
        }

        void onOpen()
        {
            if (_onOpen != null)
            {
                _onOpen();
            }
        }

        void onClose()
        {
            if (_onClose != null)
            {
                _onClose();
            }
        }
    }
}
#endif
#if UNITY_ANDROID && USE_GETSOCIAL_UI
using System;
using System.Diagnostics.CodeAnalysis;
using GetSocialSdk.Core;

namespace GetSocialSdk.Ui
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class TagClickListenerProxy : JavaInterfaceProxy
    {
        readonly Action<string> _tagClickListener;

        public TagClickListenerProxy(Action<string> tagClickListener) : base("im.getsocial.sdk.ui.TagClickListener")
        {
            _tagClickListener = tagClickListener;
        }

        void onTagClicked(string tag)
        {
            ExecuteOnMainThread(() => _tagClickListener(tag));
        }
    }
}
#endif
#if UNITY_ANDROID && USE_GETSOCIAL_UI
using System;
using System.Diagnostics.CodeAnalysis;
using GetSocialSdk.Core;

namespace GetSocialSdk.Ui
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class MentionClickListenerProxy : JavaInterfaceProxy
    {
        readonly Action<string> _onMentionClickListener;

        public MentionClickListenerProxy(Action<string> onMentionClickListener) : base("im.getsocial.sdk.ui.MentionClickListener")
        {
            _onMentionClickListener = onMentionClickListener;
        }

        void onMentionClicked(string userId)
        {
            ExecuteOnMainThread(() => _onMentionClickListener(userId));
        }
    }
}
#endif
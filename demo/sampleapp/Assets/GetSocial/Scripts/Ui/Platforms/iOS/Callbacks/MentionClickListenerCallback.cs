#if UNITY_IOS && USE_GETSOCIAL_UI
using System;
using GetSocialSdk.Core;

namespace GetSocialSdk.Ui
{
    delegate void MentionClickListenerDelegate(IntPtr mentionClickListenerPtr, string userId);
    
    public class MentionClickListenerCallback
    {
        [AOT.MonoPInvokeCallback(typeof(MentionClickListenerDelegate))]
        public static void OnMentionClicled(IntPtr mentionClickListenerPtr, string mention)
        {
            GetSocialDebugLogger.D(string.Format("OnMentionClicled for user {0}", mention));

            if (mentionClickListenerPtr != IntPtr.Zero)
            {
                mentionClickListenerPtr.Cast<Action<string>>().Invoke(mention);
            }
        }
    }
}

#endif
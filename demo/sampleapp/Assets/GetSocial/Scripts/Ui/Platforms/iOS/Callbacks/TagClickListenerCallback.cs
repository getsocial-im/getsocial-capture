#if UNITY_IOS && USE_GETSOCIAL_UI
using System;
using GetSocialSdk.Core;

namespace GetSocialSdk.Ui
{
    delegate void TagClickListenerDelegate(IntPtr tagClickListenerPtr, string tag);
    
    public class TagClickListenerCallback
    {
        [AOT.MonoPInvokeCallback(typeof(TagClickListenerDelegate))]
        public static void OnTagClicked(IntPtr tagClickListenerPtr, string tag)
        {
            GetSocialDebugLogger.D(string.Format("OnTagClicked for tag {0}", tag));

            if (tagClickListenerPtr != IntPtr.Zero)
            {
                tagClickListenerPtr.Cast<Action<string>>().Invoke(tag);
            }
        }
    }
}

#endif
#if UNITY_ANDROID && USE_GETSOCIAL_UI
using System;
using System.Diagnostics.CodeAnalysis;
using GetSocialSdk.Core;
using UnityEngine;

namespace GetSocialSdk.Ui
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public class ActionButtonListenerProxy : JavaInterfaceProxy
    {
        readonly Action<string, ActivityPost> _onButtonClicked;

        public ActionButtonListenerProxy(Action<string, ActivityPost> onButtonClicked)
            : base("im.getsocial.sdk.ui.activities.ActionButtonListener")
        {
            _onButtonClicked = onButtonClicked;
        }

        void onButtonClicked(string action, AndroidJavaObject post)
        {
            Debug.Log(">>>>>>> XXXX");
            var activityPost = new ActivityPost().ParseFromAJO(post);
            ExecuteOnMainThread(() => _onButtonClicked(action, activityPost));
        }
    }
}

#endif
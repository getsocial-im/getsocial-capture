#if UNITY_ANDROID
using UnityEngine;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GetSocialSdk.Core
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    internal class OnUserChangedListenerProxy : JavaInterfaceProxy
    {
        readonly Action _onUserChanged;

        public OnUserChangedListenerProxy(Action onUserChanged)
            : base("im.getsocial.sdk.usermanagement.OnUserChangedListener")
        {
            _onUserChanged = onUserChanged;
        }

        void onUserChanged()
        {
            ExecuteOnMainThread(_onUserChanged);
        }
    }
}
#endif
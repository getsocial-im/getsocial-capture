#if UNITY_ANDROID
using UnityEngine;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GetSocialSdk.Core
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    internal class NotificationListenerProxy : JavaInterfaceProxy
    {
        readonly Func<Notification, bool, bool> _onNotification;

        public NotificationListenerProxy(Func<Notification, bool, bool> onNotification)
            : base("im.getsocial.sdk.pushnotifications.NotificationListener")
        {
            _onNotification = onNotification;
        }

        bool onNotificationReceived(AndroidJavaObject ajo, bool wasClicked)
        {
            return _onNotification != null && _onNotification(new Notification().ParseFromAJO(ajo), wasClicked);
        }
    }
}
#endif
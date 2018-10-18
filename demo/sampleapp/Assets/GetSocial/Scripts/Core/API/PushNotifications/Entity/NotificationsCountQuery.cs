using System;
using UnityEngine;

using System.Collections.Generic;
using GetSocialSdk.MiniJSON;

namespace GetSocialSdk.Core
{
    public sealed class NotificationsCountQuery : IConvertableToNative
    {
        private static readonly Notification.NotificationTypes[] AllTypes = new Notification.NotificationTypes[0];

#pragma warning disable 414        
        private readonly bool? _isRead;
        private Notification.NotificationTypes[] _types = AllTypes;
#pragma warning restore 414
        private NotificationsCountQuery(bool? isRead)
        {
            _isRead = isRead;
        }

        public static NotificationsCountQuery ReadAndUnread() 
        {
            return new NotificationsCountQuery(null);
        }

        public static NotificationsCountQuery Read() 
        {
            return new NotificationsCountQuery(true);
        }

        public static NotificationsCountQuery Unread() 
        {
            return new NotificationsCountQuery(false);
        }

        public NotificationsCountQuery OfAllTypes()
        {
            _types = AllTypes;
            return this;
        }
        public NotificationsCountQuery OfTypes(params Notification.NotificationTypes[] types)
        {
            _types = types;
            return this;
        }

#if UNITY_ANDROID
        public AndroidJavaObject ToAjo()
        {
            var queryClass =
                new AndroidJavaClass("im.getsocial.sdk.pushnotifications.NotificationsCountQuery");
            var query = _isRead.HasValue
                ? queryClass.CallStaticAJO(_isRead.Value ? "read" : "unread")
                : queryClass.CallStaticAJO("readAndUnread");

            if (_types.Length > 0)
            {
                var types = Array.ConvertAll(_types, type => (int) type);
                query.CallAJO("ofTypes", types );    
            }
            
            return query;
        }


#elif UNITY_IOS
        public string ToJson()
        {
            var json = new Dictionary<string, object>
            {
                {"IsRead", _isRead},
                {"Types", Array.ConvertAll(_types, type => (int) type)}
            };
            return GSJson.Serialize(json);
        }
#endif
    }
}
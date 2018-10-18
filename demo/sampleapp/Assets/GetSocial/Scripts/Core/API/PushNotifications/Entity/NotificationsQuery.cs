using System;
using System.Collections.Generic;
using System.Linq;
using GetSocialSdk.MiniJSON;
using UnityEngine;

namespace GetSocialSdk.Core
{
    public sealed class NotificationsQuery : IConvertableToNative
    {
        /// <summary>
        /// Set of filtering options for <see cref="WithFilter(Filter, string)"/> method
        /// </summary>
        public enum Filter
        {
            /// <summary>
            /// No filter will be applied to the query.
            /// </summary>
            NoFilter,

            /// <summary>
            /// Query will provide all older notifications.
            /// </summary>
            Older,

            /// <summary>
            /// Query will provide all newer notifications.
            /// </summary>
            Newer
        }
        
        private static readonly Notification.NotificationTypes[] AllTypes = new Notification.NotificationTypes[0];
        
#pragma warning disable 414        
        private readonly bool? _isRead;
        private Notification.NotificationTypes[] _types = AllTypes;
        private Filter _filter = Filter.NoFilter;
        private string _notificationId;
        private int _limit;
#pragma warning restore 414

        private NotificationsQuery(bool? isRead)
        {
            _isRead = isRead;
        }

        public static NotificationsQuery ReadAndUnread() 
        {
            return new NotificationsQuery(null);
        }

        public static NotificationsQuery Read() 
        {
            return new NotificationsQuery(true);
        }

        public static NotificationsQuery Unread() 
        {
            return new NotificationsQuery(false);
        }

        public NotificationsQuery OfAllTypes() {
            _types = AllTypes;
            return this;
        }

        public NotificationsQuery OfTypes(params Notification.NotificationTypes[] types)
        {
            _types = types;
            return this;
        }

        public NotificationsQuery WithLimit(int limit)
        {
            _limit = limit;
            return this;
        }

        public NotificationsQuery WithFilter(Filter filter, string notificationId)
        {
            _filter = filter;
            _notificationId = notificationId;
            return this;
        }

#if UNITY_ANDROID
        public AndroidJavaObject ToAjo()
        {
            var queryClass =
                new AndroidJavaClass("im.getsocial.sdk.pushnotifications.NotificationsQuery");
            var query = _isRead.HasValue
                ? queryClass.CallStaticAJO(_isRead.Value ? "read" : "unread")
                : queryClass.CallStaticAJO("readAndUnread");
            query.CallAJO("withLimit", _limit);
            
            if (_types.Length > 0)
            {
                var types = Array.ConvertAll(_types, type => (int) type);
                query.CallAJO("ofTypes", types);    
            }
            
            if (_filter != Filter.NoFilter)
            {
                query.CallAJO("withFilter", _filter.ToAndroidJavaObject(), _notificationId);
            }
            return query;
        }
    
#elif UNITY_IOS
        public string ToJson()
        {
            var json = new Dictionary<string, object>
            {
                {"IsRead", _isRead},
                {"Types", Array.ConvertAll(_types, type => (int) type)},
                {"Limit", _limit},
                {"Filter", (int)_filter},
                {"FilteringNotificationId", _notificationId}
            };
            return GSJson.Serialize(json);
        }
#endif
    }
}
#if UNITY_IOS
using System;
using System.Collections.Generic;

namespace GetSocialSdk.Core
{
    delegate void VoidCallbackDelegate(IntPtr actionPtr);

    delegate void StringCallbackDelegate(IntPtr actionPtr, string data);

    delegate void BoolCallbackDelegate(IntPtr actionPtr, bool result);

    delegate void IntCallbackDelegate(IntPtr actionPtr, int result);

    delegate void FailureCallbackDelegate(IntPtr actionPtr, string error);

    delegate void FailureWithDataCallbackDelegate(IntPtr actionPtr, string data, string error);

    delegate void FetchReferralDataCallbackDelegate(IntPtr actionPtr, string referralDataJson);

    delegate bool NotificationListenerDelegate(IntPtr funcPtr, string notificationDataJson);

    public static class Callbacks
    {

        [AOT.MonoPInvokeCallback(typeof(VoidCallbackDelegate))]
        public static void ActionCallback(IntPtr actionPtr)
        {
            GetSocialDebugLogger.D("CompleteCallback");
            if (actionPtr != IntPtr.Zero)
            {
                actionPtr.Cast<Action>().Invoke();
            }
        }

        [AOT.MonoPInvokeCallback(typeof(StringCallbackDelegate))]
        public static void StringCallback(IntPtr actionPtr, string result)
        {
            GetSocialDebugLogger.D("StringResultCallaback: " + result);
            IOSUtils.TriggerCallback(actionPtr, result);
        }

        [AOT.MonoPInvokeCallback(typeof(BoolCallbackDelegate))]
        public static void BoolCallback(IntPtr actionPtr, bool result)
        {
            GetSocialDebugLogger.D("BoolCallback: " + result);
            IOSUtils.TriggerCallback(actionPtr, result);
        }

        [AOT.MonoPInvokeCallback(typeof(IntCallbackDelegate))]
        public static void IntCallback(IntPtr actionPtr, int result)
        {
            GetSocialDebugLogger.D("IntCallback: " + result);
            IOSUtils.TriggerCallback(actionPtr, result);
        }

        [AOT.MonoPInvokeCallback(typeof(FailureCallbackDelegate))]
        public static void FailureCallback(IntPtr actionPtr, string serializedError)
        {
            GetSocialDebugLogger.D("FailureCallback: " + serializedError + ", ptr: " + actionPtr.ToInt32());
            var error = new GetSocialError().ParseFromJson(serializedError);
            IOSUtils.TriggerCallback(actionPtr, error);
        }

        [AOT.MonoPInvokeCallback(typeof(FailureWithDataCallbackDelegate))]
        public static void FailureWithDataCallback(IntPtr actionPtr, string data, string errorMessage)
        {
            GetSocialDebugLogger.D("FailureWithDataCallback: " + errorMessage + ", data: " + data);
            if (actionPtr != IntPtr.Zero)
            {
                actionPtr.Cast<Action<string, string>>().Invoke(data, errorMessage);
            }
        }

        [AOT.MonoPInvokeCallback(typeof(FetchReferralDataCallbackDelegate))]
        public static void FetchReferralDataCallback(IntPtr actionPtr, string referralData)
        {
            GetSocialDebugLogger.D("OnReferralDataReceived: " + referralData);
            ReferralData data = null;
            if (!string.IsNullOrEmpty(referralData))
            {
                data = new ReferralData().ParseFromJson(referralData.ToDict());
            }
            IOSUtils.TriggerCallback(actionPtr, data);
        }

        [AOT.MonoPInvokeCallback(typeof(NotificationListenerDelegate))]
        public static bool NotificationListener(IntPtr funcPtr, string dataStr)
        {
            GetSocialDebugLogger.D("NotificationReceived: " + dataStr);
            var data = dataStr.ToDict();
            var wasClicked = (bool) data["wasClicked"];
            var notification = (string) data["notification"];
            var notificationDictionary = notification.ToDict();
            var notificationEntity = new Notification().ParseFromJson(notificationDictionary);
            if (funcPtr != IntPtr.Zero)
            {
                return funcPtr.Cast<Func<Notification, bool, bool>>().Invoke(notificationEntity, wasClicked);
            }
            return false;
        }
        
        static void GetObjectCallback<T>(IntPtr actionPtr, string json) where T : IConvertableFromNative<T>, new()
        {  
            if (actionPtr != IntPtr.Zero)
            {
                var result = GSJsonUtils.Parse<T>(json);
                IOSUtils.TriggerCallback(actionPtr, result);
            }
        }
        
        [AOT.MonoPInvokeCallback(typeof(StringCallbackDelegate))]
        public static void GetActivityPost(IntPtr actionPtr, string json)
        {
            GetObjectCallback<ActivityPost>(actionPtr, json);
        }
        
        [AOT.MonoPInvokeCallback(typeof(StringCallbackDelegate))]
        public static void GetPublicUser(IntPtr actionPtr, string json)
        {
            GetObjectCallback<PublicUser>(actionPtr, json);
        }
        
        static void GetObjectsListCallback<T>(IntPtr actionPtr, string json) where T : IConvertableFromNative<T>, new()
        {
            var result = GSJsonUtils.ParseList<T>(json);
            IOSUtils.TriggerCallback(actionPtr, result);
        }

        [AOT.MonoPInvokeCallback(typeof(StringCallbackDelegate))]
        public static void GetActivityPosts(IntPtr actionPtr, string json)
        {
            GetObjectsListCallback<ActivityPost>(actionPtr, json);
        }

        [AOT.MonoPInvokeCallback(typeof(StringCallbackDelegate))]
        public static void GetPublicUsers(IntPtr actionPtr, string json)
        {
            GetObjectsListCallback<PublicUser>(actionPtr, json);
        }

        [AOT.MonoPInvokeCallback(typeof(StringCallbackDelegate))]
        public static void GetSuggestedFriends(IntPtr actionPtr, string json)
        {
            GetObjectsListCallback<SuggestedFriend>(actionPtr, json);
        }

        [AOT.MonoPInvokeCallback(typeof(StringCallbackDelegate))]
        public static void GetReferredUsers(IntPtr actionPtr, string json)
        {
            GetObjectsListCallback<ReferredUser>(actionPtr, json);
        }

        [AOT.MonoPInvokeCallback(typeof(StringCallbackDelegate))]
        public static void GetUserReferences(IntPtr actionPtr, string json)
        {
            GetObjectsListCallback<UserReference>(actionPtr, json);
        }

        [AOT.MonoPInvokeCallback(typeof(StringCallbackDelegate))]
        public static void GetPublicUsersDictionary(IntPtr actionPtr, string json)
        {
            GetObjectsDictionaryCallback<PublicUser>(actionPtr, json);
        }
        
        [AOT.MonoPInvokeCallback(typeof(StringCallbackDelegate))]
        public static void GetNotificationsCallback(IntPtr actionPtr, string json)
        {
            GetObjectsListCallback<Notification>(actionPtr, json);
        }
        
        static void GetObjectsDictionaryCallback<TValue>(IntPtr actionPtr, string json) where TValue : IConvertableFromNative<TValue>, new()
        {
            var result = GSJsonUtils.ParseDictionary<PublicUser>(json);
            IOSUtils.TriggerCallback(actionPtr, result);
        }
        

    }
}

#endif
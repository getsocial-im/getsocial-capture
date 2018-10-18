using System;
using UnityEngine;

#if UNITY_ANDROID
namespace GetSocialSdk.Core
{
    public static class EnumConversion
    {
        public static AndroidJavaObject ToAndroidJavaObject(this ReportingReason reportingReason)
        {
            return ToAndroidJavaObject((int) reportingReason, "im.getsocial.sdk.activities.ReportingReason");
        }
        
        public static AndroidJavaObject ToAndroidJavaObject(this ActivitiesQuery.Filter filter)
        {
            return ToAndroidJavaObject((int) filter, "im.getsocial.sdk.activities.ActivitiesQuery$Filter");
        }
        
        public static AndroidJavaObject ToAndroidJavaObject(this NotificationsQuery.Filter filter)
        {
            return ToAndroidJavaObject((int) filter, "im.getsocial.sdk.pushnotifications.NotificationsQuery$Filter");
        }

        private static AndroidJavaObject ToAndroidJavaObject<T>(T type, string javaClass) where T : IConvertible
        {
            var reasonClass = new AndroidJavaClass(javaClass);
            var values = reasonClass.CallStaticAJO("values");
            return new AndroidJavaClass("java.lang.reflect.Array").CallStaticAJO("get", values, type);
        }
    }
}
#endif
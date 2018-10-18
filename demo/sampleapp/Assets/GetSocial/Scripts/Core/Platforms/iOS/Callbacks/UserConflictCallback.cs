#if UNITY_IOS
using System;
using AOT;

namespace GetSocialSdk.Core
{
    delegate void OnUserConflictDelegate(IntPtr onConflictActionPtr, string conflictUserJson);

    public static class UserConflictCallback
    {
        [MonoPInvokeCallback(typeof(OnUserConflictDelegate))]
        public static void OnUserAuthConflict(IntPtr onConflictActionPtr, string conflictUserJson)
        {
            var conflictUser = new ConflictUser().ParseFromJson(conflictUserJson.ToDict());
            IOSUtils.TriggerCallback(onConflictActionPtr, conflictUser);
        }
    }
}
#endif
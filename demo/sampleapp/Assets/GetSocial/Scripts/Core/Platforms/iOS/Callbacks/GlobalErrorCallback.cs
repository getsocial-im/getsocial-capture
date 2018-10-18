#if UNITY_IOS
using System;
using AOT;

namespace GetSocialSdk.Core
{
    delegate void GlobalErrorCallbackDelegate(IntPtr onGlobalErrorPtr, string errorJson);

    public static class GlobalErrorCallback
    {
        [AOT.MonoPInvokeCallback(typeof(GlobalErrorCallback))]
        public static void OnGlobalError(IntPtr onGlobalErrorActionPtr, string serializedError)
        {
            GetSocialDebugLogger.D("OnGlobalError : " + serializedError);
            var error = new GetSocialError().ParseFromJson(serializedError);
            IOSUtils.TriggerCallback(onGlobalErrorActionPtr, error);
        }
    }
}
#endif
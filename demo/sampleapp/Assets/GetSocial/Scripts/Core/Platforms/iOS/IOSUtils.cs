#if UNITY_IOS
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace GetSocialSdk.Core
{
    static class IOSUtils
    {
        public static IntPtr GetPointer(this object obj)
        {
            return obj == null ? IntPtr.Zero : GCHandle.ToIntPtr(GCHandle.Alloc(obj));
        }

        public static T Cast<T>(this IntPtr instancePtr)
        {
            var instanceHandle = GCHandle.FromIntPtr(instancePtr);
            if (!(instanceHandle.Target is T))
            {
                throw new InvalidCastException("Failed to cast IntPtr for type " + typeof(T));
            }

            var castedTarget = (T) instanceHandle.Target;
            return castedTarget;
        }

        public static void TriggerCallback<T>(IntPtr actionPtr, T result)
        {
            if (actionPtr != IntPtr.Zero)
            {
                actionPtr.Cast<Action<T>>().Invoke(result);
            }
        }
    }
}

#endif
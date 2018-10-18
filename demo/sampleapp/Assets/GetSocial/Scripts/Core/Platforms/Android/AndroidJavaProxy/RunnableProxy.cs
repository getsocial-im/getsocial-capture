#if UNITY_ANDROID
using UnityEngine;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GetSocialSdk.Core
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    internal class RunnableProxy : JavaInterfaceProxy
    {
        readonly Action _runnable;

        public RunnableProxy(Action runnable)
            : base("java.lang.Runnable")
        {
            _runnable = runnable;
        }

        void run()
        {
            ExecuteOnMainThread(_runnable);
        }
    }
}
#endif
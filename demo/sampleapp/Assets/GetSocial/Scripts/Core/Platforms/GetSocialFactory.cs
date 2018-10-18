using System;
using System.Linq;
using UnityEngine;

namespace GetSocialSdk.Core
{
    static class GetSocialFactory
    {

        internal enum AvailableRuntimes
        {
            Android = 0,
            iOS = 1,
            Windows = 2,
            OSX = 3,
            Linux = 4,
            Editor = 5,
            Mock = 6
        }

        private static IGetSocialNativeBridge _nativeImplementation;

        internal static IGetSocialNativeBridge Instance
        {
            get { return _nativeImplementation ?? (_nativeImplementation = FindNativeBridge()); }
        }

        private static IGetSocialNativeBridge FindNativeBridge()
        {

            IGetSocialNativeBridge nativeBridge = null;
#if UNITY_ANDROID && !UNITY_EDITOR
            nativeBridge = FindBridgeImplementation(AvailableRuntimes.Android);
#elif UNITY_IOS && !UNITY_EDITOR
            nativeBridge = FindBridgeImplementation(AvailableRuntimes.iOS);
#elif UNITY_STANDALONE_WIN
            nativeBridge = FindBridgeImplementation(AvailableRuntimes.Windows);
#elif UNITY_STANDALONE_LINUX
            nativeBridge = FindBridgeImplementation(AvailableRuntimes.Linux);
#elif UNITY_STANDALONE_OSX
            nativeBridge = FindBridgeImplementation(AvailableRuntimes.OSX);
#elif UNITY_EDITOR
            nativeBridge = FindBridgeImplementation(AvailableRuntimes.Editor);
#else
            nativeBridge = GetSocialNativeBridgeMock.Instance;
#endif
            if (nativeBridge != null)
            {
                return nativeBridge;
            }
            
            if (Application.isEditor)
            {
                return GetSocialNativeBridgeMock.Instance;
            }
            
            throw new Exception("Could not find native implementation.");
        }

        private static IGetSocialNativeBridge FindBridgeImplementation(AvailableRuntimes currentRuntime)
        {
            var type = typeof(IGetSocialNativeBridge);
            var nativeImpl = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsAbstract)
                .Select(implementation => (IGetSocialNativeBridge) Activator.CreateInstance(implementation))
                .FirstOrDefault(impl => impl.RuntimeImplementation.Contains(currentRuntime));
            return nativeImpl;
        }
    }
}
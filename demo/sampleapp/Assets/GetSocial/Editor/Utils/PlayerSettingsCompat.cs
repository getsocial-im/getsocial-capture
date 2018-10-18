using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace GetSocialSdk.Editor
{
    public class PlayerSettingsCompat
    {
        public static string bundleIdentifier
        {
            get
            {
                #if UNITY_5_6_OR_NEWER
                var appId = PlayerSettings.applicationIdentifier;
                #else
                var appId = PlayerSettings.bundleIdentifier;
                #endif
                return appId;
            }

            set
            {
                #if UNITY_5_6_OR_NEWER
                PlayerSettings.applicationIdentifier = value;
                #else
                PlayerSettings.bundleIdentifier = value;
                #endif
            }
        }

        public static string iPhoneBundleIdentifier
        {
            get
            {
                #if UNITY_5_6_OR_NEWER
                var iOsBundleIdentifier = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.iOS);
                #else
                var iOsBundleIdentifier = PlayerSettings.iPhoneBundleIdentifier;
                #endif
                return iOsBundleIdentifier;
            }
        }

        public static string targetOSVersion
        {
            get
            {
                #if UNITY_5_5_OR_NEWER
                var targetOSVersion = PlayerSettings.iOS.targetOSVersionString;
                #else
                var versionMapping = new Dictionary<iOSTargetOSVersion, string> {
                    { iOSTargetOSVersion.iOS_4_0, "4.0" },
                    { iOSTargetOSVersion.iOS_4_1, "4.1" },
                    { iOSTargetOSVersion.iOS_4_2, "4.2" },
                    { iOSTargetOSVersion.iOS_4_3, "4.3" },
                    { iOSTargetOSVersion.iOS_5_0, "5.0" },
                    { iOSTargetOSVersion.iOS_5_1, "5.1" },
                    { iOSTargetOSVersion.iOS_6_0, "6.0" },
                    { iOSTargetOSVersion.iOS_7_0, "7.0" },
                    { iOSTargetOSVersion.iOS_7_1, "7.1" },
                    { iOSTargetOSVersion.iOS_8_0, "8.0" },
                    { iOSTargetOSVersion.iOS_8_1, "8.1" },
                };

                var targetOsVersionEnum = PlayerSettings.iOS.targetOSVersion;
                var targetOSVersion = versionMapping.ContainsKey(targetOsVersionEnum) 
                    ? versionMapping[targetOsVersionEnum] : iOSTargetOSVersion.Unknown.ToString();
                #endif
                return targetOSVersion;
            }
        }
    }
}
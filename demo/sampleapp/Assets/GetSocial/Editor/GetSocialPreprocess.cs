#if UNITY_5_6_OR_NEWER
using UnityEditor;
using UnityEditor.Build;
#if UNITY_2018_1_OR_NEWER
using UnityEditor.Build.Reporting;
#endif
using UnityEngine;

namespace GetSocialSdk.Editor
{
    #if UNITY_2018_1_OR_NEWER
    public class GetSocialPreprocess : IPreprocessBuildWithReport
    #elif UNITY_5_6_OR_NEWER
    public class GetSocialPreprocess : IPreprocessBuild
    #endif
    {
    #if UNITY_2018_1_OR_NEWER
        public void OnPreprocessBuild(BuildReport report)
        {
            checkFrameworks(report.summary.platform);
        }
    #elif UNITY_5_6_OR_NEWER
        public void OnPreprocessBuild(BuildTarget target, string path)
        {
            checkFrameworks(target);
        }
    #endif

        private void checkFrameworks(BuildTarget target) 
        {
            if (target == BuildTarget.iOS)
            {
                if (!FileHelper.CheckiOSFramework())
                {
                    Debug.LogError("GetSocial: Native libraries for GetSocial SDK are missing. Download it before building the project");
                }
            }
            
            if (target == BuildTarget.Android)
            {
                if (!FileHelper.CheckAndroidFramework())
                {
                    Debug.LogError("GetSocial: Native libraries for GetSocial SDK are missing. Download it before building the project");
                }
            }
        }

        public int callbackOrder {
            get { return 1; }
        }
    }
    
}

#endif


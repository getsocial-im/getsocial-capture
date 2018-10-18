#if UNITY_IOS && USE_GETSOCIAL_UI
using System.IO;
using GetSocialSdk.Core;
using System.Runtime.InteropServices;
using UnityEngine;

namespace GetSocialSdk.Ui
{
    class GetSocialUiNativeBridgeIOS : IGetSocialUiNativeBridge
    {
        static IGetSocialUiNativeBridge _instance;

        public static IGetSocialUiNativeBridge Instance
        {
            get { return _instance ?? (_instance = new GetSocialUiNativeBridgeIOS()); }
        }

        #region IGetSocialUiNativeBridge implementation

        public bool LoadDefaultConfiguration()
        {
            return _gs_loadDefaultConfiguration();
        }

        public bool LoadConfiguration(string filePath)
        {
            var fullPath = Path.Combine(Application.streamingAssetsPath, filePath);
            GetSocialDebugLogger.D("Loading configuration at path: " + fullPath);
            return _gs_loadConfiguration(fullPath);
        }

        public bool ShowView<T>(ViewBuilder<T> viewBuilder) where T : ViewBuilder<T>
        {
            return viewBuilder.ShowInternal();
        }

        public bool CloseView(bool saveViewState)
        {
            return _gs_closeView(saveViewState);
        }

        public bool RestoreView()
        {
            return _gs_restoreView();
        }

        #endregion

        [DllImport("__Internal")]
        static extern bool _gs_loadDefaultConfiguration();

        [DllImport("__Internal")]
        static extern bool _gs_loadConfiguration(string filePath);

        [DllImport("__Internal")]
        static extern bool _gs_closeView(bool saveViewState);

        [DllImport("__Internal")]
        static extern bool _gs_restoreView();
    }
}
#endif

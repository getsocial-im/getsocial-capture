using System.Reflection;
using GetSocialSdk.Core;

#if USE_GETSOCIAL_UI

namespace GetSocialSdk.Ui
{
    sealed class GetSocialUiNativeBridgeMock : IGetSocialUiNativeBridge
    {
        static GetSocialUiNativeBridgeMock _instance;

#if UNITY_ANDROID
        public bool OnBackPressed()
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod());
            return false;
        }
#endif
        
        public static GetSocialUiNativeBridgeMock Instance
        {
            get { return _instance ?? (_instance = new GetSocialUiNativeBridgeMock()); }
        }

        #region IGetSocialUiNativeBridge implementation

        public bool LoadDefaultConfiguration()
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod());
            return false;
        }

        public bool LoadConfiguration(string filePath)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), filePath);
            return false;
        }

        public bool ShowView<T>(ViewBuilder<T> viewBuilder) where T : ViewBuilder<T>
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod(), viewBuilder);
            return false;
        }

        public bool CloseView(bool saveViewState)
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod());
            return false;
        }

        public bool RestoreView()
        {
            DebugUtils.LogMethodCall(MethodBase.GetCurrentMethod());
            return false;
        }

        #endregion
    }
}
#endif

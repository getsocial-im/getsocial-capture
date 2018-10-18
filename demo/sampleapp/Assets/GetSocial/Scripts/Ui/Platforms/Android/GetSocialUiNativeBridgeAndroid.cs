#if UNITY_ANDROID && USE_GETSOCIAL_UI
using GetSocialSdk.Core;
using UnityEngine;

namespace GetSocialSdk.Ui
{
    class GetSocialUiNativeBridgeAndroid : IGetSocialUiNativeBridge
    {
        const string GetSocialUiClassSignature = "im.getsocial.sdk.ui.GetSocialUi";
        const string AndroidUiAccessHelperClass = "im.getsocial.sdk.ui.GetSocialUiAccessHelper";

        static IGetSocialUiNativeBridge _instance;

        readonly AndroidJavaClass _getUiSocialJavaClass;

        GetSocialUiNativeBridgeAndroid()
        {
            _getUiSocialJavaClass = new AndroidJavaClass(GetSocialUiClassSignature);
        }

        public static IGetSocialUiNativeBridge Instance
        {
            get { return _instance ?? (_instance = new GetSocialUiNativeBridgeAndroid()); }
        }

        #region IGetSocialUiNativeBridge implementation

        public bool LoadDefaultConfiguration()
        {
            return JniUtils.RunOnUiThreadSafe(() =>
            {
                _getUiSocialJavaClass.CallStaticBool("loadDefaultConfiguration", JniUtils.Activity);
            });
        }

        public bool LoadConfiguration(string path)
        {
            return JniUtils.RunOnUiThreadSafe(() =>
            {
                _getUiSocialJavaClass.CallStaticBool("loadConfiguration", JniUtils.Activity, path);
            });
        }

        public bool ShowView<T>(ViewBuilder<T> viewBuilder) where T : ViewBuilder<T>
        {
            return viewBuilder.ShowInternal();
        }

        public bool OnBackPressed()
        {
            return _getUiSocialJavaClass.CallStaticBool("onBackPressed");
        }

        public bool CloseView(bool saveViewState)
        {
            return JniUtils.RunOnUiThreadSafe(() =>
            {
                _getUiSocialJavaClass.CallStatic("closeView", saveViewState);
            });
        }

        public bool RestoreView()
        {
            return JniUtils.RunOnUiThreadSafe(() =>
            {
                _getUiSocialJavaClass.CallStatic("restoreView");
            });
        }
        #endregion
    }
}

#endif

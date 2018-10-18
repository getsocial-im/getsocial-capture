#if USE_GETSOCIAL_UI
namespace GetSocialSdk.Ui
{
    static class GetSocialUiFactory
    {
        internal static IGetSocialUiNativeBridge InstantiateGetSocialUi()
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
            return GetSocialUiNativeBridgeAndroid.Instance;
            #elif UNITY_IOS && !UNITY_EDITOR
            return GetSocialUiNativeBridgeIOS.Instance;
            #else
            // if UNITY_EDITOR
            return GetSocialUiNativeBridgeMock.Instance;
            #endif
        }
    }
}
#endif

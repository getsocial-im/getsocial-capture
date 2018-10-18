#if USE_GETSOCIAL_UI
namespace GetSocialSdk.Ui
{
    interface IGetSocialUiNativeBridge
    {
        bool LoadDefaultConfiguration();

        bool LoadConfiguration(string filePath);

        bool ShowView<T>(ViewBuilder<T> viewBuilder) where T : ViewBuilder<T>;
        
        bool CloseView(bool saveViewState);

        bool RestoreView();

#if UNITY_ANDROID
        bool OnBackPressed();
#endif
    }
}
#endif

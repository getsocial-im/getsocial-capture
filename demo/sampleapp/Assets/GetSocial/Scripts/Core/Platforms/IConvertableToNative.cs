namespace GetSocialSdk.Core
{
    public interface IConvertableToNative
    {
#if UNITY_ANDROID
        UnityEngine.AndroidJavaObject ToAjo();
#elif UNITY_IOS
        string ToJson();
#endif
    }
}
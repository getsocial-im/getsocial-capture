#if UNITY_IOS
using System.Collections.Generic;
#endif
namespace GetSocialSdk.Core
{
    public interface IConvertableFromNative<out T>
    {
#if UNITY_ANDROID
        T ParseFromAJO(UnityEngine.AndroidJavaObject ajo);
#elif UNITY_IOS
        T ParseFromJson(Dictionary<string, object> json);
#endif
    }
}
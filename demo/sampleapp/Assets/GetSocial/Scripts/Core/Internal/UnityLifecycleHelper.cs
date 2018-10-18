using UnityEngine;

namespace GetSocialSdk.Core
{
    public class UnityLifecycleHelper : Singleton<UnityLifecycleHelper>
    {
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            LoadInstance();
        }
        
        void Start()
        {
            GetSocialFactory.Instance.HandleOnStartUnityEvent();
        }
    }
}
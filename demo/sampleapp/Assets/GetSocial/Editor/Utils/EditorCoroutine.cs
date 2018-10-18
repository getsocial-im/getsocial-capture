using System.Collections;
using UnityEditor;

namespace GetSocialSdk.Editor
{
    public class EditorCoroutine
    {
        readonly IEnumerator _routine;
        
        public static EditorCoroutine Start(IEnumerator _routine)
        {
            EditorCoroutine coroutine = new EditorCoroutine(_routine);
            coroutine.Start();
            return coroutine;
        }

        EditorCoroutine(IEnumerator _routine)
        {
            this._routine = _routine;
        }

        private void Start()
        {
            //Debug.Log("start");
            EditorApplication.update += Update;
        }

        public void Stop()
        {
            //Debug.Log("stop");
            EditorApplication.update -= Update;
        }

        void Update()
        {
            /*
            *    IMPORTANT: EditorCoroutine works only with yield return null 
            */
            //Debug.Log("update");
            if (!_routine.MoveNext())
            {
                Stop();
            }
        }
    }
}

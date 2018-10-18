#if UNITY_2017_1_OR_NEWER

using System.IO;
using GetSocialSdk.Core;
using UnityEditor;
using UnityEngine;

namespace GetSocialSdk.Editor
{
    public class GetSocialGetStartedWindow : EditorWindow
    {
        private const string DoNotShowGetSocialWelcomeScreenPref = "DoNotShowGetSocialGetStartedWindow";
        private const string PageToLoadPath = "Editor/HTML/index.html";
        
        private WebViewWrapper _webView;

        
        [MenuItem("GetSocial/Getting Started", false, 1000)]
        static void Open()
        {
            GetSocialGetStartedWindow window = GetWindow<GetSocialGetStartedWindow>("GetSocial");
            window.maxSize = new Vector2(470, 600);
            window.minSize = new Vector2(470, 400);
            window.Show(true);
        }

        
        #region lifecycle
        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            // Open window automatically on the first launch
            if (!PlayerPrefs.HasKey(DoNotShowGetSocialWelcomeScreenPref))
            {
                PlayerPrefs.SetInt(DoNotShowGetSocialWelcomeScreenPref, 1);
                PlayerPrefs.Save();

                Open();
            }
        }
        
        void OnEnable()
        {
            if (!_webView)
            {
                InitWebView();
            }
        }

        public void OnBecameInvisible()
        {
            if (_webView)
            {
                // Signal the browser to unhook
                _webView.Detach();
            }
        }

        void OnDestroy()
        {
            DestroyImmediate(_webView);
        }

        void OnGUI()
        {
            DrawLoadingLabel();
            DrawWebView();
        }
        #endregion
        
        
        #region private methods
        private void InitWebView()
        {
            _webView = CreateInstance<WebViewWrapper>();
            _webView.MessageFromJavascriptReceived = ProcessJsMessage;
            _webView.LocationChanged = (url) =>
            {
                _webView.ExecuteJavascript(
                    string.Format("initPage('{0}', '{1}')", EditorGUIUtility.isProSkin.ToString(), BuildConfig.PublishTarget));
            };
        }
        
        private void ProcessJsMessage(string jsonMessage)
        {
            // We have only one message, so can skip any logic
            GetSocialSettingsEditor.Edit();
        }
        
        private void DrawLoadingLabel()
        {
            GUILayout.FlexibleSpace();
            var style = new GUIStyle(GUI.skin.label) {alignment = TextAnchor.MiddleCenter};
            EditorGUILayout.LabelField("Loading...", style, GUILayout.ExpandWidth(true));
            GUILayout.FlexibleSpace();
        }
        
        private void DrawWebView()
        {
            if (_webView.Hook(this))
            {
                var pagePath = Path.Combine(GetSocialSettings.GetAbsolutePluginPath(), PageToLoadPath);
                _webView.LoadFile(pagePath);
            }

            _webView.OnGUI(new Rect(0, 0, position.width, position.height));
        }
        #endregion

    }
}

#endif
using System;

#if UNITY_ANDROID
using GetSocialSdk.Core;
using UnityEngine;
#endif

#if USE_GETSOCIAL_UI

namespace GetSocialSdk.Ui
{
    /// <summary>
    /// Base class for GetSocial UI view builder
    /// </summary>
    /// <typeparam name="T">Type of view to create</typeparam>
    public abstract class ViewBuilder<T> where T : ViewBuilder<T>
    {
        protected Action _onOpen, _onClose;
        protected string _customWindowTitle;
        protected UiActionListener _uiActionListener;

        /// <summary>
        /// Set a custom title for a GetSocial view.
        /// </summary>
        /// <param name="title">Custom title</param>
        /// <returns>Instance of Builder to chain calls</returns>
        public T SetWindowTitle(string title)
        {
            _customWindowTitle = title;
            return (T) this;
        }

        /// <summary>
        /// Set view state callback actions that will be invoked when GetSocial View is opened and closed.
        /// </summary>
        /// <param name="onOpen">Invoked when view is opened.</param>
        /// <param name="onClose">Invoked when view is closed</param>
        /// <returns>Instance of Builder to chain calls</returns>
        public T SetViewStateCallbacks(Action onOpen, Action onClose)
        {
            _onOpen = onOpen;
            _onClose = onClose;
            return (T) this;
        }

        /// <summary>
        /// Sets the UI action listener. It allows you to track actions done by users while using GetSocial UI.
        /// Also, you can allow or disallow users to perform some action. To perform an action, in UI action listener call
        /// pendingAction(). Without calling it, action won't be invoked.
        /// If you don't set a listener, all actions will be performed.
        /// </summary>
        /// <param name="listener">Listener instance</param>
        /// <returns>Instance of Builder to chain calls</returns>
        public T SetUiActionListener(UiActionListener listener)
        {
            _uiActionListener = listener;
            return (T) this;
        }

        /// <summary>
        /// Show View.
        /// </summary>
        /// <returns>true if view was shown, false otherwise</returns>
        internal abstract bool ShowInternal();

        /// <summary>
        /// Show View.
        /// </summary>
        /// <returns>true if view was shown, false otherwise</returns>
        public bool Show()
        {
            return GetSocialUi.ShowView(this);
        }
        

#if UNITY_ANDROID

        protected bool ShowBuilder(AndroidJavaObject builder)
        {
            SetUiActionListenerAJO(builder);
            SetTitleAJO(builder);
            SetViewStateListener(builder);
            // Make sure ui is instantiated at this point for ensuring OnResume was called before opening the view
            GetSocialUiFactory.InstantiateGetSocialUi();

            return JniUtils.RunOnUiThreadSafe(() =>
            {
                using (builder)
                {
                    builder.CallBool("show");
                }
            });
        }

        private void SetTitleAJO(AndroidJavaObject builderAJO)
        {
            if (_customWindowTitle != null)
            {
                builderAJO.CallAJO("setWindowTitle", _customWindowTitle);
            }
        }

        private void SetUiActionListenerAJO(AndroidJavaObject builderAJO)
        {
            if (_uiActionListener != null)
            {
                builderAJO.CallAJO("setUiActionListener", new UiActionListenerProxy(_uiActionListener));
            }
        }

        private void SetViewStateListener(AndroidJavaObject builderAJO)
        {
            if (_onOpen != null || _onClose != null)
            {
                builderAJO.CallAJO("setViewStateListener",
                    new ViewStateListener(_onOpen, _onClose));
            }
        }

#endif
    }
}

#endif
#if USE_GETSOCIAL_UI

using GetSocialSdk.Core;
using System;

#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

#if UNITY_ANDROID
using UnityEngine;
#endif

namespace GetSocialSdk.Ui
{
    /// <summary>
    /// Use this class to construct smart invites view.
    /// Call <see cref="Show()"/> to present the UI.
    /// </summary>
    public sealed class InvitesViewBuilder : ViewBuilder<InvitesViewBuilder>
    {
#pragma warning disable 414
        LinkParams _linkParams;
        InviteContent _inviteContent;
        Action<string> _onInviteComplete;
        Action<string> _onInviteCancel;
        Action<string, GetSocialError> _onInviteFailure;
#pragma warning restore 414

        /// <summary>
        /// Sets the custom invite content.
        /// </summary>
        /// <returns>The custom invite content.</returns>
        /// <param name="inviteContent">Custom invite content to send with this invite.</param>
        public InvitesViewBuilder SetCustomInviteContent(InviteContent inviteContent)
        {
            _inviteContent = inviteContent;
            return this;
        }

        /// <summary>
        /// Sets the custom referral data.
        /// </summary>
        /// <returns>The custom referral data.</returns>
        /// <param name="customReferralData">Custom referral data.</param>
        /// Deprecated, use <see cref="SetLinkParams"/> instead.
        [Obsolete("Deprecated, use SetLinkParams instead.")]
        public InvitesViewBuilder SetCustomReferralData(CustomReferralData customReferralData)
        {
            _linkParams = new LinkParams(customReferralData);
            return this;
        }

        /// <summary>
        /// Sets the link parameters.
        /// </summary>
        /// <returns>InviteBuilder.</returns>
        /// <param name="linkParams">Link parameters.</param>
        public InvitesViewBuilder SetLinkParams(LinkParams linkParams)
        {
            _linkParams = linkParams;
            return this;
        }

        /// <summary>
        /// Sets the invite callbacks.
        /// </summary>
        /// <returns>The invite callbacks.</returns>
        /// <param name="onComplete">Invoked when sending invite was successfull.</param>
        /// <param name="onCancel">Invoked when sending invite was cancelled by the user.</param>
        /// <param name="onFailure">Invoked when sending invite failed.</param>
        public InvitesViewBuilder SetInviteCallbacks(Action<string> onComplete, Action<string> onCancel,
            Action<string, GetSocialError> onFailure)
        {
            Check.Argument.IsNotNull(onComplete, "onComplete");
            Check.Argument.IsNotNull(onCancel, "onCancel");
            Check.Argument.IsNotNull(onFailure, "onFailure");

            _onInviteComplete = onComplete;
            _onInviteCancel = onCancel;
            _onInviteFailure = onFailure;
            return this;
        }

        #region implemented abstract members of ViewBuilder

        /// <summary>
        /// Present smart ivites view.
        /// </summary>
        internal override bool ShowInternal()
        {
#if UNITY_ANDROID
            return ShowBuilder(ToAJO());
#elif UNITY_IOS
            var serializedInviteContent = _inviteContent == null ? null : _inviteContent.ToJson();
            var serializedLinkParams = _linkParams == null ? null : _linkParams.ToJson();

            return _gs_showSmartInvitesView(_customWindowTitle, serializedInviteContent, serializedLinkParams,
                Callbacks.StringCallback, _onInviteComplete.GetPointer(), _onInviteCancel.GetPointer(),
                Callbacks.FailureWithDataCallback, _onInviteFailure.GetPointer(),
                Callbacks.ActionCallback, _onOpen.GetPointer(),
                Callbacks.ActionCallback, _onClose.GetPointer(),
                UiActionListenerCallback.OnUiAction, _uiActionListener.GetPointer());
#else
            return false;
#endif
        }

        #endregion

#if UNITY_ANDROID
        AndroidJavaObject ToAJO()
        {
            var invitesBuilderAJO = new AndroidJavaObject("im.getsocial.sdk.ui.invites.InvitesViewBuilder");

            if (_linkParams != null)
            {
                invitesBuilderAJO.CallAJO("setLinkParams", _linkParams.ToAjo());
            }

            if (_inviteContent != null)
            {
                var inviteContentAJO = _inviteContent.ToAjo();
                invitesBuilderAJO.CallAJO("setCustomInviteContent", inviteContentAJO);
            }

            if (_onInviteComplete != null)
            {
                invitesBuilderAJO.CallAJO("setInviteCallback",
                    new InviteUiCallbackProxy(_onInviteComplete, _onInviteCancel, _onInviteFailure));
            }

            return invitesBuilderAJO;
        }
#elif UNITY_IOS

        [DllImport("__Internal")]
        static extern bool _gs_showSmartInvitesView(
            string title,
            string serializedInviteContent,
            string serializedLinkParams,
            StringCallbackDelegate stringCallback, IntPtr onInviteCompletePtr, IntPtr onInviteCancelPtr,
            FailureWithDataCallbackDelegate failureCallback, IntPtr onFailurePtr,
            Action<IntPtr> onOpenAction, IntPtr onOpenActionPtr,
            Action<IntPtr> onCloseAction, IntPtr onCloseActionPtr,
            Action<IntPtr, int> uiActionListener, IntPtr uiActionListenerPtr);

#endif
    }
}

#endif

#if USE_GETSOCIAL_UI
using System;
using GetSocialSdk.Core;

#if UNITY_IOS
using System.Runtime.InteropServices;
using System.Collections.Generic;
using GetSocialSdk.MiniJSON;
#endif

#if UNITY_ANDROID
using UnityEngine;
#endif

namespace GetSocialSdk.Ui
{
    /// <summary>
    /// Use this class to construct activity feed view.
    /// Call <see cref="Show()"/> to present the UI.
    /// </summary>
    public sealed class ActivityFeedViewBuilder : ViewBuilder<ActivityFeedViewBuilder>
    {
#pragma warning disable 414
        readonly string _feed;

        Action<string, ActivityPost> _onButtonClickListener;
        Action<PublicUser> _onAvatarClickListener;
        Action<string> _onMentionClickListener;
        Action<string> _tagClickListener;
        
        string _filterUserId;
        bool _readOnly;
        bool _friendsFeed;
        string[] _tags = {};
#pragma warning restore 414
        
        internal ActivityFeedViewBuilder()
        {
            _feed = ActivitiesQuery.GlobalFeed;
        }

        internal ActivityFeedViewBuilder(string feed)
        {
            _feed = feed;
        }

        /// <summary>
        /// Register callback to listen when activity action button was clicked.
        /// </summary>
        /// <param name="onButtonClickListener">Called when activity action button was clicked.</param>
        /// <returns><see cref="ActivityFeedViewBuilder"/> instance.</returns>
        public ActivityFeedViewBuilder SetButtonActionListener(Action<string, ActivityPost> onButtonClickListener)
        {
            _onButtonClickListener = onButtonClickListener;

            return this;
        }

        /// <summary>
        /// Set a listener that will be called when user taps on someones avatar.
        /// </summary>
        /// <param name="onAvatarClickListener"></param>
        /// <returns><see cref="ActivityFeedViewBuilder"/> instance.</returns>
        public ActivityFeedViewBuilder SetAvatarClickListener(Action<PublicUser> onAvatarClickListener)
        {
            _onAvatarClickListener = onAvatarClickListener;

            return this;
        }

        /// <summary>
        /// Set a listener that will be called when user taps on mention in activity post.
        /// </summary>
        /// <param name="mentionClickListener">Called with ID of mentioned user or one of the shortcuts listed in <see cref="MentionShortcuts"/>.</param>
        /// <returns><see cref="ActivityFeedViewBuilder"/> instance.</returns>
        public ActivityFeedViewBuilder SetMentionClickListener(Action<string> mentionClickListener)
        {
            _onMentionClickListener = mentionClickListener;

            return this;
        }

        /// <summary>
        /// Set tag click listener, that will be notified if tag was clicked.
        /// </summary>
        /// <param name="tagClickListener">Called with name of tag that was clicked.</param>
        /// <returns><see cref="ActivityFeedViewBuilder"/> instance.</returns>
        public ActivityFeedViewBuilder SetTagClickListener(Action<string> tagClickListener) {
            _tagClickListener = tagClickListener;

            return this;
        }
        
        
        /// <summary>
        /// Set this to valid user id if you want to display feed of only one user.
        ///  If is not set, normal feed will be shown.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns><see cref="ActivityFeedViewBuilder"/> instance.</returns>
        public ActivityFeedViewBuilder SetFilterByUser(String userId) {
            _filterUserId = userId;
            
            return this;
        }
        
        /// <summary>
        /// Make the feed read-only. UI elements, that allows to post, comment or like are hidden.
        /// </summary>
        /// <param name="readOnly">should feed be read-only</param>
        /// <returns><see cref="ActivityFeedViewBuilder"/> instance.</returns>
        public ActivityFeedViewBuilder SetReadOnly(bool readOnly) {
            _readOnly = readOnly;
            
            return this;
        }
        
        /// <summary>
        /// Display feed with posts of your friends and your own.
        /// </summary>
        /// <param name="showFriendsFeed">display friends feed or not</param>
        /// <returns><see cref="ActivityDetailsViewBuilder"/> instance.</returns>
        public ActivityFeedViewBuilder SetShowFriendsFeed(bool showFriendsFeed)
        {
            _friendsFeed = showFriendsFeed;

            return this;
        }

        /// <summary>
        /// Display feed with posts, that contains at least one tag from the list.
        /// </summary>
        /// <param name="tags"> List for tags that have to be present in activity feed posts.</param>
        /// <returns><see cref="ActivityFeedViewBuilder"/> instance.</returns>
        public ActivityFeedViewBuilder SetFilterByTags(params string[] tags) {
            _tags = tags;

            return this;
        }

        internal override bool ShowInternal()
        {
#if UNITY_ANDROID
            return ShowBuilder(ToAJO());
#elif UNITY_IOS
            return _gs_showActivityFeedView(_customWindowTitle, _feed, _filterUserId, _readOnly, _friendsFeed, GSJson.Serialize(new List<string>(_tags)),
                ActivityFeedActionButtonCallback.OnActionButtonClick,
                _onButtonClickListener.GetPointer(),
                Callbacks.ActionCallback,
                _onOpen.GetPointer(),
                Callbacks.ActionCallback,
                _onClose.GetPointer(),
                UiActionListenerCallback.OnUiAction,
                _uiActionListener.GetPointer(),
                AvatarClickListenerCallback.OnAvatarClicked,
                _onAvatarClickListener.GetPointer(),
                MentionClickListenerCallback.OnMentionClicled,
                _onMentionClickListener.GetPointer(),
                TagClickListenerCallback.OnTagClicked,
                _tagClickListener.GetPointer());
#else
            return false;
#endif
        }

#if UNITY_ANDROID

        AndroidJavaObject ToAJO()
        {
            var activityFeedBuilderAJO =
                new AndroidJavaObject("im.getsocial.sdk.ui.activities.ActivityFeedViewBuilder", _feed);

            if (_filterUserId != null) {
                activityFeedBuilderAJO.CallAJO("setFilterByUser", _filterUserId);
            }
            if (_onButtonClickListener != null)
            {
                activityFeedBuilderAJO.CallAJO("setButtonActionListener",
                    new ActionButtonListenerProxy(_onButtonClickListener));
            }
            if (_onAvatarClickListener != null)
            {
                activityFeedBuilderAJO.CallAJO("setAvatarClickListener",
                    new AvatarClickListenerProxy(_onAvatarClickListener));
            }
            if (_onMentionClickListener != null)
            {
                activityFeedBuilderAJO.CallAJO("setMentionClickListener",
                    new MentionClickListenerProxy(_onMentionClickListener));
            }
            if (_tagClickListener != null) 
            {
                activityFeedBuilderAJO.CallAJO("setTagClickListener",
                    new TagClickListenerProxy(_tagClickListener));   
            }

            activityFeedBuilderAJO.CallAJO("setReadOnly", _readOnly);
            activityFeedBuilderAJO.CallAJO("setShowFriendsFeed", _friendsFeed);
            activityFeedBuilderAJO.CallAJO("setFilterByTags", _tags.ToJavaStringArray());

            return activityFeedBuilderAJO;
        }

#elif UNITY_IOS

        [DllImport("__Internal")]
        static extern bool _gs_showActivityFeedView(string customWindowTitle, string feed, string filterUserId, bool readOnly, bool friendsFeed, string tagsList,
            Action<IntPtr, string, string> onActionButtonClick, IntPtr onButtonClickPtr,
            Action<IntPtr> onOpenAction, IntPtr onOpenActionPtr,
            Action<IntPtr> onCloseAction, IntPtr onCloseActionPtr,
            Action<IntPtr, int> uiActionListener, IntPtr uiActionListenerPtr,
            Action<IntPtr, string> avatarClickListener, IntPtr avatarClickListenerPtr,
            Action<IntPtr, string> mentionClickListener, IntPtr mentionClickListenerPtr,
            Action<IntPtr, string> tagClickListener, IntPtr tagClickListenerPtr);

#endif
    }
}

#endif
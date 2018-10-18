using System;
using UnityEngine;
#if UNITY_IOS
using System.Collections.Generic;
using GetSocialSdk.MiniJSON;
#endif

namespace GetSocialSdk.Core
{

    /// <summary>
    ///
    /// </summary>
    public sealed class ActivityPostContent : IConvertableToNative
    {
#pragma warning disable 414
        string _text;
        Texture2D _image;
        string _buttonTitle;
        string _buttonAction;
        byte[] _video;
        
#pragma warning restore 414
        ActivityPostContent()
        {
        }

        public static Builder CreateBuilder()
        {
            return new Builder();
        }

        public class Builder
        {
            internal Builder()
            {
            }

            readonly ActivityPostContent _content = new ActivityPostContent();

            public Builder WithText(string text)
            {
                _content._text = text;
                return this;
            }

            public Builder WithImage(Texture2D image)
            {
                _content._image = image;
                return this;
            }

            public Builder WithButton(String title, String action)
            {
                _content._buttonTitle = title;
                _content._buttonAction = action;
                return this;
            }

            public Builder WithVideo(byte[] video)
            {
                _content._video = video;
                return this;
            }

            public ActivityPostContent Build()
            {
                return _content;
            }
        }

#if UNITY_ANDROID
        public AndroidJavaObject ToAjo()
        {
            var activityPostContentBuilderAJO = new AndroidJavaObject("im.getsocial.sdk.activities.ActivityPostContent$Builder");

            if (_text != null)
            {
                activityPostContentBuilderAJO.CallAJO("withText", _text);
            }
            if (_image != null)
            {
                activityPostContentBuilderAJO.CallAJO("withImage", _image.ToAjoBitmap());
            }
            if (_buttonAction != null && _buttonTitle != null)
            {
                activityPostContentBuilderAJO.CallAJO("withButton", _buttonTitle, _buttonAction);
            }
            if (_video != null)
            {
                activityPostContentBuilderAJO.CallAJO("withVideo", _video);
            }
            return activityPostContentBuilderAJO.CallAJO("build");
        }
#elif UNITY_IOS

        public string ToJson()
        {
            var json = new Dictionary<string, object>
            {
                {"Text", _text},
                {"ButtonTitle", _buttonTitle},
                {"ButtonAction", _buttonAction},
                {"Image", _image.TextureToBase64()},
                {"Video", _video.ByteArrayToBase64()}
            };
            return GSJson.Serialize(json);
        }

#endif
    }
}
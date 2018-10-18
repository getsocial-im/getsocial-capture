#if UNITY_ANDROID
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace GetSocialSdk.Core
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    internal class ListCallbackProxy<T> : JavaInterfaceProxy where T : IConvertableFromNative<T>, new()
    {
        readonly Action<List<T>> _onSuccess;
        readonly Action<GetSocialError> _onFailure;

        public ListCallbackProxy(Action<List<T>> onSuccess, Action<GetSocialError> onFailure)
            : base("im.getsocial.sdk.Callback")
        {
            _onSuccess = onSuccess;
            _onFailure = onFailure;
        }

        void onSuccess(AndroidJavaObject resultAJO)
        {
            var value = resultAJO.FromJavaList().ConvertAll(ajo =>
            {
                using (ajo)
                {
                    return new T().ParseFromAJO(ajo);
                }
            }).ToList();
            HandleValue(value, _onSuccess);
        }

        void onFailure(AndroidJavaObject throwable)
        {
            HandleError(throwable, _onFailure);
        }
    }
}

#endif
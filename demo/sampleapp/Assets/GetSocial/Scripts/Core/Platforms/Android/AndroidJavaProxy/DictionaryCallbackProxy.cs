#if UNITY_ANDROID

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;

namespace GetSocialSdk.Core
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    internal class DictionaryCallbackProxy<TValue> : JavaInterfaceProxy where TValue: IConvertableFromNative<TValue>, new()
    {
        private readonly Action<Dictionary<string, TValue>> _onSuccess;
        private readonly Action<GetSocialError> _onFailure;

        public DictionaryCallbackProxy(Action<Dictionary<string, TValue>> onSuccess, Action<GetSocialError> onFailure) :
            base("im.getsocial.sdk.Callback")
        {
            _onSuccess = onSuccess;
            _onFailure = onFailure;
        }
        
        void onSuccess(AndroidJavaObject resultAJO)
        {
            var retValue = new Dictionary<string, TValue>();

            if (resultAJO != null && !resultAJO.IsJavaNull())
            {
                var iterator = resultAJO.CallAJO("keySet").CallAJO("iterator");
                while (iterator.CallBool("hasNext"))
                {
                    var key = iterator.CallStr("next");
                    var value = new TValue().ParseFromAJO(resultAJO.Call<AndroidJavaObject>("get", key));
                    retValue.Add(key, value);
                }
            }
            
            HandleValue(retValue, _onSuccess);
        }

        void onFailure(AndroidJavaObject throwable)
        {
            HandleError(throwable, _onFailure);
        }
    }
}

#endif
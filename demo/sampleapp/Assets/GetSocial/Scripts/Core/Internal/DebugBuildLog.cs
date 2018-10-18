using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace GetSocialSdk.Core
{
    [DebuggerStepThrough]
    static class GetSocialDebugLogger
    {
        [Conditional("DEVELOPMENT_BUILD")]
        public static void D(string message, params object[] arguments)
        {
            if (arguments != null && arguments.Length > 0)
            {
                Debug.Log(string.Format(message, arguments));
            }
            else
            {
                Debug.Log(message);
            }
        }

        [Conditional("DEVELOPMENT_BUILD")]
        public static void I(string message, params object[] arguments)
        {
            if (arguments != null && arguments.Length > 0)
            {
                Debug.Log(string.Format(message, arguments));
            }
            else
            {
                Debug.Log(message);
            }
        }

        [Conditional("DEVELOPMENT_BUILD")]
        public static void W(string message, params object[] arguments)
        {
            if (arguments != null && arguments.Length > 0)
            {
                Debug.LogWarning(string.Format(message, arguments));
            }
            else
            {
                Debug.LogWarning(message);
            }
        }


        [Conditional("DEVELOPMENT_BUILD")]
        public static void E(string message, params object[] arguments)
        {
            if (arguments != null && arguments.Length > 0)
            {
                Debug.LogError(string.Format(message, arguments));
            }
            else
            {
                Debug.LogError(message);
            }
        }

        [Conditional("DEVELOPMENT_BUILD")]
        public static void Ex(Exception ex, string message, params object[] arguments)
        {
            Debug.LogException(ex);

            E(message, arguments);
        }
    }
}
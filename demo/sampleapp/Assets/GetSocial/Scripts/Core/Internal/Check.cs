using System;
using UnityEngine;

namespace GetSocialSdk.Core
{
    static class Check
    {
        public delegate bool Condition();

        public static void IfTrue(Condition condition, string message = "")
        {
            if (!condition())
            {
                throw new ArgumentException(message);
            }
        }

        public static void IfTrue(bool condition, string message = "")
        {
            if (!condition)
            {
                Debug.LogWarning(message);
            }
        }

        public static class Argument
        {
            public static void IsNotNull(object argument, string argumentName, string message = null)
            {
                if (message == null)
                {
                    message = string.Format("{0} must not be null", argumentName);
                }
                if (argument == null)
                {
                    Debug.LogWarning(message);
                }
            }

            public static void IsStrNotNullOrEmpty(string argument, string argumentName, string message = null)
            {
                if (message == null)
                {
                    message = string.Format("{0} must not be null or empty", argumentName);
                }
                if (string.IsNullOrEmpty(argument))
                {
                    Debug.LogWarning(message);
                }
            }

            public static void IsNotNegative(int argument, string argumentName)
            {
                if (argument < 0)
                {
                    Debug.LogWarning(argumentName + " must not be negative.");
                }
            }
        }
    }
}
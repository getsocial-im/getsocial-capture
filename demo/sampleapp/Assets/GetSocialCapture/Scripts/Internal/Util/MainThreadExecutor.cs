using System;
using System.Collections.Generic;
using UnityEngine;

namespace GetSocialSdk.Scripts.Internal.Util
{
    internal class MainThreadExecutor : Singleton<MainThreadExecutor>
    {
        readonly System.Object _queueLock = new System.Object();
        readonly List<Action> _queuedActions = new List<Action>();
        readonly List<Action> _executingActions = new List<Action>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            LoadInstance();
        }

        internal static void Queue(Action action)
        {
            if (action == null)
            {
                Debug.LogWarning("Trying to queue null action");
                return;
            }

            lock (Instance._queueLock)
            {
                Instance._queuedActions.Add(action);
            }
        }

        void Update()
        {
            MoveQueuedActionsToExecuting();

            while (_executingActions.Count > 0)
            {
                Action action = _executingActions[0];
                _executingActions.RemoveAt(0);
                action();
            }
        }

        void MoveQueuedActionsToExecuting()
        {
            lock (_queueLock)
            {
                while (_queuedActions.Count > 0)
                {
                    Action action = _queuedActions[0];
                    _executingActions.Add(action);
                    _queuedActions.RemoveAt(0);
                }
            }
        }
    }
}


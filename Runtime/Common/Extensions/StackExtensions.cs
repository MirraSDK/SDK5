using System;
using System.Collections.Generic;
using UnityEngine;

namespace MirraGames.SDK.Common {

    public static class StackExtensions {

        // A callback that throws is reported and the remaining callbacks still run:
        // one failing listener must not leave the others waiting forever.
        public static void PopInvokeAll(this Stack<Action> stack) {
            while (stack.Count > 0) {
                Action action = stack.Pop();
                try {
                    action?.Invoke();
                }
                catch (Exception exception) {
                    Debug.LogException(exception);
                }
            }
        }

        public static void PopInvokeAll<T>(this Stack<Action<T>> stack, T value) {
            while (stack.Count > 0) {
                Action<T> action = stack.Pop();
                try {
                    action?.Invoke(value);
                }
                catch (Exception exception) {
                    Debug.LogException(exception);
                }
            }
        }

    }

}

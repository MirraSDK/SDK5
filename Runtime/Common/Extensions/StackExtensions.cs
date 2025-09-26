using System;
using System.Collections.Generic;

namespace MirraGames.SDK.Common {

    public static class StackExtensions {

        public static void PopInvokeAll(this Stack<Action> stack) {
            while (stack.Count > 0) {
                stack.Pop()?.Invoke();
            }
        }

        public static void PopInvokeAll<T>(this Stack<Action<T>> stack, T value) {
            while (stack.Count > 0) {
                stack.Pop()?.Invoke(value);
            }
        }

    }

}
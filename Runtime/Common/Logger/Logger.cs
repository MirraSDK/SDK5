using UnityEngine;

namespace MirraGames.SDK.Common {

    public static class Logger {

        static Logger() {
#if UNITY_EDITOR
            EnableTextMessages = UnityEditor.EditorPrefs.GetBool($"{Naming.MirraSDK5}.{nameof(EnableTextMessages)}");
            EnableWarningMessages = UnityEditor.EditorPrefs.GetBool($"{Naming.MirraSDK5}.{nameof(EnableWarningMessages)}");
            EnableErrorMessages = UnityEditor.EditorPrefs.GetBool($"{Naming.MirraSDK5}.{nameof(EnableErrorMessages)}");
#endif
        }

        public static bool EnableTextMessages { get; } = true;
        public static bool EnableWarningMessages { get; } = true;
        public static bool EnableErrorMessages { get; } = true;

        public static void NotImplementedWarning(object source, string methodName) {
            CreateWarning(source, methodName, "not implemented");
        }

        public static void NotAvailableWarning(object source, string methodName) {
            CreateWarning(source, methodName, "not available");
        }

        public static void CreateText(object source, params object[] content) {
            CreateText(source.GetType().Name, content);
        }

        public static void CreateText(string source, params object[] content) {
            if (!EnableTextMessages) return;
            Debug.Log($"{source}: {string.Join(' ', content)}");
        }

        public static void CreateWarning(object source, params object[] content) {
            CreateWarning(source.GetType().Name, content);
        }

        public static void CreateWarning(string source, params object[] content) {
            if (!EnableWarningMessages) return;
            Debug.LogWarning($"{source}: {string.Join(' ', content)}");
        }

        public static void CreateError(object source, params object[] content) {
            CreateError(source.GetType().Name, content);
        }

        public static void CreateError(string source, params object[] content) {
            if (!EnableErrorMessages) return;
            Debug.LogError($"{source}: {string.Join(' ', content)}");
        }

    }

}
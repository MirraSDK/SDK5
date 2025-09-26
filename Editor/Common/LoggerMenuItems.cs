using MirraGames.SDK.Common;
using UnityEditor;

namespace MirraGames.SDK.Editor {

    internal static class LoggerMenuItems {

        private const string MenuRoot = Naming.MirraSDK5 + "/Logger/";
        private const string TextMenu = MenuRoot + "EnableTextMessages";
        private const string WarningMenu = MenuRoot + "EnableWarningMessages";
        private const string ErrorMenu = MenuRoot + "EnableErrorMessages";

        private static string PrefKey(string name) => $"{Naming.MirraSDK5}.{name}";

        [MenuItem(TextMenu)]
        private static void EnableTextMessages() {
            Toggle(PrefKey(nameof(EnableTextMessages)));
        }

        [MenuItem(TextMenu, true)]
        private static bool ValidateTextMessages() {
            Validate(PrefKey(nameof(EnableTextMessages)), TextMenu);
            return true;
        }

        [MenuItem(WarningMenu)]
        private static void EnableWarningMessages() {
            Toggle(PrefKey(nameof(EnableWarningMessages)));
        }

        [MenuItem(WarningMenu, true)]
        private static bool ValidateWarningMessages() {
            Validate(PrefKey(nameof(EnableWarningMessages)), WarningMenu);
            return true;
        }

        [MenuItem(ErrorMenu)]
        private static void EnableErrorMessages() {
            Toggle(PrefKey(nameof(EnableErrorMessages)));
        }

        [MenuItem(ErrorMenu, true)]
        private static bool ValidateErrorMessages() {
            Validate(PrefKey(nameof(EnableErrorMessages)), ErrorMenu);
            return true;
        }

        private static void Toggle(string key) {
            bool current = EditorPrefs.GetBool(key, false);
            EditorPrefs.SetBool(key, !current);
            Menu.SetChecked(MenuRoot, false);
        }

        private static void Validate(string key, string menuPath) {
            bool current = EditorPrefs.GetBool(key, false);
            Menu.SetChecked(menuPath, current);
        }

    }

}
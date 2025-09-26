using MirraGames.SDK.Common;

namespace MirraGames.SDK.Editor {

    public static class PreferencesEditorExtensions {

        public static void SetModuleBool(this PreferencesEditor preferencesEditor, string configurationName, string interfaceName, string key, bool value) {
            preferencesEditor.SetBool(configurationName, Naming.Key(interfaceName, key), value);
        }

        public static void SetModuleString(this PreferencesEditor preferencesEditor, string configurationName, string interfaceName, string key, string value) {
            preferencesEditor.SetString(configurationName, Naming.Key(interfaceName, key), value);
        }

    }

}
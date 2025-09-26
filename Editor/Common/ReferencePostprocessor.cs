using UnityEditor;

namespace MirraGames.SDK.Editor {

    internal class ReferencePostprocessor : AssetPostprocessor {

        [InitializeOnLoadMethod]
        public static void OnMethodLoad() {
            ExecuteIntegrityChecks();
        }

        public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
            ExecuteIntegrityChecks();
        }

        private static void ExecuteIntegrityChecks() {
            bool hasChanges = PreferencesEditor.CheckFileIntegrity()
                || VisualTreeReferenceGenerator.UpdateReferences()
                || PrefabReferenceGenerator.UpdateReferences();
            if (hasChanges) {
                AssetDatabase.Refresh();
            }
        }

    }

}
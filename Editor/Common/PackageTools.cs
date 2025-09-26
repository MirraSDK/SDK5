using MirraGames.SDK.Common;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace MirraGames.SDK.Editor {

    internal static class PackageTools {

        public static string ProjectId {
            get => Application.dataPath.GetHashCode().ToString();
        }

        public static string ProjectPath {
            get {
                string projectPath = Application.dataPath;
                int assetsIndex = projectPath.LastIndexOf("/Assets");
                if (assetsIndex >= 0) {
                    projectPath = projectPath.Substring(0, assetsIndex);
                }
                return projectPath.NormalizePath();
            }
        }

        public static string GetPrefsString(string key, string defaultValue = "") {
            return EditorPrefs.GetString($"{Naming.MirraSDK5}[{ProjectId}].{key}", defaultValue);
        }

        public static void SetPrefsString(string key, string value) {
            EditorPrefs.SetString($"{Naming.MirraSDK5}[{ProjectId}].{key}", value);
        }

    }

}
using MirraGames.SDK.Common;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Editor {

    internal static class PackageFiles {

        public static readonly string AbsoluteResourcesPath = GetAbsoluteResourcesPath();
        public static readonly string RelativeResourcesPath = GetRelativeResourcesPath();
        public static readonly string RelativePackageDatabasePath = GetRelativePackageDatabasePath();

        private static string GetAbsoluteResourcesPath() {
            return Path.Combine(Application.dataPath, Naming.Resources, Naming.MirraSDK5).NormalizePath();
        }

        private static string GetRelativeResourcesPath() {
            return Path.Combine(Naming.Assets, Naming.Resources, Naming.MirraSDK5).NormalizePath();
        }

        private static string GetRelativePackageDatabasePath() {
            return Path.Combine(Naming.Packages, Naming.PackageName).NormalizePath();
        }

        public static Texture2D FindTextureAsset(string name) {
            string[] searchPaths = new string[] { RelativePackageDatabasePath };
            string[] guids = AssetDatabase.FindAssets($"t:Texture2D {name}", searchPaths);
            foreach (string guid in guids) {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
                if (texture != null) {
                    return texture;
                }
            }
            return null;
        }

        public static VisualTreeAsset[] FindVisualTreeAssets() {
            string[] searchPaths = new string[] { RelativePackageDatabasePath };
            string[] guids = AssetDatabase.FindAssets("t:VisualTreeAsset", searchPaths);
            HashSet<VisualTreeAsset> visualTreeAssets = new();
            foreach (string guid in guids) {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                VisualTreeAsset visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(assetPath);
                if (visualTreeAsset != null) {
                    visualTreeAssets.Add(visualTreeAsset);
                }
            }
            return visualTreeAssets.ToArray();
        }

        public static GameObject[] FindPrefabs() {
            string[] searchPaths = new string[] { RelativePackageDatabasePath };
            string[] guids = AssetDatabase.FindAssets("t:Prefab", searchPaths);
            HashSet<GameObject> prefabs = new();
            foreach (string guid in guids) {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                if (prefab != null) {
                    prefabs.Add(prefab);
                }
            }
            return prefabs.ToArray();
        }

    }

}
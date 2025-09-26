using MirraGames.SDK.Common;
using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using Logger = MirraGames.SDK.Common.Logger;
using System.Linq;

namespace MirraGames.SDK.Editor {

    internal class PrefabReferenceGenerator {

        public static bool UpdateReferences() {
            bool hasChanges = false;
            try {
                if (!Directory.Exists(PackageFiles.AbsoluteResourcesPath)) {
                    Directory.CreateDirectory(PackageFiles.AbsoluteResourcesPath);
                    hasChanges = true;
                }
                GameObject[] prefabAssets = PackageFiles.FindPrefabs();
                foreach (GameObject prefabAsset in prefabAssets) {
                    bool hasDuplicates = prefabAssets.Any(asset => asset != prefabAsset && asset.name == prefabAsset.name);
                    if (hasDuplicates) {
                        throw new Exception("Duplicate Prefab name found: " + prefabAsset.name);
                    }
                }
                foreach (GameObject prefabAsset in prefabAssets) {
                    if (CheckReference(prefabAsset)) {
                        hasChanges = true;
                    }
                }
            }
            catch (Exception exception) {
                Logger.CreateError(nameof(PrefabReferenceGenerator), exception);
            }
            return hasChanges;
        }

        private static bool CheckReference(GameObject prefabAsset) {
            bool hasChanges = false;
            string systemFilePath = Path.Combine(PackageFiles.AbsoluteResourcesPath, prefabAsset.name + Naming.AssetExtension).NormalizePath();
            if (!File.Exists(systemFilePath)) {
                GenerateReference(prefabAsset);
                return true;
            }
            string databaseFilePath = Path.Combine(PackageFiles.RelativeResourcesPath, prefabAsset.name + Naming.AssetExtension).NormalizePath();
            PrefabReference prefabReference = AssetDatabase.LoadAssetAtPath<PrefabReference>(databaseFilePath);
            if (prefabReference == null || prefabReference.Prefab != prefabAsset) {
                Logger.CreateWarning(nameof(PrefabReferenceGenerator), "Invalid", prefabAsset.name);
                AssetDatabase.DeleteAsset(databaseFilePath);
                GenerateReference(prefabAsset);
                hasChanges = true;
            }
            return hasChanges;
        }

        private static void GenerateReference(GameObject prefabAsset) {
            try {
                string referencePath = Path.Combine(PackageFiles.RelativeResourcesPath, prefabAsset.name + Naming.AssetExtension).NormalizePath();
                PrefabReference prefabReference = ScriptableObject.CreateInstance<PrefabReference>();
                prefabReference.Prefab = prefabAsset;
                AssetDatabase.CreateAsset(UnityEngine.Object.Instantiate(prefabReference), referencePath);
            }
            catch (Exception exception) {
                Logger.CreateError(nameof(PrefabReferenceGenerator), exception);
            }
        }

    }

}
using MirraGames.SDK.Common;
using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using Logger = MirraGames.SDK.Common.Logger;
using System.Linq;

namespace MirraGames.SDK.Editor {

    internal class VisualTreeReferenceGenerator {

        public static bool UpdateReferences() {
            bool hasChanges = false;
            try {
                if (!Directory.Exists(PackageFiles.AbsoluteResourcesPath)) {
                    Directory.CreateDirectory(PackageFiles.AbsoluteResourcesPath);
                    hasChanges = true;
                }
                VisualTreeAsset[] visualTreeAssets = PackageFiles.FindVisualTreeAssets();
                foreach (VisualTreeAsset visualTreeAsset in visualTreeAssets) {
                    bool hasDuplicates = visualTreeAssets.Any(asset => asset != visualTreeAsset && asset.name == visualTreeAsset.name);
                    if (hasDuplicates) {
                        throw new Exception("Duplicate VisualTreeAsset name found: " + visualTreeAsset.name);
                    }
                }
                foreach (VisualTreeAsset visualTreeAsset in visualTreeAssets) {
                    if (CheckReference(visualTreeAsset)) {
                        hasChanges = true;
                    }
                }
            }
            catch (Exception exception) {
                Logger.CreateError(nameof(VisualTreeReferenceGenerator), exception);
            }
            return hasChanges;
        }

        private static bool CheckReference(VisualTreeAsset visualTreeAsset) {
            bool hasChanges = false;
            string systemFilePath = Path.Combine(PackageFiles.AbsoluteResourcesPath, visualTreeAsset.name + Naming.AssetExtension).NormalizePath();
            if (!File.Exists(systemFilePath)) {
                GenerateReference(visualTreeAsset);
                return true;
            }
            string databaseFilePath = Path.Combine(PackageFiles.RelativeResourcesPath, visualTreeAsset.name + Naming.AssetExtension).NormalizePath();
            VisualTreeReference visualTreeReference = AssetDatabase.LoadAssetAtPath<VisualTreeReference>(databaseFilePath);
            if (visualTreeReference == null || visualTreeReference.VisualTree != visualTreeAsset) {
                Logger.CreateWarning(nameof(VisualTreeReferenceGenerator), "Invalid", visualTreeAsset.name);
                AssetDatabase.DeleteAsset(databaseFilePath);
                GenerateReference(visualTreeAsset);
                hasChanges = true;
            }
            return hasChanges;
        }

        private static void GenerateReference(VisualTreeAsset visualTreeAsset) {
            try {
                string referencePath = Path.Combine(PackageFiles.RelativeResourcesPath, visualTreeAsset.name + Naming.AssetExtension).NormalizePath();
                VisualTreeReference visualTreeReference = ScriptableObject.CreateInstance<VisualTreeReference>();
                visualTreeReference.VisualTree = visualTreeAsset;
                AssetDatabase.CreateAsset(UnityEngine.Object.Instantiate(visualTreeReference), referencePath);
            }
            catch (Exception exception) {
                Logger.CreateError(nameof(VisualTreeReferenceGenerator), exception);
            }
        }

    }

}
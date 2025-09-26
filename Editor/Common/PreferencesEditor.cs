using MirraGames.SDK.Common;
using MirraGames.SDK.SourceGenerator;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Logger = MirraGames.SDK.Common.Logger;

namespace MirraGames.SDK.Editor {

    public class PreferencesEditor : PreferencesReader {

        public static bool CheckFileIntegrity() {
            bool hasChanges = false;
            PreferencesEditor preferencesEditor = CreateEditor();
            string editorConfigurationName = preferencesEditor.GetEditorConfigurationName();
            if (string.IsNullOrEmpty(editorConfigurationName)) {
                preferencesEditor.SetEditorConfigurationName(nameof(EditorConfiguration));
                hasChanges = true;
            }
            string buildConfigurationName = preferencesEditor.GetBuildConfigurationName();
            if (string.IsNullOrEmpty(buildConfigurationName)) {
                preferencesEditor.SetBuildConfigurationName("FallbackConfiguration");
                hasChanges = true;
            }
            return hasChanges;
        }

        public static PreferencesEditor CreateEditor() {
            Preferences preferences = LoadPreferencesFile();
            PreferencesEditor preferencesEditor = new(preferences);
            foreach (string configurationName in Mapping.Configurations.Keys) {
                Configuration configuration = Mapping.CreateConfigurationInstance(configurationName);
                DictionaryGroup defaultValueGroup = configuration.GetDefaultValuesGroup();
                preferencesEditor.InjectDefaultValueGroup(configurationName, defaultValueGroup);
            }
            return preferencesEditor;
        }

        private static Preferences LoadPreferencesFile() {
            string filePath = Path.Combine(Application.dataPath, Naming.Resources, Naming.MirraSDK5, nameof(Preferences)).NormalizePath();
            filePath = Path.ChangeExtension(filePath, "json");
            if (!File.Exists(filePath)) {
                Logger.CreateWarning(nameof(PreferencesEditor), "Creating new preferences file", filePath);
                return new Preferences();
            }
            string json = File.ReadAllText(filePath);
            return ParseFromJson(json);
        }

        public PreferencesEditor(Preferences preferences) : base(preferences) { }

        public void SetBuildConfigurationName(string configurationName) {
            SetString(Naming.Build, nameof(Configuration), configurationName);
        }

        public void SetEditorConfigurationName(string configurationName) {
            SetString(Naming.Editor, nameof(Configuration), configurationName);
        }

        public void SetBool(string groupKey, string key, bool value) {
            if (string.IsNullOrEmpty(groupKey) || string.IsNullOrEmpty(key)) {
                Logger.CreateError(this, "Invalid group or field key");
                return;
            }
            if (!preferenceGroups.ContainsKey(groupKey)) {
                preferenceGroups[groupKey] = new();
            }
            preferenceGroups[groupKey].boolCollection[key] = value;
            WriteToJson();
        }

        public void SetInt(string groupKey, string key, int value) {
            if (string.IsNullOrEmpty(groupKey) || string.IsNullOrEmpty(key)) {
                Logger.CreateError(this, "Invalid group or field key");
                return;
            }
            if (!preferenceGroups.ContainsKey(groupKey)) {
                preferenceGroups[groupKey] = new();
            }
            preferenceGroups[groupKey].intCollection[key] = value;
            WriteToJson();
        }

        public void SetFloat(string groupKey, string key, float value) {
            if (string.IsNullOrEmpty(groupKey) || string.IsNullOrEmpty(key)) {
                Logger.CreateError(this, "Invalid group or field key");
                return;
            }
            if (!preferenceGroups.ContainsKey(groupKey)) {
                preferenceGroups[groupKey] = new();
            }
            preferenceGroups[groupKey].floatCollection[key] = value;
            WriteToJson();
        }

        public void SetString(string groupKey, string key, string value) {
            if (string.IsNullOrEmpty(groupKey) || string.IsNullOrEmpty(key)) {
                Logger.CreateError(this, "Invalid group or field key");
                return;
            }
            if (!preferenceGroups.ContainsKey(groupKey)) {
                preferenceGroups[groupKey] = new();
            }
            preferenceGroups[groupKey].stringCollection[key] = value;
            WriteToJson();
        }

        private void WriteToJson() {
            try {
                Preferences preferences = CreatePreferences();
                string json = JsonUtility.ToJson(preferences, true);
                string assetsPath = Application.dataPath;
                string resourcesPath = Path.Combine(assetsPath, Naming.Resources).NormalizePath();
                Directory.CreateDirectory(resourcesPath);
                string fileName = $"{nameof(Preferences)}.json";
                string mirraSDKPath = Path.Combine(resourcesPath, Naming.MirraSDK5).NormalizePath();
                Directory.CreateDirectory(mirraSDKPath);
                string filePath = Path.Combine(mirraSDKPath, fileName).NormalizePath();
                File.WriteAllText(filePath, json);
                AssetDatabase.Refresh();
            }
            catch (Exception exception) {
                Logger.CreateError(this, "Failed to write file", exception);
            }
        }

        private Preferences CreatePreferences() {
            Preferences preferences = new();
            foreach (string groupKey in preferenceGroups.Keys) {
                SerializableGroup serializableGroup = new();
                DictionaryGroup dictionaryGroup = preferenceGroups[groupKey];
                Dictionary<string, bool> boolCollection = dictionaryGroup.boolCollection;
                foreach (string boolKey in boolCollection.Keys) {
                    KeyBoolPair keyBoolPair = new() {
                        key = boolKey,
                        value = boolCollection[boolKey]
                    };
                    serializableGroup.boolCollection.Add(keyBoolPair);
                }
                Dictionary<string, int> intCollection = dictionaryGroup.intCollection;
                foreach (string intKey in intCollection.Keys) {
                    KeyIntPair keyIntPair = new() {
                        key = intKey,
                        value = intCollection[intKey]
                    };
                    serializableGroup.intCollection.Add(keyIntPair);
                }
                Dictionary<string, float> floatCollection = dictionaryGroup.floatCollection;
                foreach (string floatKey in floatCollection.Keys) {
                    KeyFloatPair keyFloatPair = new() {
                        key = floatKey,
                        value = floatCollection[floatKey]
                    };
                    serializableGroup.floatCollection.Add(keyFloatPair);
                }
                Dictionary<string, string> stringCollection = dictionaryGroup.stringCollection;
                foreach (string stringKey in stringCollection.Keys) {
                    KeyStringPair keyStringPair = new() {
                        key = stringKey,
                        value = stringCollection[stringKey]
                    };
                    serializableGroup.stringCollection.Add(keyStringPair);
                }
                KeySerializableGroupPair keySerializableGroupPair = new() {
                    key = groupKey,
                    group = serializableGroup
                };
                preferences.groupCollection.Add(keySerializableGroupPair);
            }
            return preferences;
        }

    }

}
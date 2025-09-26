using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MirraGames.SDK.Common {

    public class PreferencesReader {

        public static PreferencesReader CreateReader() {
            Preferences preferences = LoadFromResources();
            PreferencesReader preferencesReader = new(preferences);
            return preferencesReader;
        }

        private static Preferences LoadFromResources() {
            string resourcePath = Path.Combine(Naming.MirraSDK5, nameof(Preferences));
            TextAsset preferencesAsset = Resources.Load<TextAsset>(resourcePath);
            Preferences preferences;
            if (preferencesAsset == null) {
                Logger.CreateWarning(nameof(PreferencesReader), nameof(Preferences), "not found");
                preferences = new();
            }
            else {
                preferences = ParseFromJson(preferencesAsset.text);
            }
            return preferences;
        }

        protected static Preferences ParseFromJson(string json) {
            Preferences preferences;
            if (string.IsNullOrEmpty(json)) {
                Logger.CreateWarning(nameof(PreferencesReader), nameof(Preferences), "is empty");
                preferences = new();
            }
            else {
                try {
                    preferences = JsonUtility.FromJson<Preferences>(json);
                    if (preferences == null) {
                        Logger.CreateWarning(nameof(PreferencesReader), "Failed to parse", nameof(Preferences));
                        preferences = new();
                    }
                }
                catch (Exception exception) {
                    Logger.CreateError(nameof(PreferencesReader), "Failed to parse", nameof(Preferences), exception);
                    preferences = new();
                }
            }
            return preferences;
        }

        internal readonly Dictionary<string, DictionaryGroup> preferenceGroups = new();
        internal readonly Dictionary<string, DictionaryGroup> defaultValueGroups = new();

        public PreferencesReader(Preferences preferences) {
            InjectPreferences(preferences);
        }

        public void InjectDefaultValueGroup(string key, DictionaryGroup defaultValueGroup) {
            defaultValueGroups[key] = defaultValueGroup;
        }

        protected void InjectPreferences(Preferences preferences) {
            preferenceGroups.Clear();
            foreach (var groupPair in preferences.groupCollection) {
                if (groupPair == null || string.IsNullOrEmpty(groupPair.key) || groupPair.group == null) {
                    Logger.CreateError(this, "Invalid group found");
                    continue;
                }
                if (preferenceGroups.ContainsKey(groupPair.key)) {
                    Logger.CreateError(this, "Duplicate group found", groupPair.key);
                    continue;
                }
                DictionaryGroup dictionaryGroup = new(groupPair.group);
                preferenceGroups.Add(groupPair.key, dictionaryGroup);
            }
        }

        public IPreferencesGroup GetPreferenceGroup(string key) {
            if (preferenceGroups.TryGetValue(key, out DictionaryGroup dictionaryGroup)) {
                return dictionaryGroup;
            }
            return DictionaryGroup.Empty;
        }

        public IPreferencesGroup GetDefaultValueGroup(string key) {
            if (defaultValueGroups.TryGetValue(key, out DictionaryGroup dictionaryGroup)) {
                return dictionaryGroup;
            }
            return DictionaryGroup.Empty;
        }

    }

}
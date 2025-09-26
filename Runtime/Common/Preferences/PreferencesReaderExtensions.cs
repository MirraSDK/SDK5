using System;
using UnityEngine;

namespace MirraGames.SDK.Common {

    public static class PreferencesReaderExtensions {

        public static PropertyGroup GetPreferencesPropertyGroup(this PreferencesReader reader, string configurationName, Type propertyGroupType) {
            string propertyGroupName = propertyGroupType.Name;
            try {
                string preferencesJson = reader.GetPreferenceGroup(configurationName).GetString(propertyGroupName);
                if (JsonUtility.FromJson(preferencesJson, propertyGroupType) is PropertyGroup propertyGroup) {
                    return propertyGroup;
                }
            }
            catch (Exception exception) {
                Logger.CreateError(nameof(PreferencesReaderExtensions), "Failed to parse", propertyGroupName, exception);
            }
            return default;
        }

        public static T GetPropertyGroup<T>(this PreferencesReader reader, string configurationName, bool returnDefault = false) where T : PropertyGroup, new() {
            string propertyGroupName = typeof(T).Name;
            bool isOverrideModuleEnabled = reader.IsOverrideModuleEnabled(configurationName, propertyGroupName) && !returnDefault;
            if (isOverrideModuleEnabled) {
                try {
                    string preferencesJson = reader.GetPreferenceGroup(configurationName).GetString(propertyGroupName);
                    T propertyGroup = JsonUtility.FromJson<T>(preferencesJson);
                    if (propertyGroup != null) {
                        return propertyGroup;
                    }
                }
                catch (Exception exception) {
                    Logger.CreateError(nameof(PreferencesReaderExtensions), "Failed to parse", propertyGroupName, exception);
                }
            }
            return new T();
        }

        public static string GetConfigurationName(this PreferencesReader reader) {
            bool isEditor = Application.isEditor;
            return isEditor ? GetEditorConfigurationName(reader) : GetBuildConfigurationName(reader);
        }

        public static string GetBuildConfigurationName(this PreferencesReader reader) {
            return reader.GetPreferenceGroup(Naming.Build).GetString(nameof(Configuration));
        }

        public static string GetEditorConfigurationName(this PreferencesReader reader) {
            return reader.GetPreferenceGroup(Naming.Editor).GetString(nameof(Configuration));
        }

        public static bool IsOverrideModuleEnabled(this PreferencesReader reader, string configurationName, string interfaceName) {
            return reader.GetPreferenceGroup(configurationName).GetBool(Naming.Key(interfaceName, Naming.Override));
        }

        public static bool GetPreferencesBool(this PreferencesReader reader, string configurationName, params string[] subkeys) {
            return reader.GetPreferenceGroup(configurationName).GetBool(Naming.Key(subkeys));
        }

        public static string GetDefaultString(this PreferencesReader reader, string configurationName, params string[] subkeys) {
            return reader.GetDefaultValueGroup(configurationName).GetString(Naming.Key(subkeys));
        }

        public static bool GetModuleBool(this PreferencesReader reader, string configurationName, string interfaceName, string key) {
            string valueKey = Naming.Key(interfaceName, key);
            bool isOverrideModuleEnabled = IsOverrideModuleEnabled(reader, configurationName, interfaceName);
            bool defaultValue = reader.GetDefaultValueGroup(configurationName).GetBool(valueKey);
            if (isOverrideModuleEnabled) {
                return reader.GetPreferenceGroup(configurationName).GetBool(valueKey, defaultValue);
            }
            return defaultValue;
        }

        public static string GetModuleString(this PreferencesReader reader, string configurationName, string interfaceName, string key) {
            string valueKey = Naming.Key(interfaceName, key);
            bool isOverrideModuleEnabled = IsOverrideModuleEnabled(reader, configurationName, interfaceName);
            string defaultValue = reader.GetDefaultValueGroup(configurationName).GetString(valueKey);
            if (isOverrideModuleEnabled) {
                return reader.GetPreferenceGroup(configurationName).GetString(valueKey, defaultValue);
            }
            return defaultValue;
        }

    }

}
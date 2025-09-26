using MirraGames.SDK.Common;
using MirraGames.SDK.SourceGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = MirraGames.SDK.Common.Logger;

namespace MirraGames.SDK.Editor {

    internal partial class ConfigurationInspector : VisualElement {

        private const int EnumFieldWidth = 175;
        private readonly Dictionary<SettingsFoldout, Type> foldoutTypeMapping = new();
        private readonly Dictionary<SettingsFoldout, PropertyGroup> configurationMapping = new();

        private PreferencesEditor preferencesEditor;

        public ConfigurationInspector() {
            VisualTreeReference reference = VisualTreeReference.Load(nameof(ConfigurationInspector));
            VisualTreeAsset asset = reference.VisualTree;
            asset.CloneTree(this);
        }

        internal VisualElement ConfigurationView => this.Q<VisualElement>(nameof(ConfigurationView));
        internal VisualElement ConfigurationContainer => this.Q<VisualElement>(nameof(ConfigurationContainer));
        internal VisualElement ProvidersContainer => this.Q<VisualElement>(nameof(ProvidersContainer));
        internal VisualElement ProvidersView => this.Q<VisualElement>(nameof(ProvidersView));

        private ConfigurationType SelectedConfiguration {
            get {
                string configurationName = PackageTools.GetPrefsString(nameof(SelectedConfiguration));
                return configurationName.ToEnumOrDefault<ConfigurationType>();
            }
            set {
                PackageTools.SetPrefsString(nameof(SelectedConfiguration), value.ToString());
            }
        }

        private string SelectedConfigurationName => SelectedConfiguration.ToString();

        public void SelectConfiguration(Type configurationType) {
            string configurationName = configurationType.Name;
            ConfigurationType selectedConfiguration = configurationName.ToEnumOrDefault<ConfigurationType>();
            Configuration configurationInstance = Mapping.CreateConfigurationInstance(configurationName);
            SelectedConfiguration = selectedConfiguration;
            preferencesEditor = PreferencesEditor.CreateEditor();
            Reset();
            InitializeConfigurationFoldouts(configurationInstance);
            InitializeProviderFoldouts(configurationInstance);
        }

        private void UpdateConfigurationFoldoutContent(SettingsFoldout settingsFoldout) {
            settingsFoldout.Clear();
            if (configurationMapping.TryGetValue(settingsFoldout, out PropertyGroup propertyGroup)) {
                Type propertyGroupType = propertyGroup.GetType();
                string preferencesKey = propertyGroupType.Name;

                string propertyGroupJson = preferencesEditor.GetPreferenceGroup(SelectedConfigurationName).GetString(preferencesKey, Naming.EmptyJson);
                bool isOverrideEnabled = preferencesEditor.IsOverrideModuleEnabled(SelectedConfigurationName, preferencesKey);

                PropertyGroup preferencesPropertyGroup = JsonUtility.FromJson(propertyGroupJson, propertyGroupType) as PropertyGroup;
                PropertyGroup defaultPropertyGroup = Activator.CreateInstance(propertyGroupType) as PropertyGroup; // TODO: create factory for this and avoid Activator
                PropertyGroup targetPropertyGroup = isOverrideEnabled ? preferencesPropertyGroup : defaultPropertyGroup;

                CreatePropertyFields(settingsFoldout, targetPropertyGroup, preferencesKey, true);
            }
        }

        private void InitializeConfigurationFoldouts(Configuration configurationInstance) {
            Type[] propertyGroupTypes = configurationInstance.PropertyGroups;
            if (propertyGroupTypes.Length > 0) {
                ConfigurationView.style.display = DisplayStyle.Flex;
            }
            else {
                ConfigurationView.style.display = DisplayStyle.None;
            }
            foreach (Type propertyGroupType in propertyGroupTypes) {
                string preferencesKey = propertyGroupType.Name;

                PropertyGroup propertyGroup = Activator.CreateInstance(propertyGroupType) as PropertyGroup; // TODO: create factory for this and avoid Activator
                bool isOverrideEnabled = preferencesEditor.IsOverrideModuleEnabled(SelectedConfigurationName, preferencesKey);

                SettingsFoldout groupFoldout = new(
                    overrideValue: isOverrideEnabled,
                    contentVisible: preferencesEditor.GetPreferencesBool(SelectedConfigurationName, preferencesKey, Naming.Visible),
                    overrideSetter: (value) => { }
                );
                groupFoldout.name = propertyGroup.Name;
                groupFoldout.Text = propertyGroup.Name;
                groupFoldout.OnContentVisibleChange += () => {
                    preferencesEditor.SetModuleBool(SelectedConfigurationName, preferencesKey, Naming.Visible, groupFoldout.ContentVisible);
                };
                groupFoldout.OnOverrideValueChange += () => {
                    preferencesEditor.SetModuleBool(SelectedConfigurationName, preferencesKey, Naming.Override, groupFoldout.OverrideValue);
                    UpdateConfigurationFoldoutContent(groupFoldout);
                };
                if (configurationInstance.ReadOnly) {
                    groupFoldout.HideOverrideToggle();
                }
                ConfigurationContainer.Add(groupFoldout);
                configurationMapping[groupFoldout] = propertyGroup;
                UpdateConfigurationFoldoutContent(groupFoldout);
            }
        }

        private void WritePropertyGroup(string preferencesKey, PropertyGroup propertyGroup) {
            string json = JsonUtility.ToJson(propertyGroup);
            preferencesEditor.SetString(SelectedConfigurationName, preferencesKey, json);
        }

        private void CreatePropertyFields(VisualElement contentContainer, PropertyGroup propertyGroup, string preferencesKey, bool clearContent) {
            if (clearContent) {
                contentContainer.Clear();
            }
            foreach (EnumProperty property in propertyGroup.GetEnumProperties()) {
                EnumField enumField = new(property.Name, property.Getter()) {
                    value = property.Getter()
                };
                enumField.RegisterValueChangedCallback(callback => {
                    property.Setter.Invoke(callback.newValue);
                    WritePropertyGroup(preferencesKey, propertyGroup);
                });
                contentContainer.Add(enumField);
            }
            foreach (BoolProperty property in propertyGroup.GetBoolProperties()) {
                BoolField boolField = new() {
                    Name = property.Name,
                    Value = property.Getter()
                };
                boolField.OnToggleClick += () => {
                    bool value = property.Getter();
                    property.Setter(!value);
                    boolField.Value = !value;
                    WritePropertyGroup(preferencesKey, propertyGroup);
                };
                contentContainer.Add(boolField);
            }
            foreach (StringProperty property in propertyGroup.GetStringProperties()) {
                TextField textField = new(property.Name) {
                    value = property.Getter()
                };
                textField.RegisterValueChangedCallback(callback => {
                    property.Setter(callback.newValue);
                    WritePropertyGroup(preferencesKey, propertyGroup);
                });
                contentContainer.Add(textField);
            }
            foreach (IntProperty property in propertyGroup.GetIntProperties()) {
                IntegerField integerField = new(property.Name) {
                    value = property.Getter()
                };
                integerField.RegisterValueChangedCallback(callback => {
                    property.Setter(callback.newValue);
                    WritePropertyGroup(preferencesKey, propertyGroup);
                });
                contentContainer.Add(integerField);
            }
            foreach (FloatProperty property in propertyGroup.GetFloatProperties()) {
                FloatField floatField = new(property.Name) {
                    value = property.Getter()
                };
                floatField.RegisterValueChangedCallback(callback => {
                    property.Setter(callback.newValue);
                    WritePropertyGroup(preferencesKey, propertyGroup);
                });
                contentContainer.Add(floatField);
            }
        }

        private void InitializeProviderFoldouts(Configuration configurationInstance) {
            Dictionary<Type, Type[]> rootCollection = Mapping.RootModules;
            Type[] sortedRootInterfaces = rootCollection.Keys.ToArray();
            Array.Sort(sortedRootInterfaces, (a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));
            foreach (Type rootInterface in sortedRootInterfaces) {
                string rootInterfaceName = rootInterface.Name;
                SettingsFoldout rootFoldout = new(
                    overrideValue: preferencesEditor.IsOverrideModuleEnabled(SelectedConfigurationName, rootInterfaceName),
                    contentVisible: preferencesEditor.GetPreferencesBool(SelectedConfigurationName, rootInterface.Name, Naming.Visible),
                    overrideSetter: (value) => { }
                );
                rootFoldout.name = rootInterfaceName.TrimInterfacePrefix();
                rootFoldout.Text = rootInterfaceName.TrimInterfacePrefix();
                rootFoldout.OnContentVisibleChange += () => {
                    preferencesEditor.SetModuleBool(SelectedConfigurationName, rootInterfaceName, Naming.Visible, rootFoldout.ContentVisible);
                };
                rootFoldout.OnOverrideValueChange += () => {
                    preferencesEditor.SetModuleBool(SelectedConfigurationName, rootInterfaceName, Naming.Override, rootFoldout.OverrideValue);
                    UpdateProviderFoldoutContent(rootFoldout);
                };
                if (configurationInstance.ReadOnly) {
                    rootFoldout.HideOverrideToggle();
                }
                foldoutTypeMapping[rootFoldout] = rootInterface;
                UpdateProviderFoldoutContent(rootFoldout);
                ProvidersContainer.Add(rootFoldout);
            }
        }

        private void Reset() {
            foldoutTypeMapping.Clear();
            ConfigurationContainer.Clear();
            ProvidersContainer.Clear();
        }

        private void UpdateProviderFoldoutContent(SettingsFoldout settingsFoldout) {
            settingsFoldout.Clear();
            Type rootInterface = foldoutTypeMapping[settingsFoldout];
            Type[] rootModules = Mapping.RootModules[rootInterface];
            if (rootModules.Length == 0) {
                Type providerEnumType = Mapping.ProviderEnums[rootInterface];
                CreateProviderEnumField(settingsFoldout, rootInterface, providerEnumType);
            }
            else {
                foreach (Type interfaceModule in rootModules) {
                    Type providerEnumType = Mapping.ProviderEnums[interfaceModule];
                    CreateProviderEnumField(settingsFoldout, rootInterface, providerEnumType);
                }
            }
        }

        private void CreateProviderEnumField(SettingsFoldout settingsFoldout, Type rootInterface, Type providerEnumType) {
            string preferencesProviderName = preferencesEditor.GetModuleString(SelectedConfigurationName, rootInterface.Name, providerEnumType.Name);
            string defaultProviderName = preferencesEditor.GetDefaultString(SelectedConfigurationName, rootInterface.Name, providerEnumType.Name);
            Enum defaultProviderEnum = defaultProviderName.ToEnumOrDefault(providerEnumType);
            Enum providerTypeEnum = preferencesProviderName.ToEnumOrDefault(providerEnumType, defaultProviderEnum);
            CreateEnumField(
                name: providerEnumType.Name,
                initialValue: providerTypeEnum,
                parent: settingsFoldout,
                onValueChanged: (newValue) => {
                    preferencesEditor.SetModuleString(SelectedConfigurationName, rootInterface.Name, providerEnumType.Name, newValue.ToString());
                    UpdateProviderFoldoutContent(settingsFoldout);
                }
            );
            if (providerTypeEnum == null) {
                return;
            }
            bool returnDefault = !preferencesEditor.IsOverrideModuleEnabled(SelectedConfigurationName, rootInterface.Name);
            string providerTypeName = providerTypeEnum.ToString();
            if (Mapping.ProviderTypes.ContainsKey(providerTypeName) == false) {
                return;
            }
            Type providerType = Mapping.ProviderTypes[providerTypeName];
            if (MainFactory.GetProviderConfiguration(preferencesEditor, SelectedConfigurationName, providerType, returnDefault) is PropertyGroup providerConfiguration) {
                SettingsGroup settingsGroup = new();
                string preferencesKey = providerConfiguration.GetType().Name;
                CreatePropertyFields(settingsGroup, providerConfiguration, preferencesKey, false);
                settingsFoldout.Add(settingsGroup);
            }
        }

        private void CreateEnumField(string name, Enum initialValue, VisualElement parent, Action<Enum> onValueChanged) {
            EnumField enumField = new(name.InsertSpacing(), initialValue);
            enumField.Q<Label>().style.width = EnumFieldWidth;
            enumField.RegisterValueChangedCallback(callback => {
                onValueChanged((Enum)callback.newValue);
            });
            parent.Add(enumField);
        }

    }

}
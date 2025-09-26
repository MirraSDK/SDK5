using MirraGames.SDK.Common;
using MirraGames.SDK.SourceGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Editor {

    internal partial class ConfigurationsView : VisualElement {

        private readonly ConfigurationInspector configurationInspector;

        public ConfigurationsView(ConfigurationInspector configurationInspector) {
            this.configurationInspector = configurationInspector;
            VisualTreeReference reference = VisualTreeReference.Load(nameof(ConfigurationsView));
            VisualTreeAsset asset = reference.VisualTree;
            asset.CloneTree(this);
            style.flexGrow = 1;

            InitializeConfigurationSettings();
            InitializeConfigurations();
        }

        private VisualElement ContentContainer => this.Q<VisualElement>("ContentContainer");
        private List<HorizontalCard> HorizontalCards { get; } = new();
        private DropdownField EditorConfiguration => this.Q<DropdownField>("EditorConfiguration");
        private DropdownField BuildConfiguration => this.Q<DropdownField>("BuildConfiguration");

        private string SelectedConfigurationName {
            get => PackageTools.GetPrefsString(nameof(SelectedConfigurationName), nameof(EditorConfiguration));
            set => PackageTools.SetPrefsString(nameof(SelectedConfigurationName), value);
        }

        private ConfigurationType EditorConfigurationType {
            get {
                PreferencesEditor preferencesEditor = PreferencesEditor.CreateEditor();
                string configurationName = preferencesEditor.GetEditorConfigurationName();
                return configurationName.ToEnumOrDefault<ConfigurationType>(ConfigurationType.EditorConfiguration);
            }
            set {
                PreferencesEditor preferencesEditor = PreferencesEditor.CreateEditor();
                preferencesEditor.SetEditorConfigurationName(value.ToString());
            }
        }

        private ConfigurationType BuildConfigurationType {
            get {
                PreferencesEditor preferencesEditor = PreferencesEditor.CreateEditor();
                string configurationName = preferencesEditor.GetBuildConfigurationName();
                return configurationName.ToEnumOrDefault<ConfigurationType>(ConfigurationType.FallbackConfiguration);
            }
            set {
                PreferencesEditor preferencesEditor = PreferencesEditor.CreateEditor();
                preferencesEditor.SetBuildConfigurationName(value.ToString());
            }
        }

        private void InitializeConfigurationSettings() {
            Array configurationChoices = Enum.GetValues(typeof(ConfigurationType));
            List<string> configurationChoiceNames = configurationChoices.Cast<ConfigurationType>().Select(v => v.ToString()).ToList();
            EditorConfiguration.choices = configurationChoiceNames;
            EditorConfiguration.value = EditorConfigurationType.ToString();
            EditorConfiguration.RegisterValueChangedCallback(callback => {
                if (callback.newValue == callback.previousValue) {
                    return;
                }
                if (Enum.TryParse<ConfigurationType>(callback.newValue, out ConfigurationType result)) {
                    EditorConfigurationType = result;
                    ToolkitWindow.OnConfigurationChanged?.Invoke();
                }
            });
            BuildConfiguration.choices = configurationChoiceNames;
            BuildConfiguration.value = BuildConfigurationType.ToString();
            BuildConfiguration.RegisterValueChangedCallback(callback => {
                if (callback.newValue == callback.previousValue) {
                    return;
                }
                if (Enum.TryParse<ConfigurationType>(callback.newValue, out ConfigurationType result)) {
                    BuildConfigurationType = result;
                    ToolkitWindow.OnConfigurationChanged?.Invoke();
                }
            });
            ToolkitWindow.OnConfigurationChanged += () => {
                EditorConfiguration.value = EditorConfigurationType.ToString();
                BuildConfiguration.value = BuildConfigurationType.ToString();
            };
        }

        public void InitializeConfigurations() {
            foreach (Type configurationType in Mapping.Configurations.Values) {
                Configuration configurationInstance = Mapping.CreateConfigurationInstance(configurationType.Name);
                string hintText = string.Empty;
                if (configurationInstance.ReadOnly) {
                    hintText = "Read Only";
                }
                HorizontalCard horizontalCard = new() {
                    name = configurationType.Name,
                    HeaderText = configurationType.Name,
                    DescriptionText = configurationInstance.Description,
                    HintText = hintText
                };
                Texture2D configurationIconTexture = null;
                if (!string.IsNullOrEmpty(configurationInstance.IconName)) {
                    configurationIconTexture = PackageFiles.FindTextureAsset(configurationInstance.IconName);
                }
                if (configurationIconTexture.IsNullOrDestroyed()) {
                    horizontalCard.LetterText = configurationType.Name[..1].ToUpper();
                }
                else {
                    horizontalCard.Thumbnail.style.backgroundImage = new StyleBackground(configurationIconTexture);
                    horizontalCard.LetterText = string.Empty;
                }
                horizontalCard.RegisterCallback<ClickEvent>(callback => {
                    DeselectCards();
                    horizontalCard.Select();
                    configurationInspector.SelectConfiguration(configurationType);
                    SelectedConfigurationName = configurationType.Name;
                });
                HorizontalCards.Add(horizontalCard);
                ContentContainer.Add(horizontalCard);
            }
            try {
                Type lastConfigurationType = Mapping.Configurations[SelectedConfigurationName];
                configurationInspector.SelectConfiguration(lastConfigurationType);
                HorizontalCards.Where(x => x.name == SelectedConfigurationName).FirstOrDefault()?.Select();
            }
            catch {
                // Ignore failed attempt to select last configuration
            }
        }

        private void DeselectCards() {
            foreach (HorizontalCard card in HorizontalCards) {
                card.Deselect();
            }
        }

    }

}
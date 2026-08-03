using MirraGames.SDK.Common;
using MirraGames.SDK.SourceGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = MirraGames.SDK.Common.Logger;

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
        private Button ImportWebGLTemplatesButton => this.Q<Button>(nameof(ImportWebGLTemplatesButton));
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
        
        private void ImportWebGLTemplates(params string[] templateNames)
        {
            if (templateNames.Length == 0) return;
            string currentTemplateName = String.Empty;
            string lastWindowTitle = String.Empty;
            ImportWebGLTemplate(templateNames[0]);

            void WindowFocusChanged()
            {
                EditorWindow window = EditorWindow.focusedWindow;
                if (lastWindowTitle.Equals("Import Unity Package") && !currentTemplateName.Equals(String.Empty))
                {
                    EditorWindow.windowFocusChanged -= WindowFocusChanged;
                    ImportWebGLTemplateCompleted(currentTemplateName);
                }
                lastWindowTitle = window.titleContent.text;
            }

            void ImportWebGLTemplateCanceled(string templateName)
            {
                currentTemplateName = String.Empty;
                AssetDatabase.importPackageCancelled -= ImportWebGLTemplateCanceled;
            }
            void ImportWebGLTemplateFailed(string templateName, string errMsg)
            {
                currentTemplateName = String.Empty;
                AssetDatabase.importPackageFailed -= ImportWebGLTemplateFailed;
                ImportWebGLTemplateCompleted(templateName);
            }
            void ImportWebGLTemplateCompleted(string templateName)
            {
                currentTemplateName = String.Empty;
                AssetDatabase.importPackageCompleted -= ImportWebGLTemplateCompleted;
                int index = Array.IndexOf(templateNames, templateName);
                if (index < templateNames.Length-1)
                {
                    ImportWebGLTemplate(templateNames[index+1]);
                }
            }
            void ImportWebGLTemplate(string templateName)
            {
                currentTemplateName = templateName;
                EditorWindow.windowFocusChanged -= WindowFocusChanged;
                EditorWindow.windowFocusChanged += WindowFocusChanged;
                AssetDatabase.importPackageCompleted += ImportWebGLTemplateCompleted;
                AssetDatabase.importPackageFailed += ImportWebGLTemplateFailed;
                AssetDatabase.importPackageCancelled += ImportWebGLTemplateCanceled;
                AssetDatabase.ImportPackage(PackageFiles.GetWebGLTemplatePath(templateName), true);
            }
        }
        
        private void InitializeConfigurationSettings() {
            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.WebGL)
            {
                ImportWebGLTemplatesButton.style.display = DisplayStyle.Flex;
                ImportWebGLTemplatesButton.clicked += () =>
                {
                    ImportWebGLTemplates("MirraWebTemplate", "MirraYoutubeTemplate");
                };
            }
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
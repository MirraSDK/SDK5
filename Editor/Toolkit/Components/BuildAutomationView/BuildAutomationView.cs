using MirraGames.SDK.Common;
using MirraGames.SDK.SourceGenerator;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Editor {

    internal partial class BuildAutomationView : VisualElement {

        public BuildAutomationView() {
            VisualTreeReference reference = VisualTreeReference.Load(nameof(BuildAutomationView));
            VisualTreeAsset asset = reference.VisualTree;
            asset.CloneTree(this);
            style.flexGrow = 1;

            InitializeView();
            EditorApplication.update += UpdateValues;
        }

        private VisualElement UnityGroup => this.Q<VisualElement>("UnityGroup");
        private Button OpenBuildSettings => this.Q<Button>("OpenBuildSettings");
        private Button OpenPlayerSettings => this.Q<Button>("OpenPlayerSettings");

        private VisualElement WebGLSettingsGroup => this.Q<VisualElement>("WebGLSettingsGroup");
        private DropdownField EnableExceptions => this.Q<DropdownField>("EnableExceptions");
        private DropdownField CompressionFormat => this.Q<DropdownField>("CompressionFormat");
        private Toggle NameFilesAsHashes => this.Q<Toggle>("NameFilesAsHashes");
        private Toggle DataCaching => this.Q<Toggle>("DataCaching");
        private DropdownField DebugSymbols => this.Q<DropdownField>("DebugSymbols");
        private Toggle DecompressionFallback => this.Q<Toggle>("DecompressionFallback");

        private VisualElement WebGLOutputGroup => this.Q<VisualElement>("WebGLOutputGroup");
        private DropdownField BuildExportFormat => this.Q<DropdownField>("BuildExportFormat");
        private TextField BuildsFolderPath => this.Q<TextField>("BuildsFolderPath");
        private Button ResetBuildsFolder => this.Q<Button>("ResetBuildsFolder");
        private Button SelectBuildsFolder => this.Q<Button>("SelectBuildsFolder");
        private TextField BuildFileName => this.Q<TextField>("BuildFileName");
        private Button ResetFileName => this.Q<Button>("ResetFileName");

        private BuildExportFormat CurrentBuildExportFormat {
            get {
                string valueName = PackageTools.GetPrefsString(nameof(CurrentBuildExportFormat));
                return valueName.ToEnumOrDefault<BuildExportFormat>();
            }
        }

        private string DefaultBuildsFolderPath {
            get {
                return Path.Combine(PackageTools.ProjectPath, Naming.Builds).NormalizePath();
            }
        }

        private string CurrentBuildsFolderPath {
            get {
                return PackageTools.GetPrefsString(nameof(CurrentBuildsFolderPath), DefaultBuildsFolderPath);
            }
            set => PackageTools.SetPrefsString(nameof(CurrentBuildsFolderPath), value);
        }

        private string DefaultProjectName {
            get {
                return PlayerSettings.productName.ToSafeFileName("build");
            }
        }

        private string DefaultBuildFileName {
            get {
                return $"{DefaultProjectName}[#NUMBER]-mirraSDK[#VERSION]";
            }
        }

        private string CurrentBuildFileName {
            get {
                return PackageTools.GetPrefsString(nameof(CurrentBuildFileName), DefaultBuildFileName);
            }
            set => PackageTools.SetPrefsString(nameof(CurrentBuildFileName), value);
        }

        private VisualElement ConfigurationGroup => this.Q<VisualElement>("ConfigurationGroup");
        private DropdownField BuildConfiguration => this.Q<DropdownField>("BuildConfiguration");

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

        private VisualElement WebGLActionsGroup => this.Q<VisualElement>("WebGLActionsGroup");
        private Button Build => this.Q<Button>("Build");
        private Button BuildAndRun => this.Q<Button>("BuildAndRun");
        private Button OpenBuildsFolder => this.Q<Button>("OpenBuildsFolder");

        private void UpdateValues() {
            EnableExceptions.value = PlayerSettings.WebGL.exceptionSupport.ToString();
            CompressionFormat.value = PlayerSettings.WebGL.compressionFormat.ToString();
            NameFilesAsHashes.value = PlayerSettings.WebGL.nameFilesAsHashes;
            DataCaching.value = PlayerSettings.WebGL.dataCaching;
            DebugSymbols.value = PlayerSettings.WebGL.debugSymbolMode.ToString();
            DecompressionFallback.value = PlayerSettings.WebGL.decompressionFallback;
        }

        private void InitializeView() {
            // Unity
            OpenBuildSettings.clicked += () => {
                EditorWindow.GetWindow(typeof(BuildPlayerWindow));
            };
            OpenPlayerSettings.clicked += () => {
                SettingsService.OpenProjectSettings("Project/Player");
            };

            // WebGL Settings
            Array enableExceptionsChoices = Enum.GetValues(typeof(WebGLExceptionSupport));
            EnableExceptions.choices = enableExceptionsChoices.Cast<WebGLExceptionSupport>().Select(v => v.ToString()).ToList();
            EnableExceptions.RegisterValueChangedCallback(evt => {
                if (evt.newValue == evt.previousValue) return;
                if (Enum.TryParse<WebGLExceptionSupport>(evt.newValue, out WebGLExceptionSupport result)) {
                    PlayerSettings.WebGL.exceptionSupport = result;
                }
            });
            Array compressionFormatChoices = Enum.GetValues(typeof(WebGLCompressionFormat));
            CompressionFormat.choices = compressionFormatChoices.Cast<WebGLCompressionFormat>().Select(v => v.ToString()).ToList();
            CompressionFormat.RegisterValueChangedCallback(evt => {
                if (evt.newValue == evt.previousValue) return;
                if (Enum.TryParse<WebGLCompressionFormat>(evt.newValue, out WebGLCompressionFormat result)) {
                    PlayerSettings.WebGL.compressionFormat = result;
                }
            });
            NameFilesAsHashes.RegisterValueChangedCallback(evt => {
                if (evt.newValue == evt.previousValue) return;
                PlayerSettings.WebGL.nameFilesAsHashes = evt.newValue;
            });
            DataCaching.RegisterValueChangedCallback(evt => {
                if (evt.newValue == evt.previousValue) return;
                PlayerSettings.WebGL.dataCaching = evt.newValue;
            });
            Array debugSymbolsChoices = Enum.GetValues(typeof(WebGLDebugSymbolMode));
            DebugSymbols.choices = debugSymbolsChoices.Cast<WebGLDebugSymbolMode>().Select(v => v.ToString()).ToList();
            DebugSymbols.RegisterValueChangedCallback(evt => {
                if (evt.newValue == evt.previousValue) return;
                if (Enum.TryParse<WebGLDebugSymbolMode>(evt.newValue, out WebGLDebugSymbolMode result)) {
                    PlayerSettings.WebGL.debugSymbolMode = result;
                }
            });
            DecompressionFallback.RegisterValueChangedCallback(evt => {
                if (evt.newValue == evt.previousValue) return;
                PlayerSettings.WebGL.decompressionFallback = evt.newValue;
            });

            // WebGL Output
            Array buildExportFormatChoices = Enum.GetValues(typeof(BuildExportFormat));
            BuildExportFormat.choices = buildExportFormatChoices.Cast<BuildExportFormat>().Select(v => v.ToString()).ToList();
            BuildExportFormat.value = CurrentBuildExportFormat.ToString();
            BuildExportFormat.RegisterValueChangedCallback(evt => {
                if (evt.newValue == evt.previousValue) return;
                if (Enum.TryParse<BuildExportFormat>(evt.newValue, out BuildExportFormat result)) {
                    PackageTools.SetPrefsString(nameof(CurrentBuildExportFormat), result.ToString());
                }
            });
            BuildsFolderPath.value = CurrentBuildsFolderPath;
            BuildsFolderPath.RegisterValueChangedCallback(evt => {
                if (evt.newValue == evt.previousValue) return;
                CurrentBuildsFolderPath = evt.newValue;
            });
            ResetBuildsFolder.clicked += () => {
                CurrentBuildsFolderPath = DefaultBuildsFolderPath;
                BuildsFolderPath.value = DefaultBuildsFolderPath;
            };
            SelectBuildsFolder.clicked += () => {
                string selectedPath = EditorUtility.OpenFolderPanel("Select Builds Folder", CurrentBuildsFolderPath, "");
                if (!string.IsNullOrEmpty(selectedPath)) {
                    CurrentBuildsFolderPath = selectedPath.NormalizePath();
                    BuildsFolderPath.value = CurrentBuildsFolderPath;
                }
            };
            BuildFileName.value = CurrentBuildFileName;
            BuildFileName.RegisterValueChangedCallback(evt => {
                if (evt.newValue == evt.previousValue) return;
                CurrentBuildFileName = evt.newValue;
            });
            ResetFileName.clicked += () => {
                CurrentBuildFileName = DefaultBuildFileName;
                BuildFileName.value = DefaultBuildFileName;
            };

            // Configuration
            Array configurationChoices = Enum.GetValues(typeof(ConfigurationType));
            BuildConfiguration.choices = configurationChoices.Cast<ConfigurationType>().Select(v => v.ToString()).ToList();
            ToolkitWindow.OnConfigurationChanged += () => {
                BuildConfiguration.value = BuildConfigurationType.ToString();
            };
            BuildConfiguration.value = BuildConfigurationType.ToString();
            BuildConfiguration.RegisterValueChangedCallback(evt => {
                if (evt.newValue == evt.previousValue) return;
                if (Enum.TryParse<ConfigurationType>(evt.newValue, out ConfigurationType result)) {
                    BuildConfigurationType = result;
                    ToolkitWindow.OnConfigurationChanged?.Invoke();
                }
            });

            // WebGL Actions
            Build.clicked += () => {
                switch (CurrentBuildExportFormat) {
                    case Editor.BuildExportFormat.Folder: {
                        BuildFolder();
                        break;
                    }
                    case Editor.BuildExportFormat.UncompressedZip: {
                        BuildUncompressedZip();
                        break;
                    }
                }
            };
            BuildAndRun.clicked += () => {
                switch (CurrentBuildExportFormat) {
                    case Editor.BuildExportFormat.Folder: {
                        BuildAndRunFolder();
                        break;
                    }
                    case Editor.BuildExportFormat.UncompressedZip: {
                        BuildAndRunUncompressedZip();
                        break;
                    }
                }
            };
            OpenBuildsFolder.clicked += () => {
                string buildsFolderPath = CurrentBuildsFolderPath;
                if (!Directory.Exists(buildsFolderPath)) {
                    Directory.CreateDirectory(buildsFolderPath);
                }
                EditorUtility.RevealInFinder(buildsFolderPath + Path.AltDirectorySeparatorChar);
            };

            BuildTarget buildTarget = EditorUserBuildSettings.activeBuildTarget;
            switch (buildTarget) {
                case BuildTarget.WebGL: {
                    UnityGroup.style.display = DisplayStyle.Flex;
                    WebGLSettingsGroup.style.display = DisplayStyle.Flex;
                    WebGLOutputGroup.style.display = DisplayStyle.Flex;
                    ConfigurationGroup.style.display = DisplayStyle.Flex;
                    WebGLActionsGroup.style.display = DisplayStyle.Flex;
                    break;
                }
                default: {
                    UnityGroup.style.display = DisplayStyle.Flex;
                    WebGLSettingsGroup.style.display = DisplayStyle.None;
                    WebGLOutputGroup.style.display = DisplayStyle.None;
                    ConfigurationGroup.style.display = DisplayStyle.Flex;
                    WebGLActionsGroup.style.display = DisplayStyle.None;
                    break;
                }
            }
        }

        private string GetBuildFilePath() {
            DirectoryInfo buildsDirectory = new(CurrentBuildsFolderPath);
            if (!buildsDirectory.Exists) {
                buildsDirectory.Create();
            }
            string fileName = CurrentBuildFileName;
            int versionHandle = buildsDirectory.GetFileSystemInfos().Count() + 1;
            fileName = fileName.Replace("#NUMBER", versionHandle.ToString());
            fileName = fileName.Replace("#VERSION", MirraSDK.Version);
            string buildDirectoryPath = Path.Combine(buildsDirectory.FullName, fileName).NormalizePath();
            DirectoryInfo buildDirectory = new(buildDirectoryPath);
            if (!buildDirectory.Exists) {
                buildDirectory.Create();
            }
            return buildDirectory.FullName;
        }

        public BuildReport ExecuteBuildPipeline(BuildPlayerOptions buildPlayerOptions) {
            try {
                return BuildPipeline.BuildPlayer(buildPlayerOptions);
            }
            catch (Exception exception) {
                if (exception.Message.Contains("buildprogram run 6 times")) {
                    throw new InvalidOperationException($"Common Unity bug detected - try to start build again. Message: {exception.Message}");
                }
                throw exception;
            }
        }

        public void BuildFolder() {
            string buildFilePath = GetBuildFilePath();
            EditorUserBuildSettings.SetBuildLocation(BuildTarget.WebGL, buildFilePath);
            BuildPlayerOptions buildPlayerOptions = GetBuildPlayerOptions();
            ExecuteBuildPipeline(buildPlayerOptions);
            EditorUtility.RevealInFinder(buildFilePath);
        }

        public void BuildUncompressedZip() {
            string buildFilePath = GetBuildFilePath();
            EditorUserBuildSettings.SetBuildLocation(BuildTarget.WebGL, buildFilePath);
            BuildPlayerOptions buildPlayerOptions = GetBuildPlayerOptions();
            BuildReport buildReport = ExecuteBuildPipeline(buildPlayerOptions);
            if (buildReport.summary.result == BuildResult.Succeeded) {
                string zipPath = CompressFolder(buildFilePath, true);
                EditorUtility.RevealInFinder(zipPath);
            }
        }

        public void BuildAndRunFolder() {
            string buildFilePath = GetBuildFilePath();
            EditorUserBuildSettings.SetBuildLocation(BuildTarget.WebGL, buildFilePath);
            BuildPlayerOptions buildPlayerOptions = GetBuildPlayerOptions();
            buildPlayerOptions.options |= UnityEditor.BuildOptions.AutoRunPlayer;
            BuildReport buildReport = ExecuteBuildPipeline(buildPlayerOptions);
        }

        public void BuildAndRunUncompressedZip() {
            string buildFilePath = GetBuildFilePath();
            EditorUserBuildSettings.SetBuildLocation(BuildTarget.WebGL, buildFilePath);
            BuildPlayerOptions buildPlayerOptions = GetBuildPlayerOptions();
            buildPlayerOptions.options |= UnityEditor.BuildOptions.AutoRunPlayer;
            BuildReport buildReport = ExecuteBuildPipeline(buildPlayerOptions);
            if (buildReport.summary.result == BuildResult.Succeeded) {
                CompressFolder(buildFilePath, false);
            }
        }

        private static string CompressFolder(string folderPath, bool deleteFolder) {
            string zipPath = folderPath + ".zip";
            if (File.Exists(zipPath)) {
                File.Delete(zipPath);
            }
            using ZipArchive archive = ZipFile.Open(zipPath, ZipArchiveMode.Create);
            foreach (string filePath in Directory.EnumerateFiles(folderPath, "*", SearchOption.AllDirectories)) {
                string relativePath = Path.GetRelativePath(folderPath, filePath);
                relativePath = relativePath.NormalizePath();
                archive.CreateEntryFromFile(filePath, relativePath, CompressionLevel.NoCompression);
            }
            if (deleteFolder == true) {
                Directory.Delete(folderPath, true);
            }
            return zipPath;
        }

        private static BuildPlayerOptions GetBuildPlayerOptions() {
            Type defaultBuildMethodsType = typeof(BuildPlayerWindow.DefaultBuildMethods);
            MethodInfo getBuildPlayerOptionsInternalMethod = defaultBuildMethodsType.GetMethod(
                name: "GetBuildPlayerOptionsInternal",
                bindingAttr: BindingFlags.Static | BindingFlags.NonPublic,
                binder: null,
                types: new Type[] { typeof(bool), typeof(BuildPlayerOptions) },
                modifiers: null
            );
            if (getBuildPlayerOptionsInternalMethod != null) {
                // Invoke the method with 'askForBuildLocation' set to false.
                BuildPlayerOptions defaultOptions = new();
                return (BuildPlayerOptions)getBuildPlayerOptionsInternalMethod.Invoke(
                    null, new object[] { false, defaultOptions }
                );
            }
            throw new InvalidOperationException("Failed to get BuildPlayerOptions");
        }

    }

}
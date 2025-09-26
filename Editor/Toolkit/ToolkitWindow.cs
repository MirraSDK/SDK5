using MirraGames.SDK.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Editor {

    internal class ToolkitWindow : EditorWindow {

        private const string NavigationElementName = "navigation";
        private const string InspectorElementName = "inspector";
        private const string ViewportElementName = "viewport";

        private const string WaitOverlay = "WaitOverlay";
        private const string WaitLabel = "WaitLabel";

        internal static Action OnConfigurationChanged;

        [MenuItem(Naming.MirraSDK5 + "/Open Toolkit")]
        public static void Open() {
            ToolkitWindow window = GetWindow<ToolkitWindow>();
            window.titleContent.text = $"{Naming.MirraSDK5}";
            window.titleContent.image = EditorGUIUtility.isProSkin ?
                PackageFiles.FindTextureAsset("MirraSDK-Light-Small") :
                PackageFiles.FindTextureAsset("MirraSDK-Dark-Small");
            window.minSize = new(1200, 600);
        }

        private Label MirraSDKInfo => rootVisualElement.Q<Label>("MirraSDKInfo");
        private Label DebugInfo => rootVisualElement.Q<Label>("DebugInfo");
        private VisualElement UpdatesAvailable => rootVisualElement.Q<VisualElement>("UpdatesAvailable");

        private IEventAggregator eventAggregator;

        private void OnEnable() {
            eventAggregator = new EventAggregator();

            VisualTreeAsset windowBaseAsset = VisualTreeReference.Load("ToolkitWindowBase").VisualTree;
            VisualElement windowBaseElement = windowBaseAsset.CloneTree();
            windowBaseElement.style.flexGrow = 1;
            rootVisualElement.Add(windowBaseElement);

            OnConfigurationChanged += UpdateDebugInfo;
            UpdateDebugInfo();

            InitializeToolkitWaitOverlay();
            InitializeToolkitNavigation();
            InitializeToolkitInspector();
            InitializeToolkitViewport();
            RestoreNavigationState();
        }

        private void UpdateDebugInfo() {
            BuildTarget buildTarget = EditorUserBuildSettings.activeBuildTarget;
            string unityVersion = Application.unityVersion;
            string systemInfo = SystemInfo.operatingSystem;
            PreferencesEditor preferencesEditor = PreferencesEditor.CreateEditor();
            string configurationName = preferencesEditor.GetBuildConfigurationName();
            if (string.IsNullOrEmpty(configurationName)) {
                configurationName = "Missing preferences";
            }
            UpdatesAvailable.style.display = DisplayStyle.None;
            MirraSDKInfo.style.marginRight = 0.0f;
            MirraSDKInfo.text = $"{nameof(MirraSDK)} {MirraSDK.Version}";
            DebugInfo.text = $"| {buildTarget} | {configurationName} | Unity {unityVersion} | {systemInfo}";
        }

        private void OnDisable() {
            eventAggregator.Dispose();
        }

        private VisualElement waitOverlay;
        private Label waitLabel;

        private void InitializeToolkitWaitOverlay() {
            waitOverlay = rootVisualElement.Q<VisualElement>(WaitOverlay);
            waitLabel = waitOverlay.Q<Label>(WaitLabel);
            
            waitOverlay.style.display = DisplayStyle.None;
            EditorEventListener.OnCompilationStarted += ShowWaitOverlay;
            EditorEventListener.OnCompilationFinished += HideWaitOverlay;
        }

        private void ShowWaitOverlay() {
            waitOverlay.style.display = DisplayStyle.Flex;
        }

        private void HideWaitOverlay() {
            waitOverlay.style.display = DisplayStyle.None;
        }

        #region Navigation

        private enum NavigationItem {
            Configurations,
            PackageManager,
            BuildAutomation
        }

        private const float NavigationPaddingLeft = 15.0f;

        private static NavigationItem CurrentNavigationItem {
            get => (NavigationItem)EditorPrefs.GetInt($"{PackageTools.ProjectId}.{nameof(CurrentNavigationItem)}", 0);
            set => EditorPrefs.SetInt($"{PackageTools.ProjectId}.{nameof(CurrentNavigationItem)}", (int)value);
        }

        private NavigationList toolkitNavigation;

        private void InitializeToolkitNavigation() {
            toolkitNavigation = new NavigationList();
            VisualElement navigationElement = rootVisualElement.Q<VisualElement>(NavigationElementName);
            navigationElement.Add(toolkitNavigation);
        }

        private void RestoreNavigationState() {
            toolkitNavigation.Clear();
            List<string> navigationItemNames = Enum.GetNames(typeof(NavigationItem)).ToList();
            for (int x = 0; x < navigationItemNames.Count; x++) {
                toolkitNavigation.RegisterItem(navigationItemNames[x], NavigationPaddingLeft, OnNavigationItemClick);
            }
            NavigationItem navigationItem = CurrentNavigationItem;
            toolkitNavigation.HighlightItem((int)navigationItem);
            SwitchViewport(navigationItem);
        }

        private void OnNavigationItemClick(ClickEvent clickEvent) {
            VisualElement element = (VisualElement)clickEvent.currentTarget;
            string elementName = element.name;
            toolkitNavigation.HighlightItem(elementName);
            CurrentNavigationItem = (NavigationItem)Enum.Parse(typeof(NavigationItem), elementName);
            SwitchViewport(CurrentNavigationItem);
        }

        private void SwitchViewport(NavigationItem item) {
            ResetInspector();
            toolkitInspector.HeaderText = item.ToReadableString();
            ResetViewport();
            switch (item) {
                case NavigationItem.Configurations: {
                    ShowConfigurations();
                    break;
                }
                case NavigationItem.PackageManager: {
                    ShowPackageManager();
                    break;
                }
                case NavigationItem.BuildAutomation: {
                    ShowBuildAutomation();
                    break;
                }
            }
        }

        #endregion

        #region Toolkit Inspector

        private Inspector toolkitInspector;

        private void InitializeToolkitInspector() {
            VisualElement inspectorElement = rootVisualElement.Q<VisualElement>(InspectorElementName);
            toolkitInspector = new Inspector();
            inspectorElement.Add(toolkitInspector);
            toolkitInspector.Clear();
        }

        private void ResetInspector() {
            toolkitInspector.Clear();
            toolkitInspector.FooterVisible = false;
            toolkitInspector.FooterContainer.Clear();
        }

        #endregion

        #region Toolkit Viewport

        private VisualElement toolkitViewport;

        private void InitializeToolkitViewport() {
            toolkitViewport = rootVisualElement.Q<VisualElement>(ViewportElementName);
            toolkitViewport.Clear();
        }

        private void ResetViewport() {
            toolkitViewport.Clear();
        }

        #endregion

        #region Configurations

        private ConfigurationsView configurationsView;
        private ConfigurationInspector configurationInspector;

        private void ShowConfigurations() {
            configurationInspector ??= new ConfigurationInspector();
            configurationsView ??= new ConfigurationsView(configurationInspector);
            toolkitViewport.Add(configurationsView);
            toolkitInspector.Add(configurationInspector);
        }

        #endregion

        #region Package Manager

        private PackageManagerView packageManagerView;

        private void ShowPackageManager() {
            packageManagerView ??= new PackageManagerView();
            toolkitViewport.Add(packageManagerView);
        }

        #endregion

        #region Build Automation

        private BuildAutomationView buildAutomationView;

        private void ShowBuildAutomation() {
            buildAutomationView ??= new BuildAutomationView();
            toolkitViewport.Add(buildAutomationView);
        }

        #endregion

    }

}
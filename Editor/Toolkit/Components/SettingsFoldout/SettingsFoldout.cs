using MirraGames.SDK.Common;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Editor {

    public class SettingsFoldout : VisualElement {

        private const string CONTENT_CONTAINER_NAME = "foldout-content";
        private const string VIEW_BUTTON_NAME = "foldout-view-button";
        private const string TOGGLE_NAME = "foldout-override-toggle";
        private const string LABEL_NAME = "foldout-label";
        private const string VIEW_CONTAINER_NAME = "foldout-view-container";
        private const string STATUS_LABEL_NAME = "foldout-status";

        private const float CONTENT_ENABLED_OPACITY = 1.0f;
        private const float CONTENT_DISABLED_OPACITY = 0.75f;

        private static readonly Color CHECKMARK_VISIBLE = Color.white;
        private static readonly Color CHECKMARK_INVISIBLE = new(0, 0, 0, 0);

        public SettingsFoldout(bool overrideValue, Action<bool> overrideSetter, bool contentVisible) {
            VisualTreeAsset settingsFoldoutTree = VisualTreeReference.Load(nameof(OverrideFoldout)).VisualTree;
            settingsFoldoutTree.CloneTree(this);

            this.overrideSetter = overrideSetter;
            ContentVisible = contentVisible;
            OverrideValue = overrideValue;

            // Show or hide the content when clicking on the view button.
            ViewButton.RegisterCallback<ClickEvent>(callback => {
                // Make sure that click event is not triggered when clicking on the toggle.
                if (callback.target == ViewButton) {
                    ContentVisible = !ContentVisible;
                }
            });

            // Bind the toggle value to the override value.
            Toggle.RegisterValueChangedCallback(callback => {
                OverrideValue = callback.newValue;
            });
        }

        private readonly Action<bool> overrideSetter;

        private VisualElement ViewButton => this.Q<VisualElement>(VIEW_BUTTON_NAME);
        private Toggle Toggle => this.Q<Toggle>(TOGGLE_NAME);
        private Label Label => this.Q<Label>(LABEL_NAME);
        private VisualElement ViewContainer => this.Q<VisualElement>(VIEW_CONTAINER_NAME);
        private Label StatusLabel => this.Q<Label>(STATUS_LABEL_NAME);

        public override VisualElement contentContainer {
            get => this.Q<VisualElement>(CONTENT_CONTAINER_NAME);
        }

        private VisualElement ToggleView {
            get => this.Q<VisualElement>(nameof(ToggleView));
        }

        public void HideOverrideToggle() {
            ToggleView.style.display = DisplayStyle.None;
            this.Q<VisualElement>("toggle-container").style.marginTop = 5;
        }

        public string Text {
            get => Label.text;
            set => Label.text = value;
        }

        public bool ContentVisible {
            get => ViewContainer.style.display == DisplayStyle.Flex;
            set => ViewContainer.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
        }

        public bool OverrideValue {
            get => Toggle.@value;
            set {
                Toggle.@value = value;

                // Make content grayed out when override is enabled.
                contentContainer.style.opacity = value ? CONTENT_ENABLED_OPACITY : CONTENT_DISABLED_OPACITY;

                // Make content unclickable when override is disabled.
                contentContainer.SetEnabled(value);

                // Change status label based on the override value.
                StatusLabel.text = value ? "(custom)" : "";

                // Save the override value.
                overrideSetter?.Invoke(value);

                // Change custom checkmark visibility.
                Toggle.Q<VisualElement>("unity-checkmark").style.unityBackgroundImageTintColor = value ? CHECKMARK_VISIBLE : CHECKMARK_INVISIBLE;
            }
        }

        public event Action OnContentVisibleChange {
            add => ViewButton.RegisterCallback<ClickEvent>(callback => value.Invoke());
            remove => ViewButton.UnregisterCallback<ClickEvent>(callback => value.Invoke());
        }

        public event Action OnOverrideValueChange {
            add => Toggle.RegisterValueChangedCallback(callback => value.Invoke());
            remove => Toggle.UnregisterValueChangedCallback(callback => value.Invoke());
        }

    }

}
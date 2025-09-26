using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Common {

    public class OverrideFoldout : VisualElement {

        private const string TEXT_NAME = "Text";
        private const string TEXT_DEFAULT = "Override Foldout";

        private const string CONTENT_VISIBLE_NAME = "Content-Visible";
        private const bool CONTENT_VISIBLE_DEFAULT = false;

        private const string OVERRIDE_VALUE_NAME = "Override-Value";
        private const bool OVERRIDE_VALUE_DEFAULT = false;

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

        public OverrideFoldout() {
            VisualTreeAsset overrideFoldoutTree = VisualTreeReference.Load(nameof(OverrideFoldout)).VisualTree;
            overrideFoldoutTree.CloneTree(this);

            // Initial values.
            ContentVisible = false;
            OverrideValue = false;

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

        public new class UxmlTraits : VisualElement.UxmlTraits {

            private readonly UxmlStringAttributeDescription textProperty = new() {
                name = TEXT_NAME,
                defaultValue = TEXT_DEFAULT
            };

            private readonly UxmlBoolAttributeDescription contentVisibleProperty = new() {
                name = CONTENT_VISIBLE_NAME,
                defaultValue = CONTENT_VISIBLE_DEFAULT
            };

            private readonly UxmlBoolAttributeDescription toggleValueProperty = new() {
                name = OVERRIDE_VALUE_NAME,
                defaultValue = OVERRIDE_VALUE_DEFAULT
            };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc) {
                base.Init(ve, bag, cc);
                var ate = ve as OverrideFoldout;
                ate.Text = textProperty.GetValueFromBag(bag, cc);
                ate.ContentVisible = contentVisibleProperty.GetValueFromBag(bag, cc);
                ate.OverrideValue = toggleValueProperty.GetValueFromBag(bag, cc);
            }

        }

        public override VisualElement contentContainer {
            get => this.Q<VisualElement>(CONTENT_CONTAINER_NAME);
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
                StatusLabel.text = value ? "(custom)" : "(default)";

                // Change custom checkmark visibility.
                Toggle.Q<VisualElement>("unity-checkmark").style.unityBackgroundImageTintColor = value ? CHECKMARK_VISIBLE : CHECKMARK_INVISIBLE;
            }
        }

        public event Action OnOverrideValueChanged {
            add => Toggle.RegisterValueChangedCallback(callback => value.Invoke());
            remove => Toggle.UnregisterValueChangedCallback(callback => value.Invoke());
        }

        private VisualElement ViewButton => this.Q<VisualElement>(VIEW_BUTTON_NAME);
        private Toggle Toggle => this.Q<Toggle>(TOGGLE_NAME);
        private Label Label => this.Q<Label>(LABEL_NAME);
        private VisualElement ViewContainer => this.Q<VisualElement>(VIEW_CONTAINER_NAME);
        private Label StatusLabel => this.Q<Label>(STATUS_LABEL_NAME);

    }

}
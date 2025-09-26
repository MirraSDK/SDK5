using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Common {

    public class BoolField : VisualElement {

        private static readonly Color CheckmarkVisibleColor = Color.white;
        private static readonly Color CheckmarkInvisibleColor = new(0, 0, 0, 0);

        public BoolField() {
            VisualTreeAsset rootAsset = VisualTreeReference.Load(nameof(BoolField)).VisualTree;
            rootAsset.CloneTree(this);
            Toggle.RegisterCallback<ClickEvent>(callback => OnToggleClick?.Invoke());
        }

        public string Name {
            get => Toggle.label;
            set => Toggle.label = value;
        }

        public bool Value {
            get => this.Q<Toggle>().value;
            set {
                Toggle.value = value;
                RefreshCheckmark(value);
            }
        }

        public event Action OnToggleClick;

        private Toggle Toggle => this.Q<Toggle>();

        private void RefreshCheckmark(bool value) {
            Color color = value ? CheckmarkVisibleColor : CheckmarkInvisibleColor;
            Toggle.Q<VisualElement>(Naming.USS.UnityCheckmark).style.unityBackgroundImageTintColor = color;
        }

    }

}
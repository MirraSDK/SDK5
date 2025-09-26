using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Common {

    public class FeatureBoolean : VisualElement {

        private static readonly Color CheckmarkVisible = Color.white;
        private static readonly Color CheckmarkHidden = new(0, 0, 0, 0);

        public FeatureBoolean(string text, Func<bool> getter) {
            VisualTreeAsset featureBooleanTree = VisualTreeReference.Load(nameof(FeatureBoolean)).VisualTree;
            featureBooleanTree.CloneTree(this);

            Getter = getter;
            BooleanText = text;
            BooleanValue = default;
        }

        private Func<bool> Getter { get; }

        public string BooleanText {
            get => this.Q<Label>().text;
            set => this.Q<Label>().text = value;
        }

        public bool BooleanValue {
            get => this.Q<Toggle>().@value;
            set {
                Toggle toggle = this.Q<Toggle>();
                toggle.@value = value;
                toggle.Q<VisualElement>(Naming.USS.UnityCheckmark).style.unityBackgroundImageTintColor = value ? CheckmarkVisible : CheckmarkHidden;
            }
        }

        public void UpdateValue() {
            BooleanValue = Getter();
        }

    }

}
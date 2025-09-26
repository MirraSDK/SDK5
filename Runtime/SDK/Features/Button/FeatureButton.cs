using System;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Common {

    public class FeatureButton : VisualElement {

        private const string BUTTON_TEXT_NAME = "button-text";
        private const string BUTTON_TEXT_DEFAULT = "Button";

        public FeatureButton(string text, Action onClick) {
            VisualTreeAsset featureButtonAsset = VisualTreeReference.Load(nameof(FeatureButton)).VisualTree;
            featureButtonAsset.CloneTree(this);

            this.onClick = onClick;
            ButtonText = text;
            this.Q<Button>().clicked += onClick;
        }

        private readonly Action onClick;

        public string ButtonText {
            get => this.Q<Button>().text;
            set => this.Q<Button>().text = value;
        }

    }

}
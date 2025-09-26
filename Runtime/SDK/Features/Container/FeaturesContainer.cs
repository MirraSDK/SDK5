using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Common {

    public class FeaturesContainer : VisualElement {

        private const string HEADER_TEXT_ELEMENT = "header-text";
        private const string PROVIDER_TEXT_ELEMENT = "provider-text";
        private const string CONTENT_CONTAINER_ELEMENT = "content-container";

        public new class UxmlFactory : UxmlFactory<FeaturesContainer> { }

        protected List<FeatureBoolean> featureBooleanList = new();
        protected List<FeatureString> featureStringList = new();
        protected List<FeatureButton> featureButtonList = new();

        public FeaturesContainer() {
            VisualTreeAsset featuresContainerAsset = VisualTreeReference.Load(nameof(FeaturesContainer)).VisualTree;
            featuresContainerAsset.CloneTree(this);
        }

        public string Name {
            get => this.Q<Label>(HEADER_TEXT_ELEMENT).text;
            set => this.Q<Label>(HEADER_TEXT_ELEMENT).text = value;
        }

        public string Provider {
            get => this.Q<Label>(PROVIDER_TEXT_ELEMENT).text;
            set => this.Q<Label>(PROVIDER_TEXT_ELEMENT).text = value;
        }

        public override VisualElement contentContainer {
            get => this.Q<VisualElement>(CONTENT_CONTAINER_ELEMENT);
        }

        protected void SetInfo(string moduleName, string facadeInterfaceName, string providerTypeName) {
            Name = moduleName;
            if (!Application.isPlaying) {
                return;
            }
            string configurationName = MirraSDK.Preferences.GetConfigurationName();
            string providerKey = Naming.Key(facadeInterfaceName, providerTypeName);
            string providerName = MirraSDK.Preferences.GetPreferenceGroup(configurationName).GetString(providerKey);
            if (string.IsNullOrEmpty(providerName)) {
                providerName = MirraSDK.Preferences.GetDefaultValueGroup(configurationName).GetString(providerKey);
            }
            Provider = providerName;
        }

        public void UpdateValues() {
            if (!Application.isPlaying) {
                return;
            }
            foreach (FeatureBoolean featureBoolean in featureBooleanList) {
                featureBoolean.UpdateValue();
            }
            foreach (FeatureString featureString in featureStringList) {
                featureString.UpdateValue();
            }
        }

        protected FeatureBoolean CreateBoolean(string text, Func<bool> getter) {
            FeatureBoolean featureBoolean = new(text, getter);
            contentContainer.Add(featureBoolean);
            featureBooleanList.Add(featureBoolean);
            return featureBoolean;
        }

        protected FeatureString CreateString(string text, Func<string> getter) {
            FeatureString featureString = new(text, getter);
            contentContainer.Add(featureString);
            featureStringList.Add(featureString);
            return featureString;
        }

        protected FeatureButton CreateButton(string text, Action onClick) {
            FeatureButton featureButton = new(text, onClick);
            contentContainer.Add(featureButton);
            featureButtonList.Add(featureButton);
            return featureButton;
        }

    }

}
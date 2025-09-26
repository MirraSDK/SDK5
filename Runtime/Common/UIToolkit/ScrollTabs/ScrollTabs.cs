using System.Collections.Generic;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Common {

    public class ScrollTabs : VisualElement {

        private const string CONTENT_CONTAINER_ELEMENT = "content-container";
        private const string BUTTONS_CONTAINER_ELEMENT = "buttons-container";

        private const string SELECTED_CLASS = "selected";

        public ScrollTabs() {
            VisualTreeAsset scrollTabsTree = VisualTreeReference.Load(nameof(ScrollTabs)).VisualTree;
            scrollTabsTree.CloneTree(this);
            style.flexGrow = 1;

            // Reset the buttons container.
            ButtonsContainer.Clear();
        }

        protected VisualElement ButtonsContainer {
            get => this.Q<VisualElement>(BUTTONS_CONTAINER_ELEMENT);
        }

        public override VisualElement contentContainer {
            get => this.Q<VisualElement>(CONTENT_CONTAINER_ELEMENT);
        }

        public VisualElement this[string tabName] {
            get => tabContentCollection.GetValueOrDefault(tabName).contentContainer;
        }

        private VisualElement selectedTab;
        private readonly Dictionary<string, ScrollView> tabContentCollection = new();

        public VisualElement CreateTab(string tabName) {
            // Create a new tab button.
            Button button = new() {
                name = tabName,
                text = tabName
            };
            if (ButtonsContainer.childCount == 0) {
                button.style.marginLeft = 0;
                button.AddToClassList(SELECTED_CLASS);
                selectedTab = button;
            }
            button.RegisterCallback<ClickEvent>(OnTabClick);
            ButtonsContainer.Add(button);

            // Create a new tab content scroll view.
            ScrollView tabContent = new() {
                name = tabName
            };
            tabContentCollection.Add(tabName, tabContent);

            // Return the tab content scroll view.
            return tabContent;
        }

        public virtual void SelectTab(string tabName) {
            // Highlight the selected tab button.
            selectedTab.RemoveFromClassList(SELECTED_CLASS);
            VisualElement element = ButtonsContainer.Q(tabName);
            element.AddToClassList(SELECTED_CLASS);
            selectedTab = element;

            // Clear content container.
            contentContainer.Clear();

            // Add selected tab content to the content container.
            ScrollView tabContent = tabContentCollection.GetValueOrDefault(element.name);
            contentContainer.Add(tabContent);
        }

        private void OnTabClick(ClickEvent callback) {
            VisualElement element = callback.target as VisualElement;
            SelectTab(element.name);
        }

    }

}
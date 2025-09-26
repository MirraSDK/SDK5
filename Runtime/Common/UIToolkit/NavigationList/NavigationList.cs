using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Common {

    public class NavigationList : VisualElement {

        private const string ITEM_SELECTED_ACTIVE = "item-selected-active";
        private const string ITEM_SELECTED_INACTIVE = "item-selected-inactive";
        private const string ITEM_ICON_ELEMENT = "item-icon";

        public NavigationList() {
            VisualTreeAsset navigationItemTree = VisualTreeReference.Load("NavigationItem").VisualTree;
            this.navigationItemTree = navigationItemTree;
            style.flexGrow = 1;
            // Make the navigation list focusable.
            focusable = true;
            // Register focus events.
            RegisterCallback<FocusInEvent>(OnFocusIn);
            RegisterCallback<FocusOutEvent>(OnFocusOut);
        }

        private bool isFocused;
        private string selectedItem;
        private readonly VisualTreeAsset navigationItemTree;
        private readonly Dictionary<string, Action<ClickEvent>> itemClickEvents = new();

        public void RegisterItem(string name, float paddingLeft, Action<ClickEvent> onItemClick) {
            RegisterItem(name, null, paddingLeft, onItemClick);
        }

        public void RegisterItem(string name, Texture2D icon, float paddingLeft, Action<ClickEvent> onItemClick) {
            VisualElement itemElement = navigationItemTree.CloneTree();
            itemElement.name = name;
            // Item element text.
            Label itemLabel = itemElement.Q<Label>();
            itemLabel.text = name.InsertSpacing();
            // Item element icon.
            VisualElement imageElement = itemElement.Q<VisualElement>(ITEM_ICON_ELEMENT);
            if (icon != null) {
                imageElement.style.backgroundImage = icon;
            }
            else {
                imageElement.style.display = DisplayStyle.None;
            }
            // Item element padding.
            itemElement.style.paddingLeft = paddingLeft;
            // Register item click event.
            itemClickEvents.Add(name, onItemClick);
            itemElement.RegisterCallback<ClickEvent>(OnItemClick);
            // Add item to the base.
            hierarchy.Add(itemElement);
        }

        public void HighlightItem(int index) {
            VisualElement element = this[index];
            HighlightItem(element.name);
        }

        public void HighlightItem(string name) {
            // Unhighlight previous item.
            if (string.IsNullOrEmpty(selectedItem) == false) {
                VisualElement selectedItemElement = this.Q<VisualElement>(selectedItem);
                selectedItemElement.RemoveFromClassList(ITEM_SELECTED_ACTIVE);
                selectedItemElement.RemoveFromClassList(ITEM_SELECTED_INACTIVE);
            }
            // Highlight new item.
            if (string.IsNullOrEmpty(name) == true) return;
            VisualElement newSelectedItem = this.Q<VisualElement>(name);
            newSelectedItem.AddToClassList(isFocused ? ITEM_SELECTED_ACTIVE : ITEM_SELECTED_INACTIVE);
            selectedItem = name;
        }

        private void OnItemClick(ClickEvent clickEvent) {
            VisualElement element = (VisualElement)clickEvent.currentTarget;
            string elementName = element.name;
            // Highlight clicked item.
            HighlightItem(elementName);
            // Invoke set event considering selection has changed.
            itemClickEvents[elementName]?.Invoke(clickEvent);
        }

        private void OnFocusIn(FocusInEvent focusEvent) {
            isFocused = true;
            HighlightItem(selectedItem);
        }

        private void OnFocusOut(FocusOutEvent focusEvent) {
            isFocused = false;
            HighlightItem(selectedItem);
        }

    }

}
using UnityEngine.UIElements;

namespace MirraGames.SDK.Common {

    public static class VisualElementExtensions {

        public static void Show(this VisualElement element) {
            element.style.display = DisplayStyle.Flex;
        }

        public static void Hide(this VisualElement element) {
            element.style.display = DisplayStyle.None;
        }

    }

}
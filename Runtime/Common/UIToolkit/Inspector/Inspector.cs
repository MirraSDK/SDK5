using UnityEngine.UIElements;

namespace MirraGames.SDK.Common {

    public class Inspector : VisualElement {

        private const string INSPECTOR_CONTENT_ELEMENT = "inspector-content";
        private const string FOOTER_CONTENT_ELEMENT = "footer-content";

        public Inspector() {
            VisualTreeAsset inspectorTree = VisualTreeReference.Load(nameof(Inspector)).VisualTree;
            inspectorTree.CloneTree(this);
            style.flexGrow = 1;
            HeaderText = string.Empty;
            FooterVisible = false;
        }

        public override VisualElement contentContainer {
            get => this.Q<VisualElement>(INSPECTOR_CONTENT_ELEMENT);
        }

        public VisualElement FooterContainer {
            get => this.Q<VisualElement>(FOOTER_CONTENT_ELEMENT);
        }

        public bool FooterVisible {
            get => FooterContainer.visible;
            set => FooterContainer.visible = value;
        }

        public string HeaderText {
            get => this.Q<Label>().text;
            set => this.Q<Label>().text = value.InsertSpacing();
        }

    }

}
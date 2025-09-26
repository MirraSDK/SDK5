using UnityEngine.UIElements;

namespace MirraGames.SDK.Common {

    public class HorizontalCard : VisualElement {

        private const string HEADER_TEXT_ELEMENT = "header-text";
        private const string DESCRIPTION_TEXT_ELEMENT = "description-text";
        private const string THUMBNAIL_ELEMENT = "thumbnail";
        private const string LETTER_ELEMENT = "letter";
        private const string HINT_TEXT_ELEMENT = "hint-text";

        public HorizontalCard() {
            VisualTreeAsset horizontalCardTree = VisualTreeReference.Load(nameof(HorizontalCard)).VisualTree;
            horizontalCardTree.CloneTree(this);

            Deselect();
        }

        public string HeaderText {
            get => this.Q<Label>(HEADER_TEXT_ELEMENT).text;
            set => this.Q<Label>(HEADER_TEXT_ELEMENT).text = value;
        }

        public string DescriptionText {
            get => this.Q<Label>(DESCRIPTION_TEXT_ELEMENT).text;
            set => this.Q<Label>(DESCRIPTION_TEXT_ELEMENT).text = value;
        }

        public string LetterText {
            get => this.Q<Label>(LETTER_ELEMENT).text;
            set => this.Q<Label>(LETTER_ELEMENT).text = value;
        }

        public string HintText {
            get => this.Q<Label>(HINT_TEXT_ELEMENT).text;
            set => this.Q<Label>(HINT_TEXT_ELEMENT).text = value;
        }

        public VisualElement Thumbnail {
            get => this.Q<VisualElement>(THUMBNAIL_ELEMENT);
        }

        public void Select() {
            this.RemoveFromClassList("normal");
            this.AddToClassList("selected");
        }

        public void Deselect() {
            this.RemoveFromClassList("selected");
            this.AddToClassList("normal");
        }

    }

}
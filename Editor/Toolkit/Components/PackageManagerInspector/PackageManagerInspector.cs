using MirraGames.SDK.Common;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Editor
{
    internal partial class PackageManagerInspector : VisualElement
    {
        public PackageManagerInspector()
        {
            VisualTreeReference reference = VisualTreeReference.Load(nameof(PackageManagerInspector));
            VisualTreeAsset asset = reference.VisualTree;
            asset.CloneTree(this);

            DescriptionLabel.text = Naming.Dash;
            ReadmeLabel.text = Naming.Dash;
        }

        public Label DescriptionLabel
        {
            get => this.Q<Label>(nameof(DescriptionLabel));
        }

        public Label ReadmeLabel
        {
            get => this.Q<Label>(nameof(ReadmeLabel));
        }
    }
}
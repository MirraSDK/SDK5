using MirraGames.SDK.Common;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Editor {

    internal partial class PackageManagerView : VisualElement {

        public PackageManagerView() {
            VisualTreeReference reference = VisualTreeReference.Load(nameof(PackageManagerView));
            VisualTreeAsset asset = reference.VisualTree;
            asset.CloneTree(this);
            style.flexGrow = 1;

            HorizontalCard mirraSDK = new() {
                HeaderText = "MirraSDK 5.0.0",
                DescriptionText = "com.romanlee17.mirrasdk",
                LetterText = "M",
                HintText = "Installed"
            };
            HorizontalCard playgama = new() {
                HeaderText = "Playgama 1.24.0",
                DescriptionText = "com.mirrasdk.playgama",
                LetterText = "P",
                HintText = "Installed"
            };
            HorizontalCard crazyGamesXSolla = new() {
                HeaderText = "CrazyGames XSolla 1.2.0",
                DescriptionText = "com.mirrasdk.crazygamesxsolla",
                LetterText = "X",
                HintText = ""
            };
            HorizontalCard ruStore = new() {
                HeaderText = "RuStore 1.0.0",
                DescriptionText = "com.mirrasdk.rustore",
                LetterText = "R",
                HintText = ""
            };
            contentContainer.Add(mirraSDK);
            contentContainer.Add(playgama);
            contentContainer.Add(crazyGamesXSolla);
            contentContainer.Add(ruStore);
        }

        private new VisualElement contentContainer {
            get => this.Q<VisualElement>("contentContainer");
        }

    }

}
using MirraGames.SDK.Common;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Editor {

    public class SettingsGroup : VisualElement {

        public SettingsGroup() {
            VisualTreeAsset settingsFoldoutTree = VisualTreeReference.Load(nameof(SettingsGroup)).VisualTree;
            settingsFoldoutTree.CloneTree(this);
        }

        public override VisualElement contentContainer => this.Q<VisualElement>(Naming.ContentContainer);

    }

}
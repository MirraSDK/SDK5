using UnityEngine;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Common {

    public class VisualTreeReference : ScriptableObject {

        public static VisualTreeReference Load(string name) {
            return Resources.Load<VisualTreeReference>($"{Naming.MirraSDK5}/{name}");
        }

        [field: SerializeField] public VisualTreeAsset VisualTree { get; internal set; }

    }

}
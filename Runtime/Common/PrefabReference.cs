using UnityEngine;

namespace MirraGames.SDK.Common {

    public class PrefabReference : ScriptableObject {

        public static PrefabReference Load(string name) {
            return Resources.Load<PrefabReference>($"{Naming.MirraSDK5}/{name}");
        }

        [field: SerializeField] public GameObject Prefab { get; internal set; }

    }

}
using MirraGames.SDK.SourceGenerator;
using MirraGames.SDK.Common;
using UnityEngine;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Features {

    public class Addressables_Features : FeaturesContainer {

        public new class UxmlFactory : UxmlFactory<Addressables_Features> { }

        public Addressables_Features() {
            SetInfo("Addressables", nameof(IAssets), nameof(AddressablesProvider));

            CreateButton(nameof(IAddressables.LoadAddressable), () => {
                MirraSDK.Assets.LoadAddressable<GameObject>("customAddressable", onSuccess: (asset) => {
                    Debug.Log($"Resolved asset: {asset.name}");
                },
                onError: () => {
                    Debug.LogError("Failed to resolve asset.");
                });
            });

            CreateButton(nameof(IAddressables.ReleaseAddressable), () => {
                MirraSDK.Assets.ReleaseAddressable("customAddressable");
            });
        }

    }

}
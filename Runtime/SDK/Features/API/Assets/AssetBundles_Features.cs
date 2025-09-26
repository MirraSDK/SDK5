using MirraGames.SDK.SourceGenerator;
using MirraGames.SDK.Common;
using UnityEngine;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Features {

    public class AssetBundles_Features : FeaturesContainer {

        public new class UxmlFactory : UxmlFactory<AssetBundles_Features> { }

        public AssetBundles_Features() {
            SetInfo("Asset Bundles", nameof(IAssets), nameof(AssetBundlesProvider));

            CreateButton(nameof(IAssetBundles.LoadBundle), () => {
                MirraSDK.Assets.LoadBundle("customBundle", "https://example.com/customBundle", onSuccess: (assetBundle) => {
                    Debug.Log($"Resolved asset: {assetBundle.name}");
                },
                onError: () => {
                    Debug.LogError("Failed to resolve asset.");
                });
            });

            CreateButton(nameof(IAssetBundles.ReleaseBundle), () => {
                MirraSDK.Assets.ReleaseBundle("customBundle", unloadAllObjects: true);
            });
        }

    }

}
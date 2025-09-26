using MirraGames.SDK.Common;
using MirraGames.SDK.SourceGenerator;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Features {

    public class BannerFeatures : FeaturesContainer {

        public new class UxmlFactory : UxmlFactory<BannerFeatures> { }

        public BannerFeatures() {
            SetInfo("Banner", nameof(IAds), nameof(AdsProvider));

            CreateBoolean(nameof(IBanner.IsBannerReady), () => {
                return MirraSDK.Ads.IsBannerReady;
            });
            CreateBoolean(nameof(IBanner.IsBannerVisible), () => {
                return MirraSDK.Ads.IsBannerVisible;
            });
            CreateBoolean(nameof(IBanner.IsBannerAvailable), () => {
                return MirraSDK.Ads.IsBannerAvailable;
            });

            CreateButton(nameof(IBanner.InvokeBanner), () => {
                MirraSDK.Ads.InvokeBanner();
            });
            CreateButton(nameof(IBanner.RefreshBanner), () => {
                MirraSDK.Ads.RefreshBanner();
            });
            CreateButton(nameof(IBanner.DisableBanner), () => {
                MirraSDK.Ads.DisableBanner();
            });
        }

    }

}
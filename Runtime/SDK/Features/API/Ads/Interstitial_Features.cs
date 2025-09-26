using MirraGames.SDK.SourceGenerator;
using MirraGames.SDK.Common;
using System;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Features {

    internal class Interstitial_Features : FeaturesContainer {

        public new class UxmlFactory : UxmlFactory<Interstitial_Features> { }

        public Interstitial_Features() {
            SetInfo("Interstitial", nameof(IAds), nameof(AdsProvider));

            CreateBoolean(nameof(IInterstitial.IsInterstitialReady), () => {
                return MirraSDK.Ads.IsInterstitialReady;
            });
            CreateBoolean(nameof(IInterstitial.IsInterstitialVisible), () => {
                return MirraSDK.Ads.IsInterstitialVisible;
            });
            CreateBoolean(nameof(IInterstitial.IsInterstitialAvailable), () => {
                return MirraSDK.Ads.IsInterstitialAvailable;
            });
            CreateString(nameof(IInterstitial.GetLastInterstitialSuccess), () => {
                DateTime? dateTime = MirraSDK.Ads.GetLastInterstitialSuccess();
                if (dateTime == null) {
                    return "null";
                }
                return dateTime.HasValue ? dateTime.Value.ToString("HH:mm:ss") : "null";
            });
            CreateButton(nameof(IInterstitial.InvokeInterstitial), () => {
                MirraSDK.Ads.InvokeInterstitial(
                    onOpen: () => { },
                    onClose: (isSuccess) => { }
                );
            });
        }

    }

}
using System;
using MirraGames.SDK.Common;
using MirraGames.SDK.SourceGenerator;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Features {

    public class Rewarded_Features : FeaturesContainer {

        public new class UxmlFactory : UxmlFactory<Rewarded_Features> { }

        public Rewarded_Features() {
            SetInfo("Rewarded", nameof(IAds), nameof(AdsProvider));

            CreateBoolean(nameof(IRewarded.IsRewardedReady), () => {
                return MirraSDK.Ads.IsRewardedReady;
            });
            CreateBoolean(nameof(IRewarded.IsRewardedVisible), () => {
                return MirraSDK.Ads.IsRewardedVisible;
            });
            CreateBoolean(nameof(IRewarded.IsRewardedAvailable), () => {
                return MirraSDK.Ads.IsRewardedAvailable;
            });

            CreateString(nameof(IRewarded.GetLastRewardedSuccess), () => {
                DateTime? dateTime = MirraSDK.Ads.GetLastRewardedSuccess();
                if (dateTime == null) {
                    return "null";
                }
                return dateTime.HasValue ? dateTime.Value.ToString("HH:mm:ss") : "null";
            });

            CreateButton(nameof(IRewarded.InvokeRewarded), () => {
                // TODO: log callbacks.
                MirraSDK.Ads.InvokeRewarded(
                    onOpen: () => { },
                    onClose: (success) => { }
                );
            });
        }

    }

}
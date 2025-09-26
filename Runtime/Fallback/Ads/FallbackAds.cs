using MirraGames.SDK.Common;
using System;

namespace MirraGames.SDK.Fallback {

    [Provider(typeof(IAds))]
    public class FallbackAds : CommonAds {

        public FallbackAds(IEventAggregator eventAggregator) : base(eventAggregator) {
            SetInitialized();
        }

        protected override void InvokeBannerImpl() {
            Logger.NotImplementedWarning(this, nameof(InvokeBannerImpl));
        }

        protected override void RefreshBannerImpl() {
            Logger.NotImplementedWarning(this, nameof(RefreshBannerImpl));
        }

        protected override void DisableBannerImpl() {
            Logger.NotImplementedWarning(this, nameof(DisableBannerImpl));
        }

        protected override void InvokeInterstitialImpl(Action onOpen = null, Action<bool> onClose = null) {
            Logger.NotImplementedWarning(this, nameof(InvokeInterstitialImpl));
            onClose?.Invoke(default);
        }

        protected override void InvokeRewardedImpl(Action onOpen = null, Action<bool> onClose = null, string rewardTag = null) {
            Logger.NotImplementedWarning(this, nameof(InvokeRewardedImpl));
            onClose?.Invoke(default);
        }

    }

}
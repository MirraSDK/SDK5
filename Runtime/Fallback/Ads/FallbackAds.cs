using MirraGames.SDK.Common;
using System;

namespace MirraGames.SDK.Fallback {

    [Provider(typeof(IAds))]
    public class FallbackAds : CommonAds {

        private readonly FallbackAds_Configuration configuration;

        public FallbackAds(FallbackAds_Configuration configuration, IEventAggregator eventAggregator) : base(eventAggregator) {
            this.configuration = configuration;
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

        public override bool IsRewardedAvailable => true;
        public override bool IsRewardedReady { get; protected set; } = true;

        protected override void InvokeRewardedImpl(Action onOpen = null, Action<bool> onClose = null, string rewardTag = null) {
            Logger.CreateText(this, "onClose", configuration.RewardsSuccess);
            onClose?.Invoke(configuration.RewardsSuccess);
        }

    }

}
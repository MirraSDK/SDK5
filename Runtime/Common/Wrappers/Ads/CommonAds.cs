using System;
using System.Collections.Generic;

namespace MirraGames.SDK.Common {

    [Awaitable, Wrapper]
    public abstract partial class CommonAds : IAds {

        protected readonly IEventAggregator eventAggregator;

        public CommonAds(IEventAggregator eventAggregator) {
            this.eventAggregator = eventAggregator;
        }

        // Banner

        public virtual bool IsBannerReady { get; protected set; } = false;
        public virtual bool IsBannerVisible { get; protected set; } = false;
        public virtual bool IsBannerAvailable { get; } = false;

        protected abstract void InvokeBannerImpl();
        protected abstract void RefreshBannerImpl();
        protected abstract void DisableBannerImpl();

        public void InvokeBanner() {
            Logger.CreateText(this, nameof(InvokeBanner));
            try {
                InvokeBannerImpl();
            }
            catch (Exception exception) {
                Logger.CreateError(this, nameof(InvokeBanner), exception);
            }
        }

        public void RefreshBanner() {
            Logger.CreateText(this, nameof(RefreshBanner));
            try {
                RefreshBannerImpl();
            }
            catch (Exception exception) {
                Logger.CreateError(this, nameof(RefreshBanner), exception);
            }
        }

        public void DisableBanner() {
            Logger.CreateText(this, nameof(DisableBanner));
            try {
                DisableBannerImpl();
            }
            catch (Exception exception) {
                Logger.CreateError(this, nameof(DisableBanner), exception);
            }
        }

        // Interstitial

        private DateTime? lastInterstitialSuccess = null;

        public virtual bool IsInterstitialReady { get; protected set; }
        public virtual bool IsInterstitialVisible { get; protected set; }
        public virtual bool IsInterstitialAvailable { get; }

        protected abstract void InvokeInterstitialImpl(Action onOpen = null, Action<bool> onClose = null);

        public DateTime? GetLastInterstitialSuccess() {
            return lastInterstitialSuccess;
        }

        public void InvokeInterstitial(Action onOpen = null, Action<bool> onClose = null) {
            Logger.CreateText(this, nameof(InvokeInterstitial));
            try {
                void onOpenCallback() {
                    Logger.CreateText(this, nameof(onOpenCallback));
                    onOpen?.Invoke();
                    PauseSourceEvent pauseSourceEvent = new(nameof(InvokeInterstitial), true);
                    eventAggregator.Publish(this, pauseSourceEvent);
                }
                void onCloseCallback(bool isSuccess) {
                    Logger.CreateText(this, nameof(onCloseCallback), isSuccess);
                    onClose?.Invoke(isSuccess);
                    if (isSuccess) {
                        lastInterstitialSuccess = DateTime.Now;
                    }
                    PauseSourceEvent pauseSourceEvent = new(nameof(InvokeInterstitial), false);
                    eventAggregator.Publish(this, pauseSourceEvent);
                }
                InvokeInterstitialImpl(onOpenCallback, onCloseCallback);
            }
            catch (Exception exception) {
                Logger.CreateError(this, nameof(InvokeInterstitial), exception);
            }
        }

        // Rewarded

        private DateTime? lastRewardedSuccess = null;
        private Dictionary<string, DateTime?> lastRewardedSuccessByTag = new();

        public virtual bool IsRewardedReady { get; protected set; }
        public virtual bool IsRewardedVisible { get; protected set; }
        public virtual bool IsRewardedAvailable { get; }

        protected abstract void InvokeRewardedImpl(Action onOpen = null, Action<bool> onClose = null, string rewardTag = null);

        public DateTime? GetLastRewardedSuccess(string rewardTag = null) {
            if (string.IsNullOrEmpty(rewardTag)) {
                return lastRewardedSuccess;
            }
            if (lastRewardedSuccessByTag.TryGetValue(rewardTag, out var dateTime)) {
                return dateTime;
            }
            return null;
        }

        public void InvokeRewarded(Action onOpen = null, Action<bool> onClose = null, string rewardTag = null) {
            Logger.CreateText(this, nameof(InvokeRewarded), rewardTag);
            try {
                void onOpenCallback() {
                    Logger.CreateText(this, nameof(onOpenCallback));
                    onOpen?.Invoke();
                    PauseSourceEvent pauseSourceEvent = new(nameof(InvokeRewarded), true);
                    eventAggregator.Publish(this, pauseSourceEvent);
                }
                void onCloseCallback(bool isSuccess) {
                    Logger.CreateText(this, nameof(onCloseCallback), isSuccess);
                    onClose?.Invoke(isSuccess);
                    if (isSuccess) {
                        lastRewardedSuccess = DateTime.Now;
                        if (!string.IsNullOrEmpty(rewardTag)) {
                            lastRewardedSuccessByTag[rewardTag] = lastRewardedSuccess;
                        }
                    }
                    PauseSourceEvent pauseSourceEvent = new(nameof(InvokeRewarded), false);
                    eventAggregator.Publish(this, pauseSourceEvent);
                }
                InvokeRewardedImpl(onOpenCallback, onCloseCallback, rewardTag);
            }
            catch (Exception exception) {
                Logger.CreateError(this, nameof(InvokeRewarded), exception, rewardTag);
            }
        }

    }

}
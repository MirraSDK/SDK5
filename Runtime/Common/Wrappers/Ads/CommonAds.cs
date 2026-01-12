using System;
using System.Collections.Generic;

namespace MirraGames.SDK.Common
{

    [Awaitable, Wrapper]
    public abstract partial class CommonAds : IAds
    {

        protected readonly IEventAggregator eventAggregator;

        public CommonAds(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        // Banner

        public virtual bool IsBannerReady { get; protected set; } = false;
        public virtual bool IsBannerVisible { get; protected set; } = false;
        public virtual bool IsBannerAvailable { get; } = false;

        protected abstract void InvokeBannerImpl();
        protected abstract void RefreshBannerImpl();
        protected abstract void DisableBannerImpl();

        public void InvokeBanner()
        {
            Logger.CreateText(this, nameof(InvokeBanner));
            try
            {
                InvokeBannerImpl();
            }
            catch (Exception exception)
            {
                Logger.CreateError(this, nameof(InvokeBanner), exception);
            }
        }

        public void RefreshBanner()
        {
            Logger.CreateText(this, nameof(RefreshBanner));
            try
            {
                RefreshBannerImpl();
            }
            catch (Exception exception)
            {
                Logger.CreateError(this, nameof(RefreshBanner), exception);
            }
        }

        public void DisableBanner()
        {
            Logger.CreateText(this, nameof(DisableBanner));
            try
            {
                DisableBannerImpl();
            }
            catch (Exception exception)
            {
                Logger.CreateError(this, nameof(DisableBanner), exception);
            }
        }

        // Interstitial

        private DateTime? lastInterstitialSuccess = null;

        public virtual bool IsInterstitialReady { get; protected set; }
        public virtual bool IsInterstitialVisible { get; protected set; }
        public virtual bool IsInterstitialAvailable { get; }

        protected abstract void InvokeInterstitialImpl(InterstitialParameters parameters, Action onOpen, Action<bool> onClose);

        public DateTime? GetLastInterstitialSuccess()
        {
            return lastInterstitialSuccess;
        }

        public void InvokeInterstitial(InterstitialParameters parameters)
        {
            Logger.CreateText(this, nameof(InvokeInterstitial), parameters.PlacementId);
            try
            {
                // Check availability
                if (!IsInterstitialAvailable)
                {
                    Logger.CreateError(this, "Interstitial not available");
                    parameters.OnClose?.Invoke(false);
                    return;
                }
                if (IsInterstitialVisible)
                {
                    Logger.CreateError(this, "Interstitial already visible");
                    parameters.OnClose?.Invoke(false);
                    return;
                }
                // Hard limit to one show per specified seconds
                // (checking last success from both interstitial and rewarded)
                if (lastInterstitialSuccess.HasValue)
                {
                    TimeSpan interstitialTimeSpan = DateTime.Now - lastInterstitialSuccess.Value;
                    double interstitialSeconds = interstitialTimeSpan.TotalSeconds;
                    if (interstitialSeconds < parameters.AdsIntervalSeconds)
                    {
                        Logger.CreateError(this, "Interstitial frequency capped (interstitial)", parameters.AdsIntervalSeconds - interstitialSeconds, "seconds left");
                        parameters.OnClose?.Invoke(false);
                        return;
                    }
                }
                if (lastRewardedSuccess.HasValue)
                {
                    TimeSpan rewardedTimeSpan = DateTime.Now - lastRewardedSuccess.Value;
                    double rewardedSeconds = rewardedTimeSpan.TotalSeconds;
                    if (rewardedSeconds < parameters.AdsIntervalSeconds)
                    {
                        Logger.CreateError(this, "Interstitial frequency capped (rewarded)", parameters.AdsIntervalSeconds - rewardedSeconds, "seconds left");
                        parameters.OnClose?.Invoke(false);
                        return;
                    }
                }
                // Invoke interstitial
                void onOpenCallback()
                {
                    Logger.CreateText(this, nameof(onOpenCallback));
                    parameters.OnOpen?.Invoke();
                    PauseSourceEvent pauseSourceEvent = new(nameof(InvokeInterstitial), true);
                    eventAggregator.Publish(this, pauseSourceEvent);
                }
                void onCloseCallback(bool isSuccess)
                {
                    Logger.CreateText(this, nameof(onCloseCallback), isSuccess);
                    parameters.OnClose?.Invoke(isSuccess);
                    if (isSuccess)
                    {
                        lastInterstitialSuccess = DateTime.Now;
                    }
                    PauseSourceEvent pauseSourceEvent = new(nameof(InvokeInterstitial), false);
                    eventAggregator.Publish(this, pauseSourceEvent);
                }
                InvokeInterstitialImpl(parameters, onOpenCallback, onCloseCallback);
            }
            catch (Exception exception)
            {
                Logger.CreateError(this, nameof(InvokeInterstitial), exception);
            }
        }

        public void InvokeInterstitial(Action onOpen = null, Action<bool> onClose = null)
        {
            Logger.CreateText(this, nameof(InvokeInterstitial));
            InterstitialParameters parameters = new()
            {
                OnOpen = onOpen,
                OnClose = onClose
            };
            InvokeInterstitial(parameters);
        }

        // Rewarded

        private DateTime? lastRewardedSuccess = null;
        private Dictionary<string, DateTime?> lastRewardedSuccessByTag = new();

        public virtual bool IsRewardedReady { get; protected set; }
        public virtual bool IsRewardedVisible { get; protected set; }
        public virtual bool IsRewardedAvailable { get; }

        protected abstract void InvokeRewardedImpl(RewardedParameters parameters, Action onOpen, Action<bool> onClose);

        public DateTime? GetLastRewardedSuccess(string rewardTag = null)
        {
            if (string.IsNullOrEmpty(rewardTag))
            {
                return lastRewardedSuccess;
            }
            if (lastRewardedSuccessByTag.TryGetValue(rewardTag, out var dateTime))
            {
                return dateTime;
            }
            return null;
        }

        public void InvokeRewarded(RewardedParameters parameters)
        {
            Logger.CreateText(this, nameof(InvokeRewarded), parameters.PlacementId);
            try
            {
                // Check availability
                if (!IsRewardedAvailable)
                {
                    Logger.CreateError(this, "Rewarded not available");
                    parameters.OnClose?.Invoke(false);
                    return;
                }
                if (IsRewardedVisible)
                {
                    Logger.CreateError(this, "Rewarded already visible");
                    parameters.OnClose?.Invoke(false);
                    return;
                }
                // Invoke rewarded
                void onOpenCallback()
                {
                    Logger.CreateText(this, nameof(onOpenCallback));
                    parameters.OnOpen?.Invoke();
                    PauseSourceEvent pauseSourceEvent = new(nameof(InvokeRewarded), true);
                    eventAggregator.Publish(this, pauseSourceEvent);
                }
                void onCloseCallback(bool isSuccess)
                {
                    Logger.CreateText(this, nameof(onCloseCallback), isSuccess);
                    parameters.OnClose?.Invoke(isSuccess);
                    if (isSuccess)
                    {
                        lastRewardedSuccess = DateTime.Now;
                        if (!string.IsNullOrEmpty(parameters.PlacementId))
                        {
                            lastRewardedSuccessByTag[parameters.PlacementId] = lastRewardedSuccess;
                        }
                    }
                    PauseSourceEvent pauseSourceEvent = new(nameof(InvokeRewarded), false);
                    eventAggregator.Publish(this, pauseSourceEvent);
                }
                InvokeRewardedImpl(parameters, onOpenCallback, onCloseCallback);
            }
            catch (Exception exception)
            {
                Logger.CreateError(this, nameof(InvokeRewarded), exception);
            }
        }

        public void InvokeRewarded(Action onOpen = null, Action<bool> onClose = null, string rewardTag = null)
        {
            RewardedParameters parameters = new()
            {
                OnOpen = onOpen,
                OnClose = onClose,
                PlacementId = rewardTag
            };
            InvokeRewarded(parameters);
        }

    }

}
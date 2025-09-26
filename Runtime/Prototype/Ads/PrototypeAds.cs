using MirraGames.SDK.Common;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = MirraGames.SDK.Common.Logger;

namespace MirraGames.SDK.Prototype {

    [Provider(typeof(IAds))]
    public class PrototypeAds : CommonAds {

        private readonly UIDocument document;

        private readonly VisualElement shadowElement;
        private readonly VisualElement interstitialElement;
        private readonly VisualElement rewardedElement;

        private readonly HashSet<string> shadowSources = new();
        private readonly Stack<Action<bool>> interstitialCloseCallbacks = new();
        private readonly Stack<Action<bool>> rewardedCloseCallbacks = new();

        public PrototypeAds(IEventAggregator eventAggregator) : base(eventAggregator) {
            GameObject prototypeDocumentPrefab = PrefabReference.Load("PrototypeDocument").Prefab;
            GameObject prototypeDocumentObject = GameObject.Instantiate(prototypeDocumentPrefab);
            prototypeDocumentObject.name = nameof(PrototypeAds);
            GameObject.DontDestroyOnLoad(prototypeDocumentObject);
            document = prototypeDocumentObject.GetComponent<UIDocument>();

            VisualTreeAsset prototypeAdsAsset = VisualTreeReference.Load(nameof(PrototypeAds)).VisualTree;
            VisualElement prototypeAdsElement = prototypeAdsAsset.Instantiate();
            prototypeAdsElement.style.flexGrow = 1;

            shadowElement = prototypeAdsElement.Q<VisualElement>(Naming.Shadow);
            interstitialElement = prototypeAdsElement.Q<VisualElement>(Naming.Interstitial);
            rewardedElement = prototypeAdsElement.Q<VisualElement>(Naming.Rewarded);

            shadowElement.Hide();
            interstitialElement.Hide();
            rewardedElement.Hide();

            document.rootVisualElement.pickingMode = PickingMode.Ignore;
            prototypeAdsElement.pickingMode = PickingMode.Ignore;

            document.rootVisualElement.Add(prototypeAdsElement);
            document.sortingOrder = 1000;

            interstitialElement.Q<Button>(Naming.Close).RegisterCallback<ClickEvent>(clickEvent => {
                RegisterShadowSource(Naming.Interstitial, false);
                interstitialCloseCallbacks.PopInvokeAll(true);
                interstitialElement.Hide();
                IsInterstitialVisible = false;
            });

            rewardedElement.Q<Button>(Naming.Success).RegisterCallback<ClickEvent>(clickEvent => {
                RegisterShadowSource(Naming.Rewarded, false);
                rewardedCloseCallbacks.PopInvokeAll(true);
                rewardedElement.Hide();
                IsRewardedVisible = false;
            });
            rewardedElement.Q<Button>(Naming.Close).RegisterCallback<ClickEvent>(clickEvent => {
                RegisterShadowSource(Naming.Rewarded, false);
                rewardedCloseCallbacks.PopInvokeAll(false);
                rewardedElement.Hide();
                IsRewardedVisible = false;
            });

            SetInitialized();
        }

        protected override void InvokeBannerImpl() {
            Logger.NotImplementedWarning(this, nameof(InvokeBannerImpl));
        }

        protected override void DisableBannerImpl() {
            Logger.NotImplementedWarning(this, nameof(DisableBannerImpl));
        }

        protected override void RefreshBannerImpl() {
            Logger.NotImplementedWarning(this, nameof(RefreshBannerImpl));
        }

        public override bool IsInterstitialReady { get; protected set; } = true;
        public override bool IsInterstitialVisible { get; protected set; } = false;
        public override bool IsInterstitialAvailable { get; } = true;

        protected override void InvokeInterstitialImpl(Action onOpen = null, Action<bool> onClose = null) {
            if (IsInterstitialVisible) {
                Logger.CreateWarning(this, "Interstitial is already visible");
                onClose?.Invoke(false);
                return;
            }
            IsInterstitialVisible = true;
            RegisterShadowSource(Naming.Interstitial, true);
            interstitialCloseCallbacks.Push(onClose);
            interstitialElement.Show();
            onOpen?.Invoke();
        }

        public override bool IsRewardedReady { get; protected set; } = true;
        public override bool IsRewardedVisible { get; protected set; } = false;
        public override bool IsRewardedAvailable { get; } = true;

        protected override void InvokeRewardedImpl(Action onOpen = null, Action<bool> onClose = null, string rewardTag = null) {
            if (IsRewardedVisible) {
                Logger.CreateWarning(this, "Rewarded is already visible");
                onClose?.Invoke(false);
                return;
            }
            IsRewardedVisible = true;
            RegisterShadowSource(Naming.Rewarded, true);
            rewardedCloseCallbacks.Push(onClose);
            rewardedElement.Show();
            onOpen?.Invoke();
        }

        private void RegisterShadowSource(string source, bool isActive) {
            if (isActive) {
                shadowSources.Add(source);
            }
            else {
                shadowSources.Remove(source);
            }
            if (shadowSources.Count > 0) {
                shadowElement.style.display = DisplayStyle.Flex;
            }
            else {
                shadowElement.style.display = DisplayStyle.None;
            }
        }

    }

}
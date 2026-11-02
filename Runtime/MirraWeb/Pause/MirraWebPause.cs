using AOT;
using MirraGames.SDK.Common;
using System;
using System.Runtime.InteropServices;

namespace MirraGames.SDK.MirraWeb {

    [Provider(typeof(IPause))]
    public class MirraWebPause : CommonPause {

        [DllImport(Naming.InternalDll)] private static extern bool mirraSDK_pause_isPaused();
        [DllImport(Naming.InternalDll)] private static extern void mirraSDK_pause_onPauseChange(DelegateInt onPauseChange);

        [MonoPInvokeCallback(typeof(DelegateVoid))]
        private static void OnPauseChangeCallback(int senderId, int isPaused) {
            try {
                OnExternalPauseChange?.Invoke(isPaused != 0);
            }
            catch (Exception exception) {
                Logger.CreateError(typeof(MirraWebPause), exception);
            }
        }

        private static event Action<bool> OnExternalPauseChange;

        /// <summary>
        /// Whether Unity focus and application pause callbacks pause the game. Platforms that dictate pause themselves turn this off.
        /// </summary>
        protected virtual bool HandleApplicationEvents => true;

        private bool isFocusLost;

        public MirraWebPause(IEventAggregator aggregator, IEventDispatcher eventDispatcher) : base(aggregator) {
            if (HandleApplicationEvents) {
                eventDispatcher.OnApplicationFocus += OnApplicationFocus;
                eventDispatcher.OnApplicationPause += OnApplicationPause;
                eventDispatcher.LateUpdate += ReleaseFocusOnInput;
            }
            try {
                mirraSDK_pause_onPauseChange(OnPauseChangeCallback);
                OnExternalPauseChange += (isPaused) => {
                    Register(nameof(OnExternalPauseChange), isPaused);
                };
                bool isPaused = mirraSDK_pause_isPaused();
                Register(nameof(OnExternalPauseChange), isPaused);
            }
            catch (Exception exception) {
                Logger.CreateError(this, exception);
            }
        }

        public void OnApplicationFocus(bool focusStatus) {
            isFocusLost = !focusStatus;
            Register(nameof(OnApplicationFocus), !focusStatus);
        }

        /// <summary>
        /// An ad iframe takes the page focus and can disappear without the window ever getting it back, so Unity never
        /// reports focus again and the game stays paused. A closed ad means the player is back, so its focus pause is
        /// released too, the same way the web layer releases "window.focus" when an ad closes.
        /// </summary>
        public override void OnEvent(PauseSourceEvent eventData) {
            base.OnEvent(eventData);
            if (eventData.Pause == false && IsAdSource(eventData.Source)) {
                ReleaseFocusPause();
            }
        }

        private static bool IsAdSource(string source) {
            return source == nameof(CommonAds.InvokeInterstitial) || source == nameof(CommonAds.InvokeRewarded);
        }

        /// <summary>
        /// Clicks, taps and keys only reach an unfocused game when the browser lost a focus event, so the first press
        /// releases a focus pause that is still held for any other reason.
        /// </summary>
        private void ReleaseFocusOnInput() {
#if ENABLE_LEGACY_INPUT_MANAGER
            if (isFocusLost && HasPressThisFrame()) {
                ReleaseFocusPause();
            }
#endif
        }

#if ENABLE_LEGACY_INPUT_MANAGER
        private static bool HasPressThisFrame() {
            if (global::UnityEngine.Input.anyKeyDown) return true;
            for (int index = 0; index < global::UnityEngine.Input.touchCount; index++) {
                if (global::UnityEngine.Input.GetTouch(index).phase == global::UnityEngine.TouchPhase.Began) return true;
            }
            return false;
        }
#endif

        private void ReleaseFocusPause() {
            if (isFocusLost == false) return;
            isFocusLost = false;
            Register(nameof(OnApplicationFocus), false);
        }

        public void OnApplicationPause(bool pauseStatus) {
            Register(nameof(OnApplicationPause), pauseStatus);
        }

    }

}
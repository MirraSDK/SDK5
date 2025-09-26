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

        public MirraWebPause(IEventAggregator aggregator, IEventDispatcher eventDispatcher) : base(aggregator) {
            eventDispatcher.OnApplicationFocus += OnApplicationFocus;
            eventDispatcher.OnApplicationPause += OnApplicationPause;
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
            Register(nameof(OnApplicationFocus), !focusStatus);
        }

        public void OnApplicationPause(bool pauseStatus) {
            Register(nameof(OnApplicationPause), pauseStatus);
        }

    }

}
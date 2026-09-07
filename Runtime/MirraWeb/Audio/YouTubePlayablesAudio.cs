using AOT;
using MirraGames.SDK.Common;
using MirraGames.SDK.UnityEngine;
using System;
using System.Runtime.InteropServices;

namespace MirraGames.SDK.YouTubePlayables {

    [Provider(typeof(IAudio))]
    public class YouTubePlayablesAudio : UnityEngineAudio {

        [DllImport(Naming.InternalDll)] private static extern bool mirraSDK_youtubePlayables_audio_isEnabled();
        [DllImport(Naming.InternalDll)] private static extern void mirraSDK_youtubePlayables_audio_onEnabledChange(DelegateInt onEnabledChange);

        private static event Action<bool> OnExternalEnabledChange;

        public YouTubePlayablesAudio(UnityEngineAudio_Configuration config, IEventAggregator eventAggregator) : base(config, eventAggregator) {
            try {
                OnExternalEnabledChange += OnAudioEnabledChange;
                SetMuted(!mirraSDK_youtubePlayables_audio_isEnabled());
                mirraSDK_youtubePlayables_audio_onEnabledChange(OnEnabledChange);
            }
            catch (Exception exception) {
                Logger.CreateError(this, exception);
            }
        }

        [MonoPInvokeCallback(typeof(DelegateInt))]
        private static void OnEnabledChange(int senderId, int isEnabled) {
            try {
                OnExternalEnabledChange?.Invoke(isEnabled != 0);
            }
            catch (Exception exception) {
                Logger.CreateError(typeof(YouTubePlayablesAudio), exception);
            }
        }

        private void OnAudioEnabledChange(bool isEnabled) {
            SetMuted(!isEnabled);
        }

    }

}

using MirraGames.SDK.Common;
using UnityEngine;

namespace MirraGames.SDK.UnityEngine {

    [Provider(typeof(IAudio))]
    public class UnityEngineAudio : CommonAudio {

        public UnityEngineAudio(UnityEngineAudio_Configuration config, IEventAggregator eventAggregator) : base(eventAggregator) {
            HandlePauseEvents = config.HandlePauseEvents;
        }

        protected override bool GetPauseImpl() {
            return AudioListener.pause;
        }

        protected override float GetVolumeImpl() {
            return AudioListener.volume;
        }

        protected override void SetPauseImpl(bool pause) {
            AudioListener.pause = pause;
        }

        protected override void SetVolumeImpl(float volume) {
            AudioListener.volume = volume;
        }
    }

}
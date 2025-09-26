using MirraGames.SDK.Common;

namespace MirraGames.SDK.Fallback {

    [Provider(typeof(IAudio))]
    public class FallbackAudio : CommonAudio {

        public FallbackAudio(IEventAggregator eventAggregator) : base(eventAggregator) { }

        protected override bool HandlePauseEvents => false;

        protected override float GetVolumeImpl() {
            return default;
        }

        protected override void SetVolumeImpl(float volume) {
            Logger.NotImplementedWarning(this, nameof(SetVolumeImpl));
        }

        protected override bool GetPauseImpl() {
            return default;
        }

        protected override void SetPauseImpl(bool pause) {
            Logger.NotImplementedWarning(this, nameof(SetPauseImpl));
        }

    }

}
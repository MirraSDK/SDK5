using MirraGames.SDK.Common;

namespace MirraGames.SDK.Fallback {

    [Provider(typeof(ITimeScale))]
    public class FallbackTimeScale : CommonTimeScale {

        public FallbackTimeScale(IEventAggregator eventAggregator) : base(eventAggregator) { }

        protected override bool HandlePauseEvents => false;

        protected override float GetScaleImpl() {
            Logger.NotImplementedWarning(this, nameof(GetScaleImpl));
            return default;
        }

        protected override void SetScaleImpl(float scale) {
            Logger.NotImplementedWarning(this, nameof(SetScaleImpl));
        }

    }

}
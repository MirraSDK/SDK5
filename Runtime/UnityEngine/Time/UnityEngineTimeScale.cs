using MirraGames.SDK.Common;
using UnityEngine;

namespace MirraGames.SDK.UnityEngine {

    [Provider(typeof(ITimeScale))]
    public class UnityEngineTimeScale : CommonTimeScale {

        public UnityEngineTimeScale(UnityEngineTimeScale_Configuration config, IEventAggregator eventAggregator) : base(eventAggregator) {
            HandlePauseEvents = config.HandlePauseEvents;
        }

        protected override float GetScaleImpl() {
            return Time.timeScale;
        }

        protected override void SetScaleImpl(float scale) {
            Time.timeScale = scale;
        }
    }

}
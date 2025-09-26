using MirraGames.SDK.Common;

namespace MirraGames.SDK.Fallback {

    [Provider(typeof(IPause))]
    public class FallbackPause : CommonPause {

        public FallbackPause(IEventAggregator eventAggregator) : base(eventAggregator) { }

    }

}
using MirraGames.SDK.Common;
using MirraGames.SDK.MirraWeb;

namespace MirraGames.SDK.YouTubePlayables {

    /// <summary>
    /// Pauses only from the Playables SDK and ads. YouTube forbids driving pause from page visibility or focus.
    /// </summary>
    [Provider(typeof(IPause))]
    public class YouTubePlayablesPause : MirraWebPause {

        protected override bool HandleApplicationEvents => false;

        public YouTubePlayablesPause(IEventAggregator aggregator, IEventDispatcher eventDispatcher) : base(aggregator, eventDispatcher) { }

    }

}

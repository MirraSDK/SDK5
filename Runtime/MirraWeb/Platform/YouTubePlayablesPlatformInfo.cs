using MirraGames.SDK.Common;
using MirraGames.SDK.MirraWeb;

namespace MirraGames.SDK.YouTubePlayables {

    /// <summary>
    /// Reports YouTube Playables for the whole YouTube build, including local test runs outside the Playables environment.
    /// </summary>
    [Provider(typeof(IPlatformInfo))]
    public class YouTubePlayablesPlatformInfo : MirraWebPlatformInfo {

        protected override PlatformType GetCurrentImpl() {
            return PlatformType.YouTubePlayables;
        }

    }

}

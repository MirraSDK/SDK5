using MirraGames.SDK.Common;

namespace MirraGames.SDK.Fallback {

    [Provider(typeof(IBootstrap))]
    public class FallbackBootstrap : CommonBootstrap {

        public FallbackBootstrap() {
            SetInitialized();
        }

    }

}
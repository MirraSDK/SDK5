using MirraGames.SDK.Common;

namespace MirraGames.SDK.Fallback {

    [Provider(typeof(IPlatformInteractions))]
    public class FallbackPlatformInteractions : CommonPlatformInteractions {

        protected override void ShareGameImpl(string messageText) {
            Logger.NotImplementedWarning(this, nameof(ShareGameImpl));
        }

        protected override void RateGameImpl() {
            Logger.NotImplementedWarning(this, nameof(RateGameImpl));
        }

    }

}
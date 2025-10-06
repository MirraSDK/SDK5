using MirraGames.SDK.Common;

namespace MirraGames.SDK.iOS {

    [Configuration]
    public class AppleAppStore : iOSConfiguration {

        public override string Name { get; } = "AppleAppStore";
        public override string Description { get; } = "https://www.apple.com/ios/app-store/";
        public override string IconName { get; } = "AppleAppStore";

    }

}
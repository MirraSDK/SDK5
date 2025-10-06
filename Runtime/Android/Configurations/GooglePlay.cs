using MirraGames.SDK.Common;

namespace MirraGames.SDK.Android {

    [Configuration]
    public class GooglePlay : AndroidConfiguration {

        public override string Name { get; } = "GooglePlay";
        public override string Description { get; } = "https://play.google.com/store";
        public override string IconName { get; } = "GooglePlay";

    }

}
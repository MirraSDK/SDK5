using MirraGames.SDK.Common;

namespace MirraGames.SDK.Android {

    [Configuration]
    public class SamsungGalaxyStore : AndroidConfiguration {

        public override string Name { get; } = "SamsungGalaxyStore";
        public override string Description { get; } = "https://www.samsung.com/us/apps/galaxy-store/";
        public override string IconName { get; } = "SamsungGalaxyStore";

    }

}
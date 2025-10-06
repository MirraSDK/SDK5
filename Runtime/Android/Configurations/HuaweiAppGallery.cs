using MirraGames.SDK.Common;

namespace MirraGames.SDK.Android {

    [Configuration]
    public class HuaweiAppGallery : AndroidConfiguration {

        public override string Name { get; } = "HuaweiAppGallery";
        public override string Description { get; } = "https://appgallery.huawei.com/";
        public override string IconName { get; } = "HuaweiAppGallery";

    }

}
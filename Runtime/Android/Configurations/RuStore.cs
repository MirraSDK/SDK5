using MirraGames.SDK.Common;

namespace MirraGames.SDK.Android {

    [Configuration]
    public class RuStore : AndroidConfiguration {

        public override string Name { get; } = "RuStore";
        public override string Description { get; } = "https://www.rustore.ru/";
        public override string IconName { get; } = "RuStore";

        public override string PaymentsProviderName => "RuStorePayments";
        public override string PlatformInteractionsProviderName => "RuStorePlatformInteractions";
        public override string PlatformInfoProviderName => "RuStorePlatformInfo";
        public override string PlayerAccountProviderName => "RuStorePlayerAccount";

    }

}
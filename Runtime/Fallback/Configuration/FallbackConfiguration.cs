using MirraGames.SDK.Common;

namespace MirraGames.SDK.Fallback {

    [Configuration]
    public class FallbackConfiguration : Configuration {

        public override string Name => nameof(Fallback);
        public override string Description => "Always available to cover missing parts";
        public override string IconName => nameof(Fallback);
        public override bool ReadOnly => true;

        public override string AchievementsProviderName { get; } = nameof(FallbackAchievements);
        public override string AdsProviderName { get; } = nameof(FallbackAds);
        public override string EventsReporterProviderName { get; } = nameof(FallbackEventsReporter);
        public override string GameplayReporterProviderName { get; } = nameof(FallbackGameplayReporter);
        public override string AddressablesProviderName { get; } = nameof(FallbackAddressables);
        public override string AssetBundlesProviderName { get; } = nameof(FallbackAssetBundles);
        public override string StreamingAssetsProviderName { get; } = nameof(FallbackStreamingAssets);
        public override string AudioProviderName { get; } = nameof(FallbackAudio);
        public override string BootstrapProviderName { get; } = nameof(FallbackBootstrap);
        public override string DataProviderName { get; } = nameof(FallbackData);
        public override string DeviceBrowserProviderName { get; } = nameof(FallbackDeviceBrowser);
        public override string DeviceCursorProviderName { get; } = nameof(FallbackDeviceCursor);
        public override string DeviceInfoProviderName { get; } = nameof(FallbackDeviceInfo);
        public override string FlagsProviderName { get; } = nameof(FallbackFlags);
        public override string LanguageInfoProviderName { get; } = nameof(FallbackLanguageInfo);
        public override string PauseProviderName { get; } = nameof(FallbackPause);
        public override string PaymentsProviderName { get; } = nameof(FallbackPayments);
        public override string PlatformInfoProviderName { get; } = nameof(FallbackPlatformInfo);
        public override string PlatformInteractionsProviderName { get; } = nameof(FallbackPlatformInteractions);
        public override string PlayerAccountProviderName { get; } = nameof(FallbackPlayerAccount);
        public override string DateTimeProviderName { get; } = nameof(FallbackDateTime);
        public override string TimeScaleProviderName { get; } = nameof(FallbackTimeScale);

    }

}
using MirraGames.SDK.Common;
using System;

namespace MirraGames.SDK.MirraWeb {

    [Configuration]
    public class MirraWebConfiguration : Configuration {

        public override string Name { get; } = nameof(MirraWeb);
        public override string Description { get; } = "Stable crossplatform Web support from MirraGames";
        public override string IconName { get; } = "MirraGames";
        public override bool ReadOnly { get; } = false;

        public override Type[] PropertyGroups { get; } = new Type[] {
            typeof(Logger_PropertyGroup),
            typeof(Y8_PropertyGroup),
            typeof(Lagged_PropertyGroup),
            typeof(CrazyGames_PropertyGroup),
            typeof(GameDistribution_PropertyGroup),
            typeof(VK_PropertyGroup),
            typeof(OK_PropertyGroup),
            typeof(LumixGames_PropertyGroup)
        };

        public override string AchievementsProviderName { get; } = nameof(MirraWebAchievements);
        public override string AdsProviderName { get; } = nameof(MirraWebAds);
        public override string EventsReporterProviderName { get; } = "FallbackEventsReporter";
        public override string GameplayReporterProviderName { get; } = nameof(MirraWebGameplayReporter);
        public override string AddressablesProviderName { get; } = "UnityEngineAddressables";
        public override string AssetBundlesProviderName { get; } = "UnityEngineAssetBundles";
        public override string StreamingAssetsProviderName { get; } = "UnityEngineStreamingAssets";
        public override string AudioProviderName { get; } = "UnityEngineAudio";
        public override string BootstrapProviderName { get; } = nameof(MirraWebBootstrap);
        public override string DataProviderName { get; } = nameof(MirraWebData);
        public override string DeviceBrowserProviderName { get; } = nameof(MirraWebDeviceBrowser);
        public override string DeviceCursorProviderName { get; } = "UnityEngineDeviceCursor";
        public override string DeviceInfoProviderName { get; } = nameof(MirraWebDeviceInfo);
        public override string FlagsProviderName { get; } = nameof(MirraWebFlags);
        public override string LanguageInfoProviderName { get; } = nameof(MirraWebLanguageInfo);
        public override string PauseProviderName { get; } = nameof(MirraWebPause);
        public override string PaymentsProviderName { get; } = nameof(MirraWebPayments);
        public override string PlatformInfoProviderName { get; } = nameof(MirraWebPlatformInfo);
        public override string PlatformInteractionsProviderName { get; } = nameof(MirraWebPlatformInteractions);
        public override string PlayerAccountProviderName { get; } = nameof(MirraWebPlayerAccount);
        public override string DateTimeProviderName { get; } = "SystemDateTime";
        public override string TimeScaleProviderName { get; } = "UnityEngineTimeScale";

    }

}
using MirraGames.SDK.Common;
using System;

namespace MirraGames.SDK.YouTubePlayables
{
    [Configuration]
    public class YouTubePlayablesConfiguration : Configuration
    {

        public override string Name { get; } = "YouTubePlayables";
        public override string Description { get; } = "YouTube Playables Web support from MirraGames";
        public override string IconName { get; } = "YouTube";
        public override bool ReadOnly { get; } = false;

        // Группы свойств для отрисовки в окне SDK
        public override Type[] PropertyGroups { get; } = new Type[] {
            typeof(Framework_PropertyGroup),
            typeof(Logger_PropertyGroup),
        };

        // Провайдеры (Объединено и без дубликатов)
        public override string AchievementsProviderName { get; } = "FallbackAchievements";
        public override string AdsProviderName { get; } = nameof(MirraWebAds);
        public override string EventsReporterProviderName { get; } = "FallbackEventsReporter";
        public override string GameplayReporterProviderName { get; } = "FallbackGameplayReporter";

        public override string AddressablesProviderName { get; } = "UnityEngineAddressables";
        public override string AssetBundlesProviderName { get; } = "UnityEngineAssetBundles";
        public override string StreamingAssetsProviderName { get; } = "UnityEngineStreamingAssets";
        public override string AudioProviderName { get; } = "UnityEngineAudio";
        public override string DeviceCursorProviderName { get; } = "UnityEngineDeviceCursor";
        public override string DateTimeProviderName { get; } = "SystemDateTime";
        public override string TimeScaleProviderName { get; } = "UnityEngineTimeScale";

        public override string BootstrapProviderName { get; } = nameof(MirraWebBootstrap);
        public override string DataProviderName { get; } = nameof(MirraWebData);
        public override string DeviceBrowserProviderName { get; } = nameof(MirraWebDeviceBrowser);
        public override string DeviceInfoProviderName { get; } = nameof(MirraWebDeviceInfo);
        public override string FlagsProviderName { get; } = nameof(MirraWebFlags);
        public override string LanguageInfoProviderName { get; } = nameof(MirraWebLanguageInfo);
        public override string PauseProviderName { get; } = nameof(MirraWebPause);
        public override string PaymentsProviderName { get; } = nameof(MirraWebPayments);
        public override string PlatformInfoProviderName { get; } = nameof(MirraWebPlatformInfo);
        public override string PlatformInteractionsProviderName { get; } = nameof(MirraWebPlatformInteractions);
        public override string PlayerAccountProviderName { get; } = nameof(MirraWebPlayerAccount);
    }
}
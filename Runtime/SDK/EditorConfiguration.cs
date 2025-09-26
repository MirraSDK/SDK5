using MirraGames.SDK.Common;
using MirraGames.SDK.Fallback;
using MirraGames.SDK.Prototype;
using MirraGames.SDK.System;
using MirraGames.SDK.UnityEngine;

namespace MirraGames.SDK {

    [Configuration]
    public class EditorConfiguration : Configuration {

        public override string Name { get; } = "Editor";
        public override string Description { get; } = "This will be used when running in the Unity Editor";
        public override string IconName { get; } = "Unity";
        public override bool ReadOnly { get; } = false;

        public override string AchievementsProviderName { get; } = nameof(FallbackAchievements);
        public override string AdsProviderName { get; } = nameof(PrototypeAds);
        public override string EventsReporterProviderName { get; } = nameof(FallbackEventsReporter);
        public override string GameplayReporterProviderName { get; } = nameof(FallbackGameplayReporter);
        public override string AddressablesProviderName { get; } = nameof(UnityEngineAddressables);
        public override string AssetBundlesProviderName { get; } = nameof(UnityEngineAssetBundles);
        public override string StreamingAssetsProviderName { get; } = nameof(UnityEngineStreamingAssets);
        public override string AudioProviderName { get; } = nameof(UnityEngineAudio);
        public override string BootstrapProviderName { get; } = nameof(FallbackBootstrap);
        public override string DataProviderName { get; } = nameof(UnityEngineData);
        public override string DeviceBrowserProviderName { get; } = nameof(UnityEngineDeviceBrowser);
        public override string DeviceCursorProviderName { get; } = nameof(UnityEngineDeviceCursor);
        public override string DeviceInfoProviderName { get; } = nameof(PrototypeDeviceInfo);
        public override string FlagsProviderName { get; } = nameof(FallbackFlags);
        public override string LanguageInfoProviderName { get; } = nameof(PrototypeLanguageInfo);
        public override string PauseProviderName { get; } = nameof(UnityEnginePause);
        public override string PaymentsProviderName { get; } = nameof(PrototypePayments);
        public override string PlatformInfoProviderName { get; } = nameof(PrototypePlatformInfo);
        public override string PlatformInteractionsProviderName { get; } = nameof(FallbackPlatformInteractions);
        public override string PlayerAccountProviderName { get; } = nameof(FallbackPlayerAccount);
        public override string DateTimeProviderName { get; } = nameof(SystemDateTime);
        public override string TimeScaleProviderName { get; } = nameof(UnityEngineTimeScale);

    }

}
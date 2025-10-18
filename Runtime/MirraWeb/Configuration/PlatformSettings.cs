using MirraGames.SDK.Common;
using System;
using UnityEngine;

namespace MirraGames.SDK.MirraWeb {

    [Serializable]
    public class PlatformSettings {

        [SerializeField] public Logger_PropertyGroup logger;
        [SerializeField] public CrazyGames_PropertyGroup crazyGames;
        [SerializeField] public Y8_PropertyGroup y8;
        [SerializeField] public GameDistribution_PropertyGroup gameDistribution;
        [SerializeField] public Lagged_PropertyGroup lagged;
        // [SerializeField] public YandexGames_PropertyGroup yandexGames;
        [SerializeField] public VK_PropertyGroup vk;
        [SerializeField] public OK_PropertyGroup ok;
        [SerializeField] public LumixGames_PropertyGroup lumixGames;
        // [SerializeField] public GamePix_PropertyGroup gamePix;
        // [SerializeField] public Poki_PropertyGroup poki;
        // [SerializeField] public CoolMath_PropertyGroup coolMath;
        // [SerializeField] public PlayDeck_PropertyGroup playDeck;

        public PlatformSettings() {
            logger = new Logger_PropertyGroup();
            crazyGames = new CrazyGames_PropertyGroup();
            y8 = new Y8_PropertyGroup();
            gameDistribution = new GameDistribution_PropertyGroup();
            lagged = new Lagged_PropertyGroup();
            // yandexGames = new YandexGames_PropertyGroup();
            vk = new VK_PropertyGroup();
            ok = new OK_PropertyGroup();
            lumixGames = new LumixGames_PropertyGroup();
            // gamePix = new GamePix_PropertyGroup();
            // poki = new Poki_PropertyGroup();
            // coolMath = new CoolMath_PropertyGroup();
            // playDeck = new PlayDeck_PropertyGroup();
        }

        public PlatformSettings(PreferencesReader preferencesReader) {
            string configurationName = nameof(MirraWebConfiguration);
            logger = preferencesReader.GetPropertyGroup<Logger_PropertyGroup>(configurationName);
            crazyGames = preferencesReader.GetPropertyGroup<CrazyGames_PropertyGroup>(configurationName);
            y8 = preferencesReader.GetPropertyGroup<Y8_PropertyGroup>(configurationName);
            gameDistribution = preferencesReader.GetPropertyGroup<GameDistribution_PropertyGroup>(configurationName);
            lagged = preferencesReader.GetPropertyGroup<Lagged_PropertyGroup>(configurationName);
            vk = preferencesReader.GetPropertyGroup<VK_PropertyGroup>(configurationName);
            ok = preferencesReader.GetPropertyGroup<OK_PropertyGroup>(configurationName);
            lumixGames = preferencesReader.GetPropertyGroup<LumixGames_PropertyGroup>(configurationName);
        }

    }

}
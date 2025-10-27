using MirraGames.SDK.Common;
using System;
using UnityEngine;

namespace MirraGames.SDK.MirraWeb {

    [Serializable]
    public class LumixGames_PropertyGroup : PropertyGroup {

        public override string Name => "LumixGames";

        [field: SerializeField] public bool isRussianDomain = false;
        [field: SerializeField] public AdSenseSettings adSense = new();
        [field: SerializeField] public YandexAdsSettings yandexAds = new();

        public override StringProperty[] GetStringProperties() {
            return new StringProperty[] {
                new(
                    "AdSense - Data Ad Client",
                    () => adSense.dataAdClient,
                    (value) => { adSense.dataAdClient = value; }
                ),
                new(
                    "AdSense - Data Ad Channel",
                    () => adSense.dataAdChannel,
                    (value) => { adSense.dataAdChannel = value; }
                ),
                new(
                    "Yandex Ads - App ID",
                    () => yandexAds.appId,
                    (value) => { yandexAds.appId = value; }
                ),
                new(
                    "Yandex Ads - Block IDs Json",
                    () => yandexAds.blockIdsJson,
                    (value) => { yandexAds.blockIdsJson = value; }
                ),
            };
        }

        public override BoolProperty[] GetBoolProperties() {
            return new BoolProperty[] {
                new(
                    "Is Russian Domain",
                    () => isRussianDomain,
                    (value) => { isRussianDomain = value; }
                ),
                new(
                    "AdSense - Ad Break Test",
                    () => adSense.dataAdBreakTest,
                    (value) => { adSense.dataAdBreakTest = value; }
                )
            };
        }

        public override FloatProperty[] GetFloatProperties() {
            return new FloatProperty[] {
                new(
                    "AdSense - Interstitial Interval (s)",
                    () => adSense.interstitialInterval,
                    (value) => { adSense.interstitialInterval = value; }
                ),
                new(
                    "Yandex Ads - Interstitial Interval (s)",
                    () => yandexAds.interstitialInterval,
                    (value) => { yandexAds.interstitialInterval = value; }
                )
            };
        }

    }

}
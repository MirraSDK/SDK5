using MirraGames.SDK.Common;
using System;
using UnityEngine;

namespace MirraGames.SDK.MirraWeb {

    [Serializable]
    public class GameDistribution_PropertyGroup : PropertyGroup {

        public override string Name => "GameDistribution";

        [SerializeField] public string gameId = "";
        [SerializeField] public bool adsDebug = false;
        [SerializeField] public bool adsAutoplay = false;
        [SerializeField] public string adsLocale = "en";
        [SerializeField] public float interstitialInterval = 0;

        public override StringProperty[] GetStringProperties() {
            return new StringProperty[] {
                new(
                    "Game Id",
                    () => gameId,
                    (value) => { gameId = value; }
                )
            };
        }

        public override FloatProperty[] GetFloatProperties() {
            return new FloatProperty[] {
                new(
                    "Interstitial Interval (s)",
                    () => interstitialInterval,
                    (value) => { interstitialInterval = value; }
                )
            };
        }

    }

}
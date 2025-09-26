using MirraGames.SDK.Common;
using System;
using UnityEngine;

namespace MirraGames.SDK.MirraWeb {

    [Serializable]
    public class CrazyGames_PropertyGroup : PropertyGroup {

        public override string Name => "CrazyGames";

        [SerializeField] public bool getUserToken = false;
        [SerializeField] public float interstitialInterval = 0;

        public override BoolProperty[] GetBoolProperties() {
            return new BoolProperty[] {
                new(
                    "Get User Token",
                    () => getUserToken,
                    (value) => { getUserToken = value; }
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
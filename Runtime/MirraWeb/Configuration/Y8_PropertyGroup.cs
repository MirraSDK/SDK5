using MirraGames.SDK.Common;
using System;
using UnityEngine;

namespace MirraGames.SDK.MirraWeb {

    [Serializable]
    public class Y8_PropertyGroup : PropertyGroup {

        public override string Name => "Y8";

        [field: SerializeField] public AdSenseSettings adSense = new();

        public override StringProperty[] GetStringProperties() {
            return new StringProperty[] {
                new(
                    "Data Ad Client",
                    () => adSense.dataAdClient,
                    (value) => { adSense.dataAdClient = value; }
                ),
                new(
                    "Data Ad Channel",
                    () => adSense.dataAdChannel,
                    (value) => { adSense.dataAdChannel = value; }
                )
            };
        }

        public override BoolProperty[] GetBoolProperties() {
            return new BoolProperty[] {
                new(
                    "Data Ad Break Test",
                    () => adSense.dataAdBreakTest,
                    (value) => { adSense.dataAdBreakTest = value; }
                )
            };
        }

        public override FloatProperty[] GetFloatProperties() {
            return new FloatProperty[] {
                new(
                    "Interstitial Interval (s)",
                    () => adSense.interstitialInterval,
                    (value) => { adSense.interstitialInterval = value; }
                )
            };
        }

    }

}
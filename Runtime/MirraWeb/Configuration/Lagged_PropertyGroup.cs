using MirraGames.SDK.Common;
using System;
using UnityEngine;

namespace MirraGames.SDK.MirraWeb {

    [Serializable]
    public class Lagged_PropertyGroup : PropertyGroup {

        public override string Name => "Lagged";

        [SerializeField] public string devId = "";
        [SerializeField] public string publisherId = "";
        [SerializeField] public float interstitialInterval = 0;

        public override StringProperty[] GetStringProperties() {
            return new StringProperty[] {
                new("Dev Id", () => devId, (value) => { devId = value; }),
                new("Publisher Id", () => publisherId, (value) => { publisherId = value; })
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
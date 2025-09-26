using MirraGames.SDK.Common;
using System;
using UnityEngine;

namespace MirraGames.SDK.MirraWeb {

    [Serializable]
    public class OK_PropertyGroup : PropertyGroup {

        public override string Name => "OK";

        [SerializeField] public string extraArgs = "";
        [SerializeField] public float interstitialInterval = 0;

        public override StringProperty[] GetStringProperties() {
            return new StringProperty[] {
                new("Extra Args", () => extraArgs, (value) => { extraArgs = value; })
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
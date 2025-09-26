using MirraGames.SDK.Common;
using System;
using UnityEngine;

namespace MirraGames.SDK.MirraWeb {

    [Serializable]
    public class VK_PropertyGroup : PropertyGroup {

        public override string Name => "VK";

        [SerializeField] public string appId = "";
        [SerializeField] public string extraArgs = "";
        [SerializeField] public float interstitialInterval = 0;

        public override StringProperty[] GetStringProperties() {
            return new StringProperty[] {
                new("App Id", () => appId, (value) => { appId = value; }),
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
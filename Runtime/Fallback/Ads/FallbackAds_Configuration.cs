using MirraGames.SDK.Common;
using UnityEngine;

namespace MirraGames.SDK.Fallback {

    [ProviderConfiguration(typeof(FallbackAds))]
    public class FallbackAds_Configuration : PropertyGroup {

        public override string Name => nameof(FallbackAds);

        [field: SerializeField] public bool RewardsSuccess { get; private set; } = false;

        public override BoolProperty[] GetBoolProperties() {
            return new BoolProperty[] {
                new(
                    nameof(RewardsSuccess),
                    getter: () => { return RewardsSuccess; },
                    setter: (value) => { RewardsSuccess = value; }
                )
            };
        }

    }

}
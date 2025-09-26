using MirraGames.SDK.Common;
using UnityEngine;

namespace MirraGames.SDK.UnityEngine {

    [ProviderConfiguration(typeof(UnityEngineTimeScale))]
    public class UnityEngineTimeScale_Configuration : PropertyGroup {

        public override string Name => nameof(UnityEngineTimeScale);

        [field: SerializeField] public bool HandlePauseEvents { get; private set; } = true;

        public override BoolProperty[] GetBoolProperties() {
            return new BoolProperty[] {
                new(
                    nameof(HandlePauseEvents),
                    getter: () => { return HandlePauseEvents; },
                    setter: (value) => { HandlePauseEvents = value; }
                )
            };
        }

    }

}
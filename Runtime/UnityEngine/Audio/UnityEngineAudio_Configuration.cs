using MirraGames.SDK.Common;
using UnityEngine;

namespace MirraGames.SDK.UnityEngine {

    [ProviderConfiguration(typeof(UnityEngineAudio))]
    public class UnityEngineAudio_Configuration : PropertyGroup {

        public override string Name => nameof(UnityEngineAudio);

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
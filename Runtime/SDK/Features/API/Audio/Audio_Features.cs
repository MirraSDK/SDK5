using MirraGames.SDK.Common;
using MirraGames.SDK.SourceGenerator;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Features {

    public class Audio_Features : FeaturesContainer {

        public new class UxmlFactory : UxmlFactory<Audio_Features> { }

        public Audio_Features() {
            SetInfo("Audio", nameof(IAudio), nameof(AudioProvider));

            CreateBoolean(nameof(IAudio.Pause), () => {
                return MirraSDK.Audio.Pause;
            });
            CreateString(nameof(IAudio.Volume), () => {
                return MirraSDK.Audio.Volume.ToString();
            });
        }

    }

}
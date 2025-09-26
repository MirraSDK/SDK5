using MirraGames.SDK.Common;
using MirraGames.SDK.SourceGenerator;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Features {

    public class TimeScale_Features : FeaturesContainer {

        public new class UxmlFactory : UxmlFactory<TimeScale_Features> { }

        public TimeScale_Features() {
            SetInfo("Time Scale", nameof(ITime), nameof(TimeScaleProvider));

            CreateString(nameof(ITimeScale.Scale), () => {
                return MirraSDK.Time.Scale.ToString();
            });
        }

    }

}
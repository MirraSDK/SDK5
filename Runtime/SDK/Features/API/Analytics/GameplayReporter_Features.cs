using MirraGames.SDK.SourceGenerator;
using MirraGames.SDK.Common;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Features {

    public class GameplayReporter_Features : FeaturesContainer {

        public new class UxmlFactory : UxmlFactory<GameplayReporter_Features> { }

        public GameplayReporter_Features() {
            SetInfo("Gameplay Reporter", nameof(IAnalytics), nameof(GameplayReporterProvider));

            CreateButton(nameof(IGameplayReporter.GameIsReady), () => {
                MirraSDK.Analytics.GameIsReady();
            });

            CreateButton(nameof(IGameplayReporter.GameplayStart), () => {
                MirraSDK.Analytics.GameplayStart();
            });

            CreateButton(nameof(IGameplayReporter.GameplayStop), () => {
                MirraSDK.Analytics.GameplayStop();
            });
        }

    }

}
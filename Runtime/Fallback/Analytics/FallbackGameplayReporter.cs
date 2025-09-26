using MirraGames.SDK.Common;

namespace MirraGames.SDK.Fallback {

    [Provider(typeof(IGameplayReporter))]
    public class FallbackGameplayReporter : CommonGameplayReporter {

        public FallbackGameplayReporter() {
            SetInitialized();
        }

        protected override void GameIsReadyImpl() {
            Logger.NotImplementedWarning(this, nameof(GameIsReadyImpl));
        }

        protected override void GameplayStartImpl(int level = 0) {
            Logger.NotImplementedWarning(this, nameof(GameplayStartImpl));
        }

        protected override void GameplayRestartImpl(int level = 0) {
            Logger.NotImplementedWarning(this, nameof(GameplayRestartImpl));
        }

        protected override void GameplayStopImpl(int level = 0) {
            Logger.NotImplementedWarning(this, nameof(GameplayStopImpl));
        }

    }

}
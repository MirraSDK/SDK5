using System;

namespace MirraGames.SDK.Common {

    [Awaitable, Wrapper]
    public abstract partial class CommonGameplayReporter : IGameplayReporter {

        protected abstract void GameIsReadyImpl();
        protected abstract void GameplayStartImpl(int level = 0);
        protected abstract void GameplayRestartImpl(int level = 0);
        protected abstract void GameplayStopImpl(int level = 0);

        public void GameIsReady() {
            Logger.CreateText(this, nameof(GameIsReady));
            try {
                GameIsReadyImpl();
            }
            catch (Exception exception) {
                Logger.CreateError(this, nameof(GameIsReady), exception);
            }
        }

        public void GameplayStart(int level = 0) {
            Logger.CreateText(this, nameof(GameplayStart), level);
            try {
                GameplayStartImpl(level);
            }
            catch (Exception exception) {
                Logger.CreateError(this, nameof(GameplayStart), exception);
            }
        }

        public void GameplayRestart(int level = 0) {
            Logger.CreateText(this, nameof(GameplayRestart), level);
            try {
                GameplayRestartImpl(level);
            }
            catch (Exception exception) {
                Logger.CreateError(this, nameof(GameplayRestart), exception);
            }
        }

        public void GameplayStop(int level = 0) {
            Logger.CreateText(this, nameof(GameplayStop), level);
            try {
                GameplayStopImpl(level);
            }
            catch (Exception exception) {
                Logger.CreateError(this, nameof(GameplayStop), exception);
            }
        }

    }

}
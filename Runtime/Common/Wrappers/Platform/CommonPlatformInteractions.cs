using System;

namespace MirraGames.SDK.Common {

    [Wrapper]
    public abstract partial class CommonPlatformInteractions : IPlatformInteractions {

        protected abstract void ShareGameImpl(string messageText);
        protected abstract void RateGameImpl();

        public void ShareGame(string messageText = "") {
            Logger.CreateText(this, nameof(ShareGame), messageText);
            try {
                ShareGameImpl(messageText);
            }
            catch (Exception exception) {
                Logger.CreateError(this, nameof(ShareGame), exception);
            }
        }

        public void RateGame() {
            Logger.CreateText(this, nameof(RateGame));
            try {
                RateGameImpl();
            }
            catch (Exception exception) {
                Logger.CreateError(this, nameof(RateGame), exception);
            }
        }

    }

}
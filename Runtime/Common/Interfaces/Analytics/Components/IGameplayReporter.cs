namespace MirraGames.SDK.Common {

    [Awaitable, Module]
    public partial interface IGameplayReporter {

        void GameIsReady();
        void GameplayStart(int level = 0);
        void GameplayRestart(int level = 0);
        void GameplayStop(int level = 0);

    }

}
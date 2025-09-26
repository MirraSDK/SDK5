namespace MirraGames.SDK.Common {

    [Module]
    public partial interface IPlatformInteractions {

        void ShareGame(string messageText = "");
        void RateGame();

    }

}
namespace MirraGames.SDK.Common {

    [Module]
    public partial interface IAudio {

        float Volume { get; set; }
        bool Pause { get; set; }

    }

}
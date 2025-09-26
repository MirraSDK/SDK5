namespace MirraGames.SDK.Common {

    [Awaitable, Module]
    public partial interface ILanguageInfo {

        LanguageType Current { get; }

    }

}
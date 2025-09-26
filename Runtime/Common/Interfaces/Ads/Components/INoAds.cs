namespace MirraGames.SDK.Common {

    public interface INoAds {

        bool IsNoAdsEnabled { get; }
        void EnableNoAds();
        void DisableNoAds();

    }

}
namespace MirraGames.SDK.Common {

    public interface IBanner {

        bool IsBannerReady { get; }
        bool IsBannerVisible { get; }
        bool IsBannerAvailable { get; }

        void InvokeBanner();
        void RefreshBanner();
        void DisableBanner();

    }

}
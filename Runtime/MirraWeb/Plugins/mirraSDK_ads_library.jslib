const mirraSDK_ads_library = {

    mirraSDK_ads_isBannerReady: function () {
        return Module.mirraSDK.ads.isBannerReady;
    },

    mirraSDK_ads_isBannerVisible: function () {
        return Module.mirraSDK.ads.isBannerVisible;
    },

    mirraSDK_ads_isBannerAvailable: function () {
        return Module.mirraSDK.ads.isBannerAvailable;
    },

    mirraSDK_ads_invokeBanner: function () {
        Module.mirraSDK.ads.invokeBanner();
    },

    mirraSDK_ads_refreshBanner: function () {
        Module.mirraSDK.ads.refreshBanner();
    },

    mirraSDK_ads_disableBanner: function () {
        Module.mirraSDK.ads.disableBanner();
    },

    mirraSDK_ads_isInterstitialReady: function () {
        return Module.mirraSDK.ads.isInterstitialReady;
    },

    mirraSDK_ads_isInterstitialVisible: function () {
        return Module.mirraSDK.ads.isInterstitialVisible;
    },

    mirraSDK_ads_isInterstitialAvailable: function () {
        return Module.mirraSDK.ads.isInterstitialAvailable;
    },

    mirraSDK_ads_invokeInterstitial: function (senderId, onOpenPtr, onClosePtr) {
        const onOpen = () => {
            Module.invokeMonoPCallback(senderId, onOpenPtr);
        };
        const onClose = (isSuccess) => {
            Module.invokeMonoPCallback(senderId, onClosePtr, isSuccess);
        };
        Module.mirraSDK.ads.invokeInterstitial(onOpen, onClose);
    },

    mirraSDK_ads_isRewardedReady: function () {
        return Module.mirraSDK.ads.isRewardedReady;
    },

    mirraSDK_ads_isRewardedVisible: function () {
        return Module.mirraSDK.ads.isRewardedVisible;
    },

    mirraSDK_ads_isRewardedAvailable: function () {
        return Module.mirraSDK.ads.isRewardedAvailable;
    },

    mirraSDK_ads_invokeRewarded: function (senderId, onOpenPtr, onClosePtr) {
        const onOpen = () => {
            Module.invokeMonoPCallback(senderId, onOpenPtr);
        };
        const onClose = (isSuccess) => {
            Module.invokeMonoPCallback(senderId, onClosePtr, isSuccess);
        };
        Module.mirraSDK.ads.invokeRewarded(onOpen, onClose);
    },

};
mergeInto(LibraryManager.library, mirraSDK_ads_library);
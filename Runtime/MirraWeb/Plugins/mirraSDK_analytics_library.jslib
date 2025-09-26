const mirraSDK_analytics_library = {

    mirraSDK_analytics_gameIsReady: function () {
        Module.mirraSDK.analytics.gameIsReady();
    },

    mirraSDK_analytics_gameplayStart: function (level) {
        Module.mirraSDK.analytics.gameplayStart(level);
    },

    mirraSDK_analytics_gameplayRestart: function (level) {
        Module.mirraSDK.analytics.gameplayRestart(level);
    },

    mirraSDK_analytics_gameplayStop: function (level) {
        Module.mirraSDK.analytics.gameplayStop(level);
    },

};
mergeInto(LibraryManager.library, mirraSDK_analytics_library);
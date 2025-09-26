const mirraSDK_platform_library = {

    mirraSDK_platform_current: function () {
        const platform = Module.mirraSDK.platform.current.toString();
        return Module.allocateString(platform);
    },

    mirraSDK_platform_appId: function () {
        const appId = Module.mirraSDK.platform.appId.toString();
        return Module.allocateString(appId);
    },

    mirraSDK_platform_shareGame: function (messageText_utf8) {
        const messageText = UTF8ToString(messageText_utf8);
        Module.mirraSDK.platform.shareGame(messageText);
    },

    mirraSDK_platform_rateGame: function () {
        Module.mirraSDK.platform.rateGame();
    }

};
mergeInto(LibraryManager.library, mirraSDK_platform_library);
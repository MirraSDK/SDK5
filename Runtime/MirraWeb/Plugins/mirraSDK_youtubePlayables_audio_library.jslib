const mirraSDK_youtubePlayables_audio_library = {

    mirraSDK_youtubePlayables_audio_isEnabled: function () {
        return window.ytgame.system.isAudioEnabled();
    },

    mirraSDK_youtubePlayables_audio_onEnabledChange: function (onEnabledChangePtr) {
        window.ytgame.system.onAudioEnabledChange((isAudioEnabled) => {
            Module.invokeMonoPCallback(-1, onEnabledChangePtr, isAudioEnabled);
        });
    }

};
mergeInto(LibraryManager.library, mirraSDK_youtubePlayables_audio_library);

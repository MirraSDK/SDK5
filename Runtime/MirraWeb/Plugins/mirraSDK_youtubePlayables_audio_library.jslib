// Outside a YouTube frame there is no host audio setting: the Playables SDK reports audio as off as soon as a callback is
// subscribed and never reports it again, which would silence every local test. Audio stays on there.
const mirraSDK_youtubePlayables_audio_library = {

    mirraSDK_youtubePlayables_audio_isEnabled: function () {
        const ytgame = window.ytgame;
        if (!ytgame || !ytgame.IN_PLAYABLES_ENV) return true;
        return ytgame.system.isAudioEnabled();
    },

    mirraSDK_youtubePlayables_audio_onEnabledChange: function (onEnabledChangePtr) {
        const ytgame = window.ytgame;
        if (!ytgame || !ytgame.IN_PLAYABLES_ENV) return;
        ytgame.system.onAudioEnabledChange((isAudioEnabled) => {
            Module.invokeMonoPCallback(-1, onEnabledChangePtr, isAudioEnabled);
        });
    }

};
mergeInto(LibraryManager.library, mirraSDK_youtubePlayables_audio_library);

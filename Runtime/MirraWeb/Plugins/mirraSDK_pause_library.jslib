const mirraSDK_pause_library = {

    mirraSDK_pause_isPaused: function () {
        return Module.MirraSDK.pause.isPaused;
    },

    mirraSDK_pause_onPauseChange: function (onPauseChange_ptr) {
        const onPauseChange = (isPaused) => {
            Module.invokeMonoPCallback(-1, onPauseChange_ptr, isPaused);
        };
        Module.MirraSDK.pause.onPauseChange.add(onPauseChange);
    }

};
mergeInto(LibraryManager.library, mirraSDK_pause_library);
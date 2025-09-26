const mirraSDK_language_library = {

    mirraSDK_language_current: function () {
        return Module.mirraSDK.language.current;
    },

};
mergeInto(LibraryManager.library, mirraSDK_language_library);
const mirraSDK_library = {

    createMirraSDK: function (platformSettingsJsonUTF8, onCreateInstancePtr) {
        const platformSettingsJson = UTF8ToString(platformSettingsJsonUTF8);
        const platformSettings = JSON.parse(platformSettingsJson);
        async function createAsync() {
            await Module.createMirraSDK(platformSettings);
            Module.invokeMonoPCallback(-1, onCreateInstancePtr);
        }
        createAsync();
    },

};
mergeInto(LibraryManager.library, mirraSDK_library);
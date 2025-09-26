const mirraSDK_device_library = {

    mirraSDK_device_isMobile: function () {
        return Module.mirraSDK.device.isMobile;
    },

    mirraSDK_device_systemType: function () {
        const systemType = Module.mirraSDK.device.systemType;
        return Module.allocateString(systemType);
    },

    mirraSDK_device_openUrl: function (url_ptr) {
        const url = UTF8ToString(url_ptr);
        Module.mirraSDK.device.openUrl(url);
    }

};
mergeInto(LibraryManager.library, mirraSDK_device_library);
const mirraSDK_flags_library = {

    mirraSDK_flags_getBool: function (keyUTF8, defaultValue) {
        const key = UTF8ToString(keyUTF8);
        const value = Module.mirraSDK.flags.get(key, defaultValue);
        return value === "true" || value === 1 || value === true;
    },

    mirraSDK_flags_getInt: function (keyUTF8, defaultValue) {
        const key = UTF8ToString(keyUTF8);
        return Module.mirraSDK.flags.get(key, defaultValue);
    },

    mirraSDK_flags_getFloat: function (keyUTF8, defaultValue) {
        const key = UTF8ToString(keyUTF8);
        return Module.mirraSDK.flags.get(key, defaultValue);
    },

    mirraSDK_flags_getString: function (keyUTF8, defaultValueUTF8) {
        const key = UTF8ToString(keyUTF8);
        const defaultValue = UTF8ToString(defaultValueUTF8);
        const value = Module.mirraSDK.flags.get(key, defaultValue);
        return Module.allocateString(value);
    },

    mirraSDK_flags_hasKey: function (keyUTF8) {
        const key = UTF8ToString(keyUTF8);
        return Module.mirraSDK.flags.hasKey(key);
    },

};
mergeInto(LibraryManager.library, mirraSDK_flags_library);
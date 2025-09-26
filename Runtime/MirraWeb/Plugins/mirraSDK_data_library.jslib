const mirraSDK_data_library = {

    mirraSDK_data_getBool: function (keyUTF8, defaultValue) {
        const key = UTF8ToString(keyUTF8);
        return Module.mirraSDK.data.get(key, defaultValue);
    },

    mirraSDK_data_setBool: function (keyUTF8, value, important) {
        const key = UTF8ToString(keyUTF8);
        Module.mirraSDK.data.set(key, value, important);
    },

    mirraSDK_data_getInt: function (keyUTF8, defaultValue) {
        const key = UTF8ToString(keyUTF8);
        return Module.mirraSDK.data.get(key, defaultValue);
    },

    mirraSDK_data_setInt: function (keyUTF8, value, important) {
        const key = UTF8ToString(keyUTF8);
        Module.mirraSDK.data.set(key, value, important);
    },

    mirraSDK_data_getFloat: function (keyUTF8, defaultValue) {
        const key = UTF8ToString(keyUTF8);
        return Module.mirraSDK.data.get(key, defaultValue);
    },

    mirraSDK_data_setFloat: function (keyUTF8, value, important) {
        const key = UTF8ToString(keyUTF8);
        Module.mirraSDK.data.set(key, value, important);
    },

    mirraSDK_data_getString: function (keyUTF8, defaultValueUTF8) {
        const key = UTF8ToString(keyUTF8);
        const defaultValue = UTF8ToString(defaultValueUTF8);
        const value = Module.mirraSDK.data.get(key, defaultValue);
        return Module.allocateString(value);
    },


    mirraSDK_data_setString: function (keyUTF8, valueUTF8, important) {
        const key = UTF8ToString(keyUTF8);
        const value = UTF8ToString(valueUTF8);
        Module.mirraSDK.data.set(key, value, important);
    },


    mirraSDK_data_save: function () {
        Module.mirraSDK.data.save();
    },

    mirraSDK_data_hasKey: function (keyUTF8) {
        const key = UTF8ToString(keyUTF8);
        return Module.mirraSDK.data.hasKey(key);
    },

    mirraSDK_data_deleteKey: function (keyUTF8) {
        const key = UTF8ToString(keyUTF8);
        Module.mirraSDK.data.deleteKey(key);
    },

    mirraSDK_data_deleteAll: function () {
        Module.mirraSDK.data.deleteAll();
    },

};
mergeInto(LibraryManager.library, mirraSDK_data_library);
const mirraSDK_player_library = {

    mirraSDK_player_displayName: function () {
        const displayName = Module.mirraSDK.player.displayName;
        return Module.allocateString(displayName);
    },

    mirraSDK_player_firstName: function () {
        const firstName = Module.mirraSDK.player.firstName;
        return Module.allocateString(firstName);
    },

    mirraSDK_player_lastName: function () {
        const lastName = Module.mirraSDK.player.lastName;
        return Module.allocateString(lastName);
    },

    mirraSDK_player_username: function () {
        const username = Module.mirraSDK.player.username;
        return Module.allocateString(username);
    },

    mirraSDK_player_uniqueId: function () {
        const uniqueId = Module.mirraSDK.player.uniqueId;
        return Module.allocateString(uniqueId);
    },

    mirraSDK_player_isLoggedIn: function () {
        return Module.mirraSDK.player.isLoggedIn;
    },

    mirraSDK_player_invokeLogin: function (senderId, onSuccessPtr, onErrorPtr) {
        const onSuccess = () => {
            Module.invokeMonoPCallback(senderId, onSuccessPtr);
        };
        const onError = () => {
            Module.invokeMonoPCallback(senderId, onErrorPtr);
        };
        Module.mirraSDK.player.invokeLogin(onSuccess, onError);
    },

};
mergeInto(LibraryManager.library, mirraSDK_player_library);
const mirraSDK_achievements_library = {

    mirraSDK_achievements_happyTime: function () {
        Module.mirraSDK.achievements.happyTime();
    },

    mirraSDK_achievements_unlock: function (achievementId_utf8) {
        const achievementId = UTF8ToString(achievementId_utf8);
        Module.mirraSDK.achievements.unlock(achievementId);
    },

    mirraSDK_achievements_getScore: function (senderId, boardId_utf8, onScore_ptr) {
        const onScore = (score) => {
            Module.invokeMonoPCallback(senderId, onScore_ptr, score);
        };
        const boardId = UTF8ToString(boardId_utf8);
        (async () => {
            const score = await Module.mirraSDK.achievements.getScore(boardId);
            onScore(score);
        })();
    },

    mirraSDK_achievements_setScore: function (boardId_utf8, score) {
        const boardId = UTF8ToString(boardId_utf8);
        Module.mirraSDK.achievements.setScore(boardId, score);
    },

    mirraSDK_achievements_getLeaderboard: function (senderId, boardId_utf8, onLeaderboard_ptr) {
        const onLeaderboard = (leaderboard) => {
            const json = JSON.stringify(leaderboard);
            const json_utf8 = Module.allocateString(json);
            Module.invokeMonoPCallback(senderId, onLeaderboard_ptr, json_utf8);
        };
        const boardId = UTF8ToString(boardId_utf8);
        (async () => {
            const leaderboard = await Module.mirraSDK.achievements.getLeaderboard(boardId);
            onLeaderboard(leaderboard);
        })();
    }

};
mergeInto(LibraryManager.library, mirraSDK_achievements_library);
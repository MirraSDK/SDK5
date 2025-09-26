using System;

namespace MirraGames.SDK.Common {

    [Awaitable, Module]
    public partial interface IAchievements {

        void HappyTime();
        void Unlock(string achievementId);
        void GetScore(string boardId, Action<int> onScore);
        void SetScore(string boardId, int score);
        void GetLeaderboard(string boardId, Action<Leaderboard> onLeaderboard);

    }

}
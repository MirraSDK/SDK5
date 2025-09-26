using MirraGames.SDK.Common;
using System;

namespace MirraGames.SDK.Fallback {

    [Provider(typeof(IAchievements))]
    public class FallbackAchievements : CommonAchievements {

        public FallbackAchievements() {
            SetInitialized();
        }

        protected override void HappyTimeImpl() {
            Logger.NotImplementedWarning(this, nameof(HappyTimeImpl));
        }

        protected override void UnlockImpl(string achievementId) {
            Logger.NotImplementedWarning(this, nameof(UnlockImpl));
        }

        protected override void GetScoreImpl(string boardId, Action<int> onScore) {
            Logger.NotImplementedWarning(this, nameof(GetScoreImpl));
            onScore?.Invoke(default);
        }

        protected override void SetScoreImpl(string boardId, int score) {
            Logger.NotImplementedWarning(this, nameof(SetScoreImpl));
        }

        protected override void GetLeaderboardImpl(string boardId, Action<Leaderboard> onLeaderboard) {
            Logger.NotImplementedWarning(this, nameof(GetLeaderboardImpl));
            onLeaderboard?.Invoke(default);
        }

    }

}
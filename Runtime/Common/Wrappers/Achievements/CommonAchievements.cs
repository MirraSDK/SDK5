using System;

namespace MirraGames.SDK.Common {

    [Awaitable, Wrapper]
    public abstract partial class CommonAchievements : IAchievements {

        protected abstract void HappyTimeImpl();
        protected abstract void UnlockImpl(string achievementId);
        protected abstract void GetScoreImpl(string boardId, Action<int> onScore);
        protected abstract void SetScoreImpl(string boardId, int score);
        protected abstract void GetLeaderboardImpl(string boardId, Action<Leaderboard> onLeaderboard);

        public void HappyTime() {
            Logger.CreateText(this, nameof(HappyTime));
            try {
                HappyTimeImpl();
            }
            catch (Exception exception) {
                Logger.CreateError(this, nameof(HappyTime), exception);
            }
        }

        public void Unlock(string achievementId) {
            Logger.CreateText(this, nameof(Unlock), achievementId);
            try {
                UnlockImpl(achievementId);
            }
            catch (Exception exception) {
                Logger.CreateError(this, nameof(Unlock), exception, achievementId);
            }
        }

        public void GetScore(string boardId, Action<int> onScore) {
            Logger.CreateText(this, nameof(GetScore), boardId);
            try {
                GetScoreImpl(boardId, onScore);
            }
            catch (Exception exception) {
                Logger.CreateError(this, nameof(GetScore), exception, boardId);
            }
        }

        public void SetScore(string boardId, int score) {
            Logger.CreateText(this, nameof(SetScore), boardId, score);
            try {
                SetScoreImpl(boardId, score);
            }
            catch (Exception exception) {
                Logger.CreateError(this, nameof(SetScore), exception, boardId, score);
            }
        }

        public void GetLeaderboard(string boardId, Action<Leaderboard> onLeaderboard) {
            Logger.CreateText(this, nameof(GetLeaderboard), boardId);
            try {
                GetLeaderboardImpl(boardId, onLeaderboard);
            }
            catch (Exception exception) {
                Logger.CreateError(this, nameof(GetLeaderboard), exception, boardId);
            }
        }

    }

}
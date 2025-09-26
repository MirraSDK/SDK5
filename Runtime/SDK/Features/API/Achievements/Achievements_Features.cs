using MirraGames.SDK.SourceGenerator;
using MirraGames.SDK.Common;
using UnityEngine;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Features {

    public class Achievements_Features : FeaturesContainer {

        public new class UxmlFactory : UxmlFactory<Achievements_Features> { }

        public Achievements_Features() {
            SetInfo("Achievements", nameof(IAchievements), nameof(AchievementsProvider));

            CreateButton(nameof(IAchievements.HappyTime), () => {
                MirraSDK.Achievements.HappyTime();
            });
            CreateButton(nameof(IAchievements.Unlock), () => {
                MirraSDK.Achievements.Unlock("example_achievement");
            });
            CreateButton(nameof(IAchievements.GetScore), () => {
                MirraSDK.Achievements.GetScore("score", score => {

                });
            });
            CreateButton(nameof(IAchievements.SetScore), () => {
                MirraSDK.Achievements.SetScore("score", Random.Range(0, 1000));
            });
            CreateButton(nameof(IAchievements.GetLeaderboard), () => {
                MirraSDK.Achievements.GetLeaderboard("score", leaderboard => {

                });
            });
        }

    }

}
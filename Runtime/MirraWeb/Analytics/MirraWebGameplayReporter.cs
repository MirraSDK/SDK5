using MirraGames.SDK.Common;
using System.Runtime.InteropServices;

namespace MirraGames.SDK.MirraWeb {

    [Provider(typeof(IGameplayReporter))]
    public class MirraWebGameplayReporter : CommonGameplayReporter {

        [DllImport(Naming.InternalDll)] private static extern void mirraSDK_analytics_gameIsReady();
        [DllImport(Naming.InternalDll)] private static extern void mirraSDK_analytics_gameplayStart(int level);
        [DllImport(Naming.InternalDll)] private static extern void mirraSDK_analytics_gameplayRestart(int level);
        [DllImport(Naming.InternalDll)] private static extern void mirraSDK_analytics_gameplayStop(int level);

        public MirraWebGameplayReporter() {
            SetInitialized();
        }

        protected override void GameIsReadyImpl() {
            mirraSDK_analytics_gameIsReady();
        }

        protected override void GameplayStartImpl(int level = 0) {
            mirraSDK_analytics_gameplayStart(level);
        }

        protected override void GameplayRestartImpl(int level = 0) {
            mirraSDK_analytics_gameplayRestart(level);
        }

        protected override void GameplayStopImpl(int level = 0) {
            mirraSDK_analytics_gameplayStop(level);
        }

    }

}
using UnityEngine;

namespace MirraGames.SDK.Common {

    public class PreventSleep : MonoBehaviour {

        private void Awake() {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            DontDestroyOnLoad(gameObject);
        }

        private void OnApplicationPause(bool pauseStatus) {
            if (pauseStatus) {
                Screen.sleepTimeout = SleepTimeout.SystemSetting;
            }
            else {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }
        }

        private void OnApplicationFocus(bool hasFocus) {
            if (!hasFocus) {
                Screen.sleepTimeout = SleepTimeout.SystemSetting;
            }
            else {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }
        }

        private void OnApplicationQuit() {
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
        }

    }

}
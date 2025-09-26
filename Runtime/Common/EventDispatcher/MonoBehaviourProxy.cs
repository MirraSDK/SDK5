using System;
using UnityEngine;

namespace MirraGames.SDK.Common {

    public class MonoBehaviourProxy : MonoBehaviour {

        public event Action StartEvent = () => { };
        private void Start() => StartEvent.Invoke();

        public event Action UpdateEvent = () => { };
        private void Update() => UpdateEvent.Invoke();

        public event Action<bool> OnApplicationFocusEvent = _ => { };
        private void OnApplicationFocus(bool focus) => OnApplicationFocusEvent.Invoke(focus);

        public event Action<bool> OnApplicationPauseEvent = _ => { };
        private void OnApplicationPause(bool pause) => OnApplicationPauseEvent.Invoke(pause);

        public event Action OnGUIEvent = () => { };
        private void OnGUI() => OnGUIEvent.Invoke();

        public event Action LateUpdateEvent = () => { };
        private void LateUpdate() => LateUpdateEvent.Invoke();

        public event Action OnDestroyEvent = () => { };
        private void OnDestroy() => OnDestroyEvent.Invoke();

    }

}
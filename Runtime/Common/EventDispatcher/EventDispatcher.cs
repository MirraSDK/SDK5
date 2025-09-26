using System;
using System.Collections;
using UnityEngine;

namespace MirraGames.SDK.Common {

    public class EventDispatcher : IEventDispatcher {

        private readonly MonoBehaviourProxy proxy;

        public EventDispatcher(string name) {
            GameObject gameObject = new(name);
            proxy = gameObject.AddComponent<MonoBehaviourProxy>();
            GameObject.DontDestroyOnLoad(gameObject);
        }

        public event Action Start {
            add => proxy.StartEvent += value;
            remove => proxy.StartEvent -= value;
        }

        public event Action OnGUI {
            add => proxy.OnGUIEvent += value;
            remove => proxy.OnGUIEvent -= value;
        }

        public event Action LateUpdate {
            add => proxy.LateUpdateEvent += value;
            remove => proxy.LateUpdateEvent -= value;
        }

        public event Action<bool> OnApplicationFocus {
            add => proxy.OnApplicationFocusEvent += value;
            remove => proxy.OnApplicationFocusEvent -= value;
        }

        public event Action<bool> OnApplicationPause {
            add => proxy.OnApplicationPauseEvent += value;
            remove => proxy.OnApplicationPauseEvent -= value;
        }

        public event Action OnDestroy {
            add => proxy.OnDestroyEvent += value;
            remove => proxy.OnDestroyEvent -= value;
        }

        public Coroutine StartCoroutine(IEnumerator routine) {
            return proxy.StartCoroutine(routine);
        }

        public void StopCoroutine(Coroutine coroutine) {
            proxy.StopCoroutine(coroutine);
        }

        public void StopAllCoroutines() {
            proxy.StopAllCoroutines();
        }

    }

}
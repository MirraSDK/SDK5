using System;
using System.Collections;
using UnityEngine;

namespace MirraGames.SDK.Common {

    public interface IEventDispatcher {

        public event Action Start;
        public event Action OnGUI;
        public event Action LateUpdate;
        public event Action<bool> OnApplicationFocus;
        public event Action<bool> OnApplicationPause;
        public event Action OnDestroy;

        public Coroutine StartCoroutine(IEnumerator routine);
        public void StopCoroutine(Coroutine coroutine);
        public void StopAllCoroutines();

    }

}
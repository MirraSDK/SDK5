using System;
using UnityEngine;

namespace MirraGames.SDK.Common {

    [Serializable]
    public record KeyFloatPair {
        public string key;
        public float value = 0.0f;
    }

}
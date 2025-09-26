using System;

namespace MirraGames.SDK.Common {

    [Serializable]
    public record KeyBoolPair {
        public string key;
        public bool value = false;
    }

}
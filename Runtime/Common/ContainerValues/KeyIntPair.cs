using System;

namespace MirraGames.SDK.Common {

    [Serializable]
    public record KeyIntPair {
        public string key;
        public int value = 0;
    }

}
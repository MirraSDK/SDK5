using System;

namespace MirraGames.SDK.Common {

    [Serializable]
    public record KeyStringPair {
        public string key;
        public string value = string.Empty;
    }

}
using System;

namespace MirraGames.SDK.Common {

    [Serializable]
    public record KeySerializableGroupPair {
        public string key;
        public SerializableGroup group;
    }

}
using System;
using System.Collections.Generic;

namespace MirraGames.SDK.Common {

    [Serializable]
    public record ContainerValues {
        public List<KeyBoolPair> boolValues = new();
        public List<KeyIntPair> intValues = new();
        public List<KeyFloatPair> floatValues = new();
        public List<KeyStringPair> stringValues = new();
    }

}
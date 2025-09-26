using System.Collections.Generic;

namespace MirraGames.SDK.Common {

    public class DictionaryGroup : IPreferencesGroup {

        public static DictionaryGroup Empty { get; } = new();

        public readonly Dictionary<string, bool> boolCollection = new();
        public readonly Dictionary<string, int> intCollection = new();
        public readonly Dictionary<string, float> floatCollection = new();
        public readonly Dictionary<string, string> stringCollection = new();

        public DictionaryGroup() { }

        public DictionaryGroup(SerializableGroup serializableGroup) {
            InjectKeyValuePairs(serializableGroup);
        }

        public void InjectKeyValuePairs(SerializableGroup serializableGroup) {
            InjectBoolCollection(serializableGroup);
            InjectIntCollection(serializableGroup);
            InjectFloatCollection(serializableGroup);
            InjectStringCollection(serializableGroup);
        }

        public bool GetBool(string key, bool defaultValue) {
            if (boolCollection.TryGetValue(key, out bool value)) {
                return value;
            }
            return defaultValue;
        }

        public int GetInt(string key, int defaultValue) {
            if (intCollection.TryGetValue(key, out int value)) {
                return value;
            }
            return defaultValue;
        }

        public float GetFloat(string key, float defaultValue) {
            if (floatCollection.TryGetValue(key, out float value)) {
                return value;
            }
            return defaultValue;
        }

        public string GetString(string key, string defaultValue) {
            if (stringCollection.TryGetValue(key, out string value)) {
                return value;
            }
            return defaultValue;
        }

        internal void InjectBoolCollection(SerializableGroup serializableGroup) {
            boolCollection.Clear();
            if (serializableGroup.boolCollection == null) {
                return;
            }
            foreach (KeyBoolPair pair in serializableGroup.boolCollection) {
                if (pair == null || string.IsNullOrEmpty(pair.key)) {
                    Logger.CreateError(this, "Invalid pair found");
                    continue;
                }
                if (boolCollection.ContainsKey(pair.key)) {
                    Logger.CreateError(this, "Duplicate found", pair.key);
                    continue;
                }
                boolCollection.Add(pair.key, pair.value);
            }
        }

        internal void InjectIntCollection(SerializableGroup serializableGroup) {
            intCollection.Clear();
            if (serializableGroup.intCollection == null) {
                return;
            }
            foreach (KeyIntPair pair in serializableGroup.intCollection) {
                if (pair == null || string.IsNullOrEmpty(pair.key)) {
                    Logger.CreateError(this, "Invalid pair found");
                    continue;
                }
                if (intCollection.ContainsKey(pair.key)) {
                    Logger.CreateError(this, "Duplicate found", pair.key);
                    continue;
                }
                intCollection.Add(pair.key, pair.value);
            }
        }

        internal void InjectFloatCollection(SerializableGroup serializableGroup) {
            floatCollection.Clear();
            if (serializableGroup.floatCollection == null) {
                return;
            }
            foreach (KeyFloatPair pair in serializableGroup.floatCollection) {
                if (pair == null || string.IsNullOrEmpty(pair.key)) {
                    Logger.CreateError(this, "Invalid pair found");
                    continue;
                }
                if (floatCollection.ContainsKey(pair.key)) {
                    Logger.CreateError(this, "Duplicate found", pair.key);
                    continue;
                }
                floatCollection.Add(pair.key, pair.value);
            }
        }

        internal void InjectStringCollection(SerializableGroup serializableGroup) {
            stringCollection.Clear();
            if (serializableGroup.stringCollection == null) {
                return;
            }
            foreach (KeyStringPair pair in serializableGroup.stringCollection) {
                if (pair == null || string.IsNullOrEmpty(pair.key)) {
                    Logger.CreateError(this, "Invalid pair found");
                    continue;
                }
                if (stringCollection.ContainsKey(pair.key)) {
                    Logger.CreateError(this, "Duplicate found", pair.key);
                    continue;
                }
                stringCollection.Add(pair.key, pair.value);
            }
        }

    }

}
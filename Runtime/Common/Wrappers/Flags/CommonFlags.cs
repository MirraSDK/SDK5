using System;
using System.Collections.Generic;
using UnityEngine;

namespace MirraGames.SDK.Common {

    [Awaitable, Wrapper]
    public abstract partial class CommonFlags : IFlags {

        private readonly Dictionary<string, bool> boolCollection = new();
        private readonly Dictionary<string, int> intCollection = new();
        private readonly Dictionary<string, float> floatCollection = new();
        private readonly Dictionary<string, string> stringCollection = new();

        protected abstract void ReadJson(Action<string> jsonRequest);

        protected void ParseContainers(string json) {
            Logger.CreateText(this, nameof(ParseContainers), json);
            ContainerValues containerValues = JsonUtility.FromJson<ContainerValues>(json) ?? new();
            ConvertContainerToDictionaries(containerValues);
        }

        protected void ConvertContainerToDictionaries(ContainerValues containerValues) {
            boolCollection.Clear();
            foreach (KeyBoolPair pair in containerValues.boolValues) {
                boolCollection.Add(pair.key, pair.value);
            }
            intCollection.Clear();
            foreach (KeyIntPair pair in containerValues.intValues) {
                intCollection.Add(pair.key, pair.value);
            }
            floatCollection.Clear();
            foreach (KeyFloatPair pair in containerValues.floatValues) {
                floatCollection.Add(pair.key, pair.value);
            }
            stringCollection.Clear();
            foreach (KeyStringPair pair in containerValues.stringValues) {
                stringCollection.Add(pair.key, pair.value);
            }
        }

        public bool GetBool(string key, bool defaultValue = false) {
            return boolCollection.GetValueOrDefault(key, defaultValue);
        }

        public int GetInt(string key, int defaultValue = 0) {
            return intCollection.GetValueOrDefault(key, defaultValue);
        }

        public float GetFloat(string key, float defaultValue = 0.0f) {
            return floatCollection.GetValueOrDefault(key, defaultValue);
        }

        public string GetString(string key, string defaultValue = "") {
            return stringCollection.GetValueOrDefault(key, defaultValue);
        }

        public bool HasKey(string key) {
            return boolCollection.ContainsKey(key)
                || intCollection.ContainsKey(key)
                || floatCollection.ContainsKey(key)
                || stringCollection.ContainsKey(key);
        }

    }

}
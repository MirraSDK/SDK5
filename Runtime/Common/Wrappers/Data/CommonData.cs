using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MirraGames.SDK.Common {

    [Awaitable, Wrapper]
    public abstract partial class CommonData : IData {

        protected const string ContainerName = "json-data";
        protected virtual float SaveCheckDelay { get; } = 3.0f;

        private readonly Dictionary<string, bool> boolCollection = new();
        private readonly Dictionary<string, int> intCollection = new();
        private readonly Dictionary<string, float> floatCollection = new();
        private readonly Dictionary<string, string> stringCollection = new();

        protected readonly IEventDispatcher eventDispatcher;
        private readonly WaitForEndOfFrame waitForEndOfFrame;
        private readonly WaitForSecondsRealtime waitForSecondsRealtime;
        private readonly Coroutine saveCoroutine;

        private bool isSaveRequested = false;

        public CommonData(IEventDispatcher eventDispatcher) {
            this.eventDispatcher = eventDispatcher;
            waitForEndOfFrame = new WaitForEndOfFrame();
            waitForSecondsRealtime = new WaitForSecondsRealtime(SaveCheckDelay);
            saveCoroutine = eventDispatcher.StartCoroutine(SaveCoroutine());
        }

        protected abstract void ReadJson(Action<string> jsonRequest);
        protected abstract void WriteJson(string json);

        private IEnumerator SaveCoroutine() {
            while (true) {
                if (isSaveRequested) {
                    WriteSaves();
                    isSaveRequested = false;
                    yield return waitForSecondsRealtime;
                }
                yield return waitForEndOfFrame;
            }
        }

        private void WriteSaves() {
            ContainerValues containerValues = ConvertDictionariesToContainer();
            string json = JsonUtility.ToJson(containerValues);
            try {
                WriteJson(json);
            }
            catch (Exception exception) {
                Logger.CreateError(this, nameof(WriteSaves), exception);
            }
        }

        protected void ParseContainers(string json) {
            Logger.CreateText(this, nameof(ParseContainers), json);
            ContainerValues containerValues = JsonUtility.FromJson<ContainerValues>(json) ?? new();
            ConvertContainerToDictionaries(containerValues);
            SetInitialized();
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

        protected ContainerValues ConvertDictionariesToContainer() {
            ContainerValues containerValues = new();
            foreach (KeyValuePair<string, bool> pair in boolCollection) {
                KeyBoolPair keyBoolPair = new() {
                    key = pair.Key,
                    value = pair.Value
                };
                containerValues.boolValues.Add(keyBoolPair);
            }
            foreach (KeyValuePair<string, int> pair in intCollection) {
                KeyIntPair keyIntPair = new() {
                    key = pair.Key,
                    value = pair.Value
                };
                containerValues.intValues.Add(keyIntPair);
            }
            foreach (KeyValuePair<string, float> pair in floatCollection) {
                KeyFloatPair keyFloatPair = new() {
                    key = pair.Key,
                    value = pair.Value
                };
                containerValues.floatValues.Add(keyFloatPair);
            }
            foreach (KeyValuePair<string, string> pair in stringCollection) {
                KeyStringPair keyStringPair = new() {
                    key = pair.Key,
                    value = pair.Value
                };
                containerValues.stringValues.Add(keyStringPair);
            }
            return containerValues;
        }

        public bool GetBool(string key, bool defaultValue = false) {
            return boolCollection.GetValueOrDefault(key, defaultValue);
        }

        public void SetBool(string key, bool writeValue, bool important = true) {
            boolCollection[key] = writeValue;
            if (important) Save();
        }

        public int GetInt(string key, int defaultValue = 0) {
            return intCollection.GetValueOrDefault(key, defaultValue);
        }

        public void SetInt(string key, int writeValue, bool important = true) {
            intCollection[key] = writeValue;
            if (important) Save();
        }

        public float GetFloat(string key, float defaultValue = 0.0f) {
            return floatCollection.GetValueOrDefault(key, defaultValue);
        }

        public void SetFloat(string key, float writeValue, bool important = true) {
            floatCollection[key] = writeValue;
            if (important) Save();
        }

        public string GetString(string key, string defaultValue = "") {
            return stringCollection.GetValueOrDefault(key, defaultValue);
        }

        public void SetString(string key, string writeValue, bool important = true) {
            stringCollection[key] = writeValue;
            if (important) Save();
        }

        public TSerializable GetObject<TSerializable>(string key, TSerializable defaultValue = default) {
            string json = stringCollection.GetValueOrDefault(key, string.Empty);
            return string.IsNullOrEmpty(json) ? defaultValue : JsonUtility.FromJson<TSerializable>(json);
        }

        public void SetObject<TSerializable>(string key, TSerializable writeValue, bool important = true) {
            string json = JsonUtility.ToJson(writeValue);
            stringCollection[key] = json;
            if (important) Save();
        }

        public void Save() {
            isSaveRequested = true;
        }

        public bool HasKey(string key) {
            return boolCollection.ContainsKey(key)
                || intCollection.ContainsKey(key)
                || floatCollection.ContainsKey(key)
                || stringCollection.ContainsKey(key);
        }

        public void DeleteKey(string key) {
            boolCollection.Remove(key);
            intCollection.Remove(key);
            floatCollection.Remove(key);
            stringCollection.Remove(key);
            Save();
        }

        public void DeleteAll() {
            boolCollection.Clear();
            intCollection.Clear();
            floatCollection.Clear();
            stringCollection.Clear();
            Save();
        }

    }

}
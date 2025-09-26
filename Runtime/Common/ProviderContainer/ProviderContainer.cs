using System;
using System.Collections.Generic;
using System.Linq;

namespace MirraGames.SDK.Common {

    public class ProviderContainer : IProviderContainer {

        private readonly struct ProviderInfo {
            public readonly Type type;
            public readonly Enum provider;
            public ProviderInfo(Type type, Enum provider) {
                this.type = type;
                this.provider = provider;
            }
        }

        private readonly Dictionary<ProviderInfo, object> providersCollection = new();

        public bool Contains<T>() {
            return providersCollection.Any(x => x.Key.type == typeof(T));
        }

        public void Register<T>(T instance, Enum provider = null) {
            Type type = typeof(T);
            if (instance is null) {
                Logger.CreateError(this, $"Attempt to register {type.Name} as null");
            }
            ProviderInfo key = new(type, provider);
            providersCollection[key] = instance;
        }

        public void Register(Type type, object instance, Enum provider = null) {
            if (type == null) {
                throw new ArgumentNullException(nameof(type));
            }
            if (instance == null) {
                Logger.CreateError(this, $"Attempt to register {type.Name} as null");
            }
            if (!type.IsInstanceOfType(instance)) {
                throw new ArgumentException($"Instance is not of type {type.FullName}", nameof(instance));
            }
            ProviderInfo key = new(type, provider);
            providersCollection[key] = instance;
        }

        public T First<T>(T defaultValue = default) {
            foreach (var providerInfo in providersCollection.Keys) {
                if (providerInfo.type == typeof(T)) {
                    return (T)providersCollection[providerInfo];
                }
            }
            Logger.CreateWarning(this, typeof(T), "not found");
            return defaultValue;
        }

        public T Find<T>(Enum provider, T defaultValue = default) {
            foreach (var providerInfo in providersCollection.Keys) {
                if (providerInfo.provider == provider) {
                    return (T)providersCollection[providerInfo];
                }
            }
            Logger.CreateError(this, $"Type {typeof(T)} as {provider} not found");
            return defaultValue;
        }

    }

}
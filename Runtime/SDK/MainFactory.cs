using MirraGames.SDK.SourceGenerator;
using MirraGames.SDK.Common;
using System;

namespace MirraGames.SDK {

    internal partial class MainFactory {

        public string ConfigurationName { get; private set; }

        public PreferencesReader CreatePreferencesReader() {
            if (providerContainer.Contains<PreferencesReader>()) {
                return providerContainer.First<PreferencesReader>();
            }
            PreferencesReader preferencesReader = PreferencesReader.CreateReader();
            ConfigurationName = preferencesReader.GetConfigurationName();
            Configuration configurationInstance = Mapping.CreateConfigurationInstance(ConfigurationName);
            DictionaryGroup defaultValueGroup = configurationInstance.GetDefaultValuesGroup();
            preferencesReader.InjectDefaultValueGroup(ConfigurationName, defaultValueGroup);
            foreach (Type providerType in Mapping.ProviderConfigurations.Keys) {
                Type providerConfigurationType = Mapping.ProviderConfigurations[providerType];
                Type interfaceType = Mapping.ProviderToInterface[providerType];
                Type rootType = Mapping.InterfaceToRoot[interfaceType];
                string rootName = rootType.Name;
                bool isOverrideModuleEnabled = preferencesReader.IsOverrideModuleEnabled(ConfigurationName, rootName);
                object providerConfigurationInstance = GetProviderConfiguration(preferencesReader, ConfigurationName, providerType, !isOverrideModuleEnabled);
                providerContainer.Register(providerConfigurationType, providerConfigurationInstance);
            }
            providerContainer.Register<PreferencesReader>(preferencesReader);
            return preferencesReader;
        }

        public static object GetProviderConfiguration(PreferencesReader preferencesReader, string configurationName, Type providerType, bool returnDefault) {
            Logger.CreateText(nameof(PreferencesReader), "Reading", providerType.Name, "as default", returnDefault);
            if (returnDefault) {
                return Mapping.CreateProviderConfigurationInstance(providerType.Name);
            }
            Type propertyGroupType = Mapping.ProviderConfigurations[providerType];
            PropertyGroup propertyGroup = preferencesReader.GetPreferencesPropertyGroup(configurationName, propertyGroupType);
            if (propertyGroup != null) {
                Logger.CreateText(nameof(PreferencesReader), "Applying custom", propertyGroup.Name, "values");
                return propertyGroup;
            }
            Logger.CreateWarning(nameof(PreferencesReader), "Fallback to default", providerType.Name, "values");
            return Mapping.CreateProviderConfigurationInstance(providerType.Name);
        }

        public IEventAggregator CreateEventAggregator() {
            if (providerContainer.Contains<IEventAggregator>()) {
                return providerContainer.First<IEventAggregator>();
            }
            IEventAggregator eventAggregator = new EventAggregator();
            providerContainer.Register<IEventAggregator>(eventAggregator);
            return eventAggregator;
        }

        public IEventDispatcher CreateEventDispatcher() {
            if (providerContainer.Contains<IEventDispatcher>()) {
                return providerContainer.First<IEventDispatcher>();
            }
            IEventDispatcher eventDispatcher = new EventDispatcher(nameof(MirraSDK));
            providerContainer.Register<IEventDispatcher>(eventDispatcher);
            return eventDispatcher;
        }

    }

}
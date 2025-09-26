namespace MirraGames.SDK.Common {

    public static class ConfigurationExtensions {

        public static DictionaryGroup GetDefaultValuesGroup(this Configuration configuration) {
            if (configuration == null) {
                return DictionaryGroup.Empty;
            }
            return configuration.GetDefaultValuesGroup();
        }

    }

}
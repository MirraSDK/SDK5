namespace MirraGames.SDK.Common {

    public interface IDataContainer {

        bool GetBool(string key, bool defaultValue = false);
        void SetBool(string key, bool writeValue, bool important = true);

        int GetInt(string key, int defaultValue = 0);
        void SetInt(string key, int writeValue, bool important = true);

        float GetFloat(string key, float defaultValue = 0.0f);
        void SetFloat(string key, float writeValue, bool important = true);

        string GetString(string key, string defaultValue = "");
        void SetString(string key, string writeValue, bool important = true);

        TSerializable GetObject<TSerializable>(string key, TSerializable defaultValue = default);
        void SetObject<TSerializable>(string key, TSerializable writeValue, bool important = true);

        void Save();
        bool HasKey(string key);
        void DeleteKey(string key);
        void DeleteAll();

    }

}
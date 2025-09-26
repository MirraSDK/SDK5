namespace MirraGames.SDK.Common {

    [Awaitable, Module]
    public partial interface IFlags {

        bool GetBool(string key, bool defaultValue = false);
        int GetInt(string key, int defaultValue = 0);
        float GetFloat(string key, float defaultValue = 0.0f);
        string GetString(string key, string defaultValue = "");
        bool HasKey(string key);

    }

}
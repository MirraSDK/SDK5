using System;

namespace MirraGames.SDK.Editor
{
    [Serializable]
    public class PackageDependencies
    {
        public string[] GitUrls;
        public string[] TarballUrls;
    }
}
using System;

namespace MirraGames.SDK.Editor
{
    [Serializable]
    public class PackageDependencies
    {
        public string[] UnityPackages;
        public string[] GitUrls;
        public string[] TarballUrls;
    }
}
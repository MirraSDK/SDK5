using System;

namespace MirraGames.SDK.Editor
{
    /// <summary>
    /// Contents of a package repository's dependencies.json.
    /// </summary>
    [Serializable]
    public class PackageDependencies
    {
        public string[] UnityPackages;
        public string[] GitUrls;
        public string[] TarballUrls;

        /// <summary>
        /// Scoped registries the project manifest needs before the registry packages can resolve.
        /// </summary>
        public ScopedRegistry[] ScopedRegistries;

        /// <summary>
        /// Packages installed from a registry at the given version or newer.
        /// </summary>
        public RegistryPackage[] RegistryPackages;

        /// <summary>
        /// Registry packages the developer may add or remove one by one.
        /// </summary>
        public OptionalRegistryPackage[] OptionalRegistryPackages;

        public bool HasRegistryPackages
        {
            get => RegistryPackages != null && RegistryPackages.Length > 0;
        }
    }

    [Serializable]
    public class ScopedRegistry
    {
        public string Name;
        public string Url;
        public string[] Scopes;
    }

    [Serializable]
    public class RegistryPackage
    {
        public string Name;
        public string Version;
    }

    [Serializable]
    public class OptionalRegistryPackage
    {
        public string Name;
        public string Version;

        /// <summary>
        /// Text shown on the install and remove buttons.
        /// </summary>
        public string Label;
    }
}

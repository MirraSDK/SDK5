using MirraGames.SDK.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Logger = MirraGames.SDK.Common.Logger;

namespace MirraGames.SDK.Editor
{
    public static class UnityPackageManager
    {
        private const string ExternalPackagesFolder = "ExternalPackages";
        private const int DownloadTimeoutSeconds = 120;
        private const int UpmTimeoutSeconds = 300;
        private const int PollIntervalMs = 100;

        public static async Task ImportFromUnityPackage(string unityPackageUrl)
        {
            Logger.CreateText(nameof(UnityPackageManager), nameof(ImportFromUnityPackage), "Downloading", Naming.Quote(unityPackageUrl));

            string projectPath = GetProjectPath();
            string externalPackagesPath = Path.Combine(projectPath, ExternalPackagesFolder);

            if (!Directory.Exists(externalPackagesPath))
            {
                Directory.CreateDirectory(externalPackagesPath);
            }

            string fileName = GetFileNameFromUrl(unityPackageUrl, ".unitypackage");
            string localFilePath = Path.Combine(externalPackagesPath, fileName).Replace('\\', '/');

            if (File.Exists(localFilePath))
            {
                File.Delete(localFilePath);
            }

            try
            {
                DownloadHandlerFile downloadHandler = new(localFilePath)
                {
                    removeFileOnAbort = true
                };

                using UnityWebRequest request = new(unityPackageUrl, UnityWebRequest.kHttpVerbGET)
                {
                    downloadHandler = downloadHandler,
                    redirectLimit = 10,
                    timeout = DownloadTimeoutSeconds
                };

                UnityWebRequestAsyncOperation operation = request.SendWebRequest();
                DateTime timeoutTime = DateTime.UtcNow.AddSeconds(DownloadTimeoutSeconds);

                while (!operation.isDone && DateTime.UtcNow < timeoutTime)
                {
                    float progress = request.downloadProgress;
                    if (EditorUtility.DisplayCancelableProgressBar(
                        "Importing Package",
                        $"Downloading {fileName}... {progress * 100f:F0}%",
                        progress))
                    {
                        request.Abort();
                        EditorUtility.ClearProgressBar();
                        Logger.CreateWarning(nameof(UnityPackageManager), nameof(ImportFromUnityPackage), "Download cancelled by user");
                        return;
                    }
                    await Task.Delay(PollIntervalMs);
                }

                if (!operation.isDone)
                {
                    request.Abort();
                    EditorUtility.ClearProgressBar();
                    Logger.CreateError(nameof(UnityPackageManager), nameof(ImportFromUnityPackage), "Download timed out", Naming.Quote(unityPackageUrl));
                    return;
                }

                if (request.result != UnityWebRequest.Result.Success)
                {
                    EditorUtility.ClearProgressBar();
                    Logger.CreateError(nameof(UnityPackageManager), nameof(ImportFromUnityPackage),
                        "Download failed", Naming.Quote(request.error), Naming.Quote(unityPackageUrl));
                    return;
                }

                EditorUtility.ClearProgressBar();

                Logger.CreateText(nameof(UnityPackageManager), nameof(ImportFromUnityPackage),
                    "Opening import dialog", Naming.Quote(localFilePath));

                AssetDatabase.ImportPackage(localFilePath, true);
            }
            catch (Exception exception)
            {
                Logger.CreateError(nameof(UnityPackageManager), nameof(ImportFromUnityPackage), exception.Message);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        public static async Task ImportFromTarball(string tarballUrl, string packageName = null)
        {
            if (!string.IsNullOrEmpty(packageName) && IsPackageInstalled(packageName))
            {
                Logger.CreateText(nameof(UnityPackageManager), nameof(ImportFromTarball),
                    "Package already installed, skipping", Naming.Quote(packageName));
                return;
            }

            Logger.CreateText(nameof(UnityPackageManager), nameof(ImportFromTarball), "Downloading", Naming.Quote(tarballUrl));

            string projectPath = GetProjectPath();
            string externalPackagesPath = Path.Combine(projectPath, ExternalPackagesFolder);

            if (!Directory.Exists(externalPackagesPath))
            {
                Directory.CreateDirectory(externalPackagesPath);
            }

            string fileName = GetFileNameFromUrl(tarballUrl);
            string localFilePath = Path.Combine(externalPackagesPath, fileName).Replace('\\', '/');

            if (File.Exists(localFilePath))
            {
                File.Delete(localFilePath);
            }

            try
            {
                DownloadHandlerFile downloadHandler = new(localFilePath)
                {
                    removeFileOnAbort = true
                };

                using UnityWebRequest request = new(tarballUrl, UnityWebRequest.kHttpVerbGET)
                {
                    downloadHandler = downloadHandler,
                    redirectLimit = 10,
                    timeout = DownloadTimeoutSeconds
                };

                UnityWebRequestAsyncOperation operation = request.SendWebRequest();
                DateTime timeoutTime = DateTime.UtcNow.AddSeconds(DownloadTimeoutSeconds);

                while (!operation.isDone && DateTime.UtcNow < timeoutTime)
                {
                    float progress = request.downloadProgress;
                    if (EditorUtility.DisplayCancelableProgressBar(
                        "Importing Package",
                        $"Downloading {fileName}... {progress * 100f:F0}%",
                        progress * 0.7f))
                    {
                        request.Abort();
                        EditorUtility.ClearProgressBar();
                        Logger.CreateWarning(nameof(UnityPackageManager), nameof(ImportFromTarball), "Download cancelled by user");
                        return;
                    }
                    await Task.Delay(PollIntervalMs);
                }

                if (!operation.isDone)
                {
                    request.Abort();
                    EditorUtility.ClearProgressBar();
                    Logger.CreateError(nameof(UnityPackageManager), nameof(ImportFromTarball), "Download timed out", Naming.Quote(tarballUrl));
                    return;
                }

                if (request.result != UnityWebRequest.Result.Success)
                {
                    EditorUtility.ClearProgressBar();
                    Logger.CreateError(nameof(UnityPackageManager), nameof(ImportFromTarball),
                        "Download failed", Naming.Quote(request.error), Naming.Quote(tarballUrl));
                    return;
                }

                string packageIdentifier = $"file:{localFilePath}";
                Logger.CreateText(nameof(UnityPackageManager), nameof(ImportFromTarball),
                    "Adding package", Naming.Quote(packageIdentifier));

                await AddPackageWithProgress(packageIdentifier);
            }
            catch (Exception exception)
            {
                Logger.CreateError(nameof(UnityPackageManager), nameof(ImportFromTarball), exception.Message);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        public static async Task ImportFromGit(string gitUrl, string packageName = null)
        {
            if (!string.IsNullOrEmpty(packageName) && IsPackageInstalled(packageName))
            {
                Logger.CreateText(nameof(UnityPackageManager), nameof(ImportFromGit),
                    "Package already installed, skipping", Naming.Quote(packageName));
                return;
            }

            Logger.CreateText(nameof(UnityPackageManager), nameof(ImportFromGit),
                "Adding git package", Naming.Quote(gitUrl));

            try
            {
                await AddPackageWithProgress(gitUrl);
            }
            catch (Exception exception)
            {
                Logger.CreateError(nameof(UnityPackageManager), nameof(ImportFromGit), exception.Message);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        /// <summary>
        /// Installs or updates a git package together with everything its dependency list names:
        /// tarballs, git packages, scoped registries and registry packages. Script reloads stay locked
        /// until every step has run, so a half-installed state never compiles.
        /// </summary>
        public static async Task InstallWithDependencies(PackageDependencies dependencies, string packageGitUrl, string packageName, bool isUpdate)
        {
            LockReload();
            try
            {
                await InstallLegacyDependencies(dependencies);
                await SyncRegistryDependencies(dependencies);

                // An update passes no package name, so the add is not skipped as already installed.
                Logger.CreateText(nameof(UnityPackageManager), nameof(InstallWithDependencies),
                    isUpdate ? "Updating package" : "Installing package", Naming.Quote(packageGitUrl));
                await ImportFromGit(packageGitUrl, isUpdate ? null : packageName);
            }
            finally
            {
                UnlockReload();
            }
        }

        /// <summary>
        /// Installs the tarball and git entries of a dependency list, in that order.
        /// </summary>
        public static async Task InstallLegacyDependencies(PackageDependencies dependencies)
        {
            if (dependencies == null)
            {
                return;
            }
            if (dependencies.TarballUrls != null)
            {
                foreach (string tarballUrl in dependencies.TarballUrls)
                {
                    Logger.CreateText(nameof(UnityPackageManager), nameof(InstallLegacyDependencies),
                        "Installing tarball dependency", Naming.Quote(tarballUrl));
                    await ImportFromTarball(tarballUrl);
                }
            }
            if (dependencies.GitUrls != null)
            {
                foreach (string gitUrl in dependencies.GitUrls)
                {
                    Logger.CreateText(nameof(UnityPackageManager), nameof(InstallLegacyDependencies),
                        "Installing git dependency", Naming.Quote(gitUrl));
                    await ImportFromGit(gitUrl);
                }
            }
        }

        /// <summary>
        /// Writes the scoped registries and registry packages of a dependency list into Packages/manifest.json
        /// and resolves them. Optional packages are raised to their listed version when they are already
        /// installed, or all of them when <paramref name="includeOptional"/> is set.
        /// Returns true when the manifest changed.
        /// </summary>
        public static async Task<bool> SyncRegistryDependencies(PackageDependencies dependencies, bool includeOptional = false)
        {
            if (dependencies == null)
            {
                return false;
            }

            List<RegistryPackage> packages = new();
            if (dependencies.RegistryPackages != null)
            {
                packages.AddRange(dependencies.RegistryPackages);
            }
            if (dependencies.OptionalRegistryPackages != null)
            {
                foreach (OptionalRegistryPackage optionalPackage in dependencies.OptionalRegistryPackages)
                {
                    if (optionalPackage != null && (includeOptional || IsPackageInstalled(optionalPackage.Name)))
                    {
                        packages.Add(new RegistryPackage { Name = optionalPackage.Name, Version = optionalPackage.Version });
                    }
                }
            }

            return await ApplyRegistryPackages(dependencies.ScopedRegistries, packages.ToArray());
        }

        /// <summary>
        /// Installs one optional registry package together with the registries and required packages it relies on.
        /// Returns true when the manifest changed.
        /// </summary>
        public static async Task<bool> InstallRegistryPackage(PackageDependencies dependencies, OptionalRegistryPackage optionalPackage)
        {
            if (dependencies == null || optionalPackage == null)
            {
                return false;
            }

            List<RegistryPackage> packages = new();
            if (dependencies.RegistryPackages != null)
            {
                packages.AddRange(dependencies.RegistryPackages);
            }
            packages.Add(new RegistryPackage { Name = optionalPackage.Name, Version = optionalPackage.Version });

            return await ApplyRegistryPackages(dependencies.ScopedRegistries, packages.ToArray());
        }

        /// <summary>
        /// Removes a registry package from Packages/manifest.json and resolves. Returns true when it was listed.
        /// </summary>
        public static async Task<bool> RemoveRegistryPackage(string packageName)
        {
            try
            {
                string manifestPath = GetManifestPath();
                string manifestJson = File.ReadAllText(manifestPath);
                string updatedJson = PackageManifestEditor.Remove(manifestJson, packageName, out bool changed);
                if (!changed)
                {
                    Logger.CreateText(nameof(UnityPackageManager), nameof(RemoveRegistryPackage),
                        "Package is not listed in the manifest", Naming.Quote(packageName));
                    return false;
                }

                WriteManifest(manifestPath, updatedJson);
                Logger.CreateText(nameof(UnityPackageManager), nameof(RemoveRegistryPackage), "Removed", Naming.Quote(packageName));
                await ResolveWithProgress();
                return true;
            }
            catch (Exception exception)
            {
                Logger.CreateError(nameof(UnityPackageManager), nameof(RemoveRegistryPackage), exception.Message);
                return false;
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        private static async Task<bool> ApplyRegistryPackages(ScopedRegistry[] registries, RegistryPackage[] packages)
        {
            try
            {
                string manifestPath = GetManifestPath();
                string manifestJson = File.ReadAllText(manifestPath);

                foreach (string conflict in PackageManifestEditor.FindScopeConflicts(manifestJson, registries))
                {
                    Logger.CreateWarning(nameof(UnityPackageManager), nameof(SyncRegistryDependencies), conflict);
                }

                string updatedJson = PackageManifestEditor.Apply(manifestJson, registries, packages, out bool changed);
                if (!changed)
                {
                    Logger.CreateText(nameof(UnityPackageManager), nameof(SyncRegistryDependencies), "Registry packages are up to date");
                    return false;
                }

                foreach (RegistryPackage package in packages)
                {
                    string previous = PackageManifestEditor.GetDependency(manifestJson, package.Name);
                    string current = PackageManifestEditor.GetDependency(updatedJson, package.Name);
                    if (previous != current)
                    {
                        Logger.CreateText(nameof(UnityPackageManager), nameof(SyncRegistryDependencies),
                            Naming.Quote(package.Name), previous ?? "not installed", "->", current);
                    }
                }

                WriteManifest(manifestPath, updatedJson);
                await ResolveWithProgress();
                return true;
            }
            catch (Exception exception)
            {
                Logger.CreateError(nameof(UnityPackageManager), nameof(SyncRegistryDependencies), exception.Message);
                return false;
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        private static string GetManifestPath()
        {
            return Path.Combine(GetProjectPath(), "Packages", "manifest.json").Replace('\\', '/');
        }

        // The new text goes to a sibling file first, so a failed write never leaves a truncated manifest.
        private static void WriteManifest(string manifestPath, string manifestJson)
        {
            string temporaryPath = manifestPath + ".tmp";
            File.WriteAllText(temporaryPath, manifestJson, new UTF8Encoding(false));
            File.Copy(temporaryPath, manifestPath, true);
            File.Delete(temporaryPath);
        }

        private static async Task ResolveWithProgress()
        {
            const string title = "Importing Package";
            const string message = "Resolving registry packages...";
            EditorUtility.DisplayProgressBar(title, message, 0.5f);

            UnityEditor.PackageManager.Client.Resolve();

            // The list request is queued behind the resolve, so its completion marks the end of the resolve.
            UnityEditor.PackageManager.Requests.ListRequest listRequest = UnityEditor.PackageManager.Client.List(true, false);
            DateTime startTime = DateTime.UtcNow;
            DateTime timeoutTime = startTime.AddSeconds(UpmTimeoutSeconds);

            while (!listRequest.IsCompleted && DateTime.UtcNow < timeoutTime)
            {
                float elapsed = (float)(DateTime.UtcNow - startTime).TotalSeconds;
                EditorUtility.DisplayProgressBar(title, $"{message} ({elapsed:F0}s)", 0.5f + 0.49f * Math.Min(elapsed / UpmTimeoutSeconds, 1f));
                await Task.Delay(PollIntervalMs);
            }

            if (!listRequest.IsCompleted)
            {
                Logger.CreateWarning(nameof(UnityPackageManager), nameof(ResolveWithProgress),
                    "Package Manager resolve timed out, UPM will continue in background");
                return;
            }

            if (listRequest.Status == UnityEditor.PackageManager.StatusCode.Failure)
            {
                Logger.CreateError(nameof(UnityPackageManager), nameof(ResolveWithProgress),
                    "Failed to resolve packages", Naming.Quote(listRequest.Error?.message));
                return;
            }

            Logger.CreateText(nameof(UnityPackageManager), nameof(ResolveWithProgress), "Packages resolved");
        }

        private static async Task AddPackageWithProgress(string packageIdentifier)
        {
            EditorUtility.DisplayCancelableProgressBar("Importing Package",
                $"Adding {Naming.Quote(packageIdentifier)} to Unity Package Manager...", 0.75f);

            UnityEditor.PackageManager.Requests.AddRequest addRequest =
                UnityEditor.PackageManager.Client.Add(packageIdentifier);

            DateTime startTime = DateTime.UtcNow;
            DateTime timeoutTime = startTime.AddSeconds(UpmTimeoutSeconds);

            while (!addRequest.IsCompleted && DateTime.UtcNow < timeoutTime)
            {
                float elapsed = (float)(DateTime.UtcNow - startTime).TotalSeconds;
                float progress = 0.75f + 0.24f * Math.Min(elapsed / UpmTimeoutSeconds, 1f);
                if (EditorUtility.DisplayCancelableProgressBar("Importing Package",
                    $"Adding {Naming.Quote(packageIdentifier)} to Unity Package Manager... ({elapsed:F0}s)", progress))
                {
                    EditorUtility.ClearProgressBar();
                    Logger.CreateWarning(nameof(UnityPackageManager), nameof(AddPackageWithProgress),
                        "Cancelled by user, UPM will continue in background", Naming.Quote(packageIdentifier));
                    return;
                }
                await Task.Delay(PollIntervalMs);
            }

            if (!addRequest.IsCompleted)
            {
                EditorUtility.ClearProgressBar();
                Logger.CreateWarning(nameof(UnityPackageManager), nameof(AddPackageWithProgress),
                    "Package Manager request timed out, UPM will continue in background", Naming.Quote(packageIdentifier));
                return;
            }

            if (addRequest.Status == UnityEditor.PackageManager.StatusCode.Failure)
            {
                Logger.CreateError(nameof(UnityPackageManager), nameof(AddPackageWithProgress),
                    "Failed to add package", Naming.Quote(addRequest.Error?.message));
                return;
            }

            Logger.CreateText(nameof(UnityPackageManager), nameof(AddPackageWithProgress),
                "Successfully added package", Naming.Quote(packageIdentifier));
        }

        public static void LockReload()
        {
            EditorApplication.LockReloadAssemblies();
            AssetDatabase.DisallowAutoRefresh();
            Logger.CreateText(nameof(UnityPackageManager), nameof(LockReload), "Assembly reload locked");
        }

        public static void UnlockReload()
        {
            AssetDatabase.AllowAutoRefresh();
            EditorApplication.UnlockReloadAssemblies();
            AssetDatabase.Refresh();
            Logger.CreateText(nameof(UnityPackageManager), nameof(UnlockReload), "Assembly reload unlocked, refreshing");
        }

        public static bool IsPackageInstalled(string packageName)
        {
            return GetInstalledVersion(packageName) != null;
        }

        /// <summary>
        /// Returns the version of an installed package, or null when it is not installed.
        /// </summary>
        public static string GetInstalledVersion(string packageName)
        {
#if UNITY_2022_2_OR_NEWER
            UnityEditor.PackageManager.PackageInfo[] packages = UnityEditor.PackageManager.PackageInfo.GetAllRegisteredPackages();
            foreach (UnityEditor.PackageManager.PackageInfo package in packages)
            {
                if (package.name == packageName)
                {
                    return package.version;
                }
            }
            return null;
#else
            return UnityEditor.PackageManager.PackageInfo.FindForPackageName(packageName)?.version;
#endif
        }

        /// <summary>
        /// True when a required registry package is missing or older than listed, or an installed optional one is older than listed.
        /// </summary>
        public static bool NeedsRegistrySync(PackageDependencies dependencies)
        {
            if (dependencies == null)
            {
                return false;
            }
            if (dependencies.RegistryPackages != null)
            {
                foreach (RegistryPackage package in dependencies.RegistryPackages)
                {
                    string installedVersion = GetInstalledVersion(package.Name);
                    if (installedVersion == null || PackageManifestEditor.CompareVersions(installedVersion, package.Version) < 0)
                    {
                        return true;
                    }
                }
            }
            if (dependencies.OptionalRegistryPackages != null)
            {
                foreach (OptionalRegistryPackage package in dependencies.OptionalRegistryPackages)
                {
                    string installedVersion = GetInstalledVersion(package.Name);
                    if (installedVersion != null && PackageManifestEditor.CompareVersions(installedVersion, package.Version) < 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static string GetProjectPath()
        {
            string dataPath = Application.dataPath;
            int assetsIndex = dataPath.LastIndexOf("/Assets");
            if (assetsIndex >= 0)
            {
                return dataPath[..assetsIndex];
            }
            return dataPath;
        }

        private static string GetFileNameFromUrl(string url, string expectedExtension = ".tgz")
        {
            try
            {
                Uri uri = new(url);
                string fileName = Path.GetFileName(uri.LocalPath);
                if (!string.IsNullOrEmpty(fileName) && fileName.EndsWith(expectedExtension, StringComparison.OrdinalIgnoreCase))
                {
                    return fileName;
                }
            }
            catch
            {
                // Fall through to generate a default name
            }
            return $"package-{DateTime.UtcNow:yyyyMMddHHmmss}{expectedExtension}";
        }
    }
}
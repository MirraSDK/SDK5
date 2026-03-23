using MirraGames.SDK.Common;
using System;
using System.IO;
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

        public static async Task ImportFromTarball(string tarballUrl)
        {
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

                string packageIdentifier = $"file:{ExternalPackagesFolder}/{fileName}";
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

        public static async Task ImportFromGit(string gitUrl)
        {
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

        private static async Task AddPackageWithProgress(string packageIdentifier)
        {
            EditorUtility.DisplayProgressBar("Importing Package",
                $"Adding {Naming.Quote(packageIdentifier)} to Unity Package Manager...", 0.75f);

            UnityEditor.PackageManager.Requests.AddRequest addRequest =
                UnityEditor.PackageManager.Client.Add(packageIdentifier);

            DateTime startTime = DateTime.UtcNow;
            DateTime timeoutTime = startTime.AddSeconds(UpmTimeoutSeconds);

            while (!addRequest.IsCompleted && DateTime.UtcNow < timeoutTime)
            {
                float elapsed = (float)(DateTime.UtcNow - startTime).TotalSeconds;
                float progress = 0.75f + 0.24f * Math.Min(elapsed / UpmTimeoutSeconds, 1f);
                EditorUtility.DisplayProgressBar("Importing Package",
                    $"Adding {Naming.Quote(packageIdentifier)} to Unity Package Manager...", progress);
                await Task.Delay(PollIntervalMs);
            }

            if (!addRequest.IsCompleted)
            {
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

        private static string GetFileNameFromUrl(string url)
        {
            try
            {
                Uri uri = new(url);
                string fileName = Path.GetFileName(uri.LocalPath);
                if (!string.IsNullOrEmpty(fileName) && fileName.EndsWith(".tgz", StringComparison.OrdinalIgnoreCase))
                {
                    return fileName;
                }
            }
            catch
            {
                // Fall through to generate a default name
            }
            return $"package-{DateTime.UtcNow:yyyyMMddHHmmss}.tgz";
        }
    }
}
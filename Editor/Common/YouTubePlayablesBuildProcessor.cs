using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.IO;
using System.IO.Compression;
using System.Linq;
using UnityEngine;
using MirraGames.SDK.Common;

namespace MirraGames.SDK.Editor
{
    public class YouTubePlayablesBuildProcessor : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        public int callbackOrder => -1000;
        private const string YT_TEMPLATE_NAME = "PROJECT:MirraYoutubePlayablesTemplate";

        private bool IsYouTubeBuild()
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.WebGL) return false;
            PreferencesEditor preferencesEditor = PreferencesEditor.CreateEditor();
            string activeConfigName = preferencesEditor.GetBuildConfigurationName();
            bool isYouTubeConfig = activeConfigName == "YouTubePlayables" ||
                                  activeConfigName == "YouTubePlayablesConfiguration" ||
                                  activeConfigName.Contains("YouTubePlayables");

            return isYouTubeConfig;
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            if (!IsYouTubeBuild()) return;
            Debug.Log("<color=cyan>[YouTubePlayables] PRE-BUILD: Template found. Disabling compression...</color>");
            PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            Debug.Log($"<color=yellow>[YouTubePlayables] POST-BUILD triggered. Result: {report.summary.result}</color>");

            if (report.summary.result == BuildResult.Failed || report.summary.result == BuildResult.Cancelled)
            {
                Debug.Log($"<color=red>[YouTubePlayables] Build failed or cancelled. Skipping packaging.</color>");
                return;
            }

            if (!IsYouTubeBuild())
            {
                Debug.Log("<color=gray>[YouTubePlayables] This is not a YouTube build. Skipping.</color>");
                return;
            }

            Debug.Log("<color=green>[YouTubePlayables] Starting to package .data and .wasm into ZIP...</color>");
            ProcessBuildFolder(report.summary.outputPath);
        }

        public static void ProcessBuildFolder(string buildFolderPath)
        {
            string webBuildFolder = Path.Combine(buildFolderPath, "Build");

            if (!Directory.Exists(webBuildFolder))
            {
                if (Directory.Exists(Path.Combine(buildFolderPath, "TemplateData")))
                    webBuildFolder = buildFolderPath;
                else
                {
                    Debug.LogError($"[YouTubePlayables] Build folder not found at path: {buildFolderPath}");
                    return;
                }
            }

            var filesToZip = Directory.GetFiles(webBuildFolder, "*.data")
                                      .Concat(Directory.GetFiles(webBuildFolder, "*.wasm"));

            int zippedCount = 0;
            foreach (var filePath in filesToZip)
            {
                string fileName = Path.GetFileName(filePath);
                string zipFilePath = Path.Combine(webBuildFolder, fileName + ".zip");

                if (File.Exists(zipFilePath)) File.Delete(zipFilePath);

                using (ZipArchive archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
                {
                    archive.CreateEntryFromFile(filePath, fileName, System.IO.Compression.CompressionLevel.Optimal);
                }

                File.Delete(filePath);
                zippedCount++;
                Debug.Log($"<color=white>[YouTubePlayables] Packaged: <b>{fileName}</b> -> <b>{Path.GetFileName(zipFilePath)}</b></color>");
            }

            if (zippedCount > 0)
                Debug.Log($"<color=green>[YouTubePlayables] SUCCESS: Packaged {zippedCount} files. Originals deleted.</color>");
            else
                Debug.LogWarning("[YouTubePlayables] WARNING: No .data or .wasm files found to package!");
        }
    }
}
using MirraGames.SDK.Common;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using Logger = MirraGames.SDK.Common.Logger;

namespace MirraGames.SDK.Editor
{
    internal partial class PackageManagerView : VisualElement
    {
        public PackageManagerView()
        {
            VisualTreeReference reference = VisualTreeReference.Load(nameof(PackageManagerView));
            VisualTreeAsset asset = reference.VisualTree;
            asset.CloneTree(this);
            style.flexGrow = 1;
            _ = InitializeView();
        }

        private async Task InitializeView()
        {
            await CreatePackageCard("MirraSDK/SDK5");
            await CreatePackageCard("MirraSDK/SDK5-AppLovin-API");
            await CreatePackageCard("MirraSDK/SDK5-RuStore-API");
            await CreatePackageCard("MirraSDK/SDK5-YandexMobileAds-API");
            await CreatePackageCard("MirraSDK/SDK5-Playgama-API");
        }

        private async Task CreatePackageCard(string repositoryHandle)
        {
            PackageInfo packageInfo = await GetPackageInfo(repositoryHandle);
            if (packageInfo == null)
            {
                Logger.CreateWarning(this, nameof(CreatePackageCard), "Unable to access repository", Naming.Quote(repositoryHandle));
                return;
            }
            HorizontalCard card = new()
            {
                HeaderText = $"{packageInfo.displayName} (version: {packageInfo.version})",
                DescriptionText = packageInfo.name,
                LetterText = "M",
                HintText = ""
            };
            contentContainer.Add(card);
        }

        private string GetPackageJsonUrl(string repositoryHandle)
        {
            return $"https://raw.githubusercontent.com/{repositoryHandle}/refs/heads/main/package.json";
        }

        private async Task<PackageInfo> GetPackageInfo(string repositoryHandle)
        {
            string packageJsonUrl = GetPackageJsonUrl(repositoryHandle);
            Logger.CreateText(this, nameof(GetPackageInfo), Naming.Quote(packageJsonUrl));
            using UnityWebRequest webRequest = UnityWebRequest.Get(packageJsonUrl);
            UnityWebRequestAsyncOperation asyncOperation = webRequest.SendWebRequest();
            DateTime timeoutDateTime = DateTime.UtcNow.AddSeconds(10);
            while (!asyncOperation.isDone && DateTime.UtcNow < timeoutDateTime)
            {
                await Task.Delay(100);
            }
            if (!asyncOperation.isDone)
            {
                Logger.CreateWarning(this, nameof(GetPackageInfo), "Request timed out", Naming.Quote(packageJsonUrl));
                webRequest.Abort();
                return null;
            }
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Logger.CreateWarning(this, nameof(GetPackageInfo), "Request failed", Naming.Quote(webRequest.error), Naming.Quote(packageJsonUrl));
                return null;
            }
            try
            {
                string jsonResponse = webRequest.downloadHandler.text;
                PackageInfo packageInfo = JsonUtility.FromJson<PackageInfo>(jsonResponse);
                // Logger.CreateText(this, jsonResponse);
                return packageInfo;
            }
            catch (Exception exception)
            {
                Logger.CreateError(this, nameof(GetPackageInfo), exception, Naming.Quote(webRequest.downloadHandler.text));
                return null;
            }
        }

        private new VisualElement contentContainer
        {
            get => this.Q<VisualElement>(nameof(contentContainer));
        }
    }
}
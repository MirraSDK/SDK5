using MirraGames.SDK.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using Logger = MirraGames.SDK.Common.Logger;

namespace MirraGames.SDK.Editor
{
    internal partial class PackageManagerView : VisualElement
    {
        public class PackageCardInfo
        {
            public PackageInfo Info;
            public Texture2D Icon;
            public string Readme;
        }

        private readonly PackageManagerInspector PackageManagerInspector;
        private readonly Dictionary<HorizontalCard, PackageCardInfo> PackageCards = new();

        public PackageManagerView(PackageManagerInspector packageManagerInspector)
        {
            PackageManagerInspector = packageManagerInspector;
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

            bool isPackageInstalled = IsPackageInstalled(packageInfo.name);
            string localPackageVersion = GetLocalPackageVersion(packageInfo.name);
            HorizontalCard card = new()
            {
                HeaderText = $"{packageInfo.displayName}",
                DescriptionText = packageInfo.name,
                LetterText = packageInfo.displayName[..1].ToUpper(),
                HintText = isPackageInstalled ? $"Available: {packageInfo.version}\nInstalled: {localPackageVersion}" : $"Available: {packageInfo.version}\nNot installed"
            };
            PackageCardInfo cardInfo = new()
            {
                Info = packageInfo
            };
            PackageCards.Add(card, cardInfo);
            contentContainer.Add(card);

            Task<Texture2D> packageIcon = GetPackagePng(repositoryHandle);
            _ = packageIcon.ContinueWith(task =>
            {
                if (task.Result != null)
                {
                    cardInfo.Icon = task.Result;
                    card.SetIcon(task.Result);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());

            Task<string> packageReadme = GetPackageReadme(repositoryHandle);
            _ = packageReadme.ContinueWith(task =>
            {
                if (task.Result != null)
                {
                    cardInfo.Readme = task.Result;
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());

            card.RegisterCallback<ClickEvent>(callback =>
            {
                DeselectCards();
                card.Select();
                string description = PackageCards[card].Info.description;
                if (string.IsNullOrEmpty(description))
                {
                    description = Naming.Dash;
                }
                string readme = PackageCards[card].Readme;
                if (string.IsNullOrEmpty(readme))
                {
                    readme = Naming.Dash;
                }
                PackageManagerInspector.DescriptionLabel.text = description;
                PackageManagerInspector.ReadmeLabel.text = readme;
            });
        }

        private void DeselectCards()
        {
            foreach (HorizontalCard card in PackageCards.Keys)
            {
                card.Deselect();
            }
        }

        private bool IsPackageInstalled(string packageName)
        {
            return UnityEditor.PackageManager.PackageInfo.FindForPackageName(packageName) != null;
        }

        private string GetLocalPackageVersion(string packageName)
        {
            UnityEditor.PackageManager.PackageInfo packageInfo = UnityEditor.PackageManager.PackageInfo.FindForPackageName(packageName);
            return packageInfo?.version;
        }

        private string GetGitHubRepositoryUrl(string repositoryHandle)
        {
            return $"https://github.com/{repositoryHandle}";
        }

        private string GetPackageJsonUrl(string repositoryHandle)
        {
            return $"https://raw.githubusercontent.com/{repositoryHandle}/refs/heads/main/package.json";
        }

        private string GetPackagePngUrl(string repositoryHandle)
        {
            return $"https://raw.githubusercontent.com/{repositoryHandle}/refs/heads/main/package.png";
        }

        private string GetPackageReadmeUrl(string repositoryHandle)
        {
            return $"https://raw.githubusercontent.com/{repositoryHandle}/refs/heads/main/README.md";
        }

        private async Task<string> GetPackageReadme(string repositoryHandle)
        {
            string readmeUrl = GetPackageReadmeUrl(repositoryHandle);
            byte[] data = await Get(readmeUrl);
            if (data == null)
            {
                return null;
            }
            return Encoding.UTF8.GetString(data);
        }

        private async Task<Texture2D> GetPackagePng(string repositoryHandle)
        {
            string packagePngUrl = GetPackagePngUrl(repositoryHandle);
            byte[] data = await Get(packagePngUrl);
            if (data == null)
            {
                return null;
            }
            Texture2D texture = new(2, 2, TextureFormat.RGBA32, false);
            texture.LoadImage(data, true);
            return texture;
        }

        private async Task<PackageInfo> GetPackageInfo(string repositoryHandle)
        {
            string packageJsonUrl = GetPackageJsonUrl(repositoryHandle);
            byte[] data = await Get(packageJsonUrl);
            if(data == null)
            {
                return null;
            }
            try
            {
                string jsonResponse = Encoding.UTF8.GetString(data);
                PackageInfo packageInfo = JsonUtility.FromJson<PackageInfo>(jsonResponse);
                return packageInfo;
            }
            catch (Exception exception)
            {
                Logger.CreateError(this, nameof(GetPackageInfo), exception, Naming.Quote(Encoding.UTF8.GetString(data)));
                return null;
            }
        }

        private async Task<byte[]> Get(string url)
        {
            Logger.CreateText(this, nameof(Get), Naming.Quote(url));
            using UnityWebRequest webRequest = UnityWebRequest.Get(url);
            UnityWebRequestAsyncOperation asyncOperation = webRequest.SendWebRequest();
            DateTime timeoutDateTime = DateTime.UtcNow.AddSeconds(10);
            while (!asyncOperation.isDone && DateTime.UtcNow < timeoutDateTime)
            {
                await Task.Delay(100);
            }
            if (!asyncOperation.isDone)
            {
                Logger.CreateWarning(this, nameof(Get), "Request timed out", Naming.Quote(url));
                webRequest.Abort();
                return null;
            }
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Logger.CreateWarning(this, nameof(Get), "Request failed", Naming.Quote(webRequest.error), Naming.Quote(url));
                return null;
            }
            return webRequest.downloadHandler.data;
        }

        private new VisualElement contentContainer
        {
            get => this.Q<VisualElement>(nameof(contentContainer));
        }
    }
}
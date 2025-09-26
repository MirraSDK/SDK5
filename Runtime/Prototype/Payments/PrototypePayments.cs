using MirraGames.SDK.Common;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = MirraGames.SDK.Common.Logger;

namespace MirraGames.SDK.Prototype {

    [Provider(typeof(IPayments))]
    public class PrototypePayments : CommonPayments {

        private readonly UIDocument document;
        private readonly VisualElement shadowElement;
        private readonly VisualElement purchaseElement;
        private readonly Label productTagLabel;

        private readonly HashSet<string> shadowSources = new();
        private Action purchaseSuccessCallback;
        private Action purchaseCloseCallback;
        private string productTag;
        private bool isPurchaseVisible = false;

        public PrototypePayments(IData data) : base(data) {
            GameObject prototypeDocumentPrefab = PrefabReference.Load("PrototypeDocument").Prefab;
            GameObject prototypeDocumentObject = GameObject.Instantiate(prototypeDocumentPrefab);
            prototypeDocumentObject.name = nameof(PrototypePayments);
            GameObject.DontDestroyOnLoad(prototypeDocumentObject);
            document = prototypeDocumentObject.GetComponent<UIDocument>();

            VisualTreeAsset prototypePaymentsAsset = VisualTreeReference.Load(nameof(PrototypePayments)).VisualTree;
            VisualElement prototypePaymentsElement = prototypePaymentsAsset.Instantiate();
            prototypePaymentsElement.style.flexGrow = 1;

            shadowElement = prototypePaymentsElement.Q<VisualElement>(Naming.Shadow);
            purchaseElement = prototypePaymentsElement.Q<VisualElement>(Naming.Purchase);
            productTagLabel = purchaseElement.Q<Label>(Naming.ProductTag);

            shadowElement.Hide();
            purchaseElement.Hide();

            document.rootVisualElement.pickingMode = PickingMode.Ignore;
            prototypePaymentsElement.pickingMode = PickingMode.Ignore;

            document.rootVisualElement.Add(prototypePaymentsElement);
            document.sortingOrder = 1000;

            purchaseElement.Q<Button>(Naming.SuccessWithSupply).RegisterCallback<ClickEvent>(clickEvent => {
                RegisterPurchase(productTag);
                RegisterShadowSource(Naming.Purchase, false);
                purchaseSuccessCallback?.Invoke();
                purchaseElement.Hide();
                isPurchaseVisible = false;
            });
            purchaseElement.Q<Button>(Naming.SuccessWithoutSupply).RegisterCallback<ClickEvent>(clickEvent => {
                RegisterPurchase(productTag);
                RegisterShadowSource(Naming.Purchase, false);
                purchaseElement.Hide();
                isPurchaseVisible = false;
                Logger.CreateText(this, $"Restart play mode to restore {productTag}");
            });
            purchaseElement.Q<Button>(Naming.Close).RegisterCallback<ClickEvent>(clickEvent => {
                RegisterShadowSource(Naming.Purchase, false);
                purchaseCloseCallback?.Invoke();
                purchaseElement.Hide();
                isPurchaseVisible = false;
            });

            string json = PlayerPrefs.GetString(PurchasesKey);
            if (string.IsNullOrEmpty(json)) {
                Purchases = new string[0];
            }
            else {
                try {
                    PurchasesArray purchasesArray = JsonUtility.FromJson<PurchasesArray>(json);
                    Purchases = purchasesArray.Purchases ?? new string[0];
                }
                catch (Exception exception) {
                    Logger.CreateError(this, exception);
                    Purchases = new string[0];
                }
            }

            SetInitialized();
        }

        private string PurchasesKey {
            get => Naming.Key(Naming.MirraSDK5, nameof(PrototypePayments));
        }

        protected override ProductData GetProductDataImpl(string productTag) {
            return new ProductData(productTag, 1023.99f, "$MIR");
        }

        protected override bool IsAlreadyPurchasedImpl(string productTag) {
            return Array.Exists(Purchases, purchase => purchase == productTag);
        }

        protected override void PurchaseImpl(string productTag, Action onSuccess, Action onError = null) {
            if (isPurchaseVisible) {
                Logger.CreateWarning(this, "Purchase is already visible");
                onError?.Invoke();
                return;
            }
            isPurchaseVisible = true;
            this.productTag = productTag;
            productTagLabel.text = productTag;
            purchaseSuccessCallback = onSuccess;
            purchaseCloseCallback = onError;
            RegisterShadowSource(Naming.Purchase, true);
            purchaseElement.Show();
        }

        protected override void RestorePurchasesImpl(Action<IRestoreData> onRestoreData) {
            IRestoreData restoreData = new RestoreData(this, Purchases);
            onRestoreData?.Invoke(restoreData);
        }

        private void RegisterPurchase(string productTag) {
            string[] purchases = new string[Purchases.Length + 1];
            Array.Copy(Purchases, purchases, Purchases.Length);
            purchases[^1] = productTag;
            Purchases = purchases;
            PurchasesArray purchasesArray = new() { 
                Purchases = Purchases 
            };
            string json = JsonUtility.ToJson(purchasesArray);
            PlayerPrefs.SetString(PurchasesKey, json);
            PlayerPrefs.Save();
        }

        private void RegisterShadowSource(string source, bool isActive) {
            if (isActive) {
                shadowSources.Add(source);
            }
            else {
                shadowSources.Remove(source);
            }
            if (shadowSources.Count > 0) {
                shadowElement.style.display = DisplayStyle.Flex;
            }
            else {
                shadowElement.style.display = DisplayStyle.None;
            }
        }

    }

}
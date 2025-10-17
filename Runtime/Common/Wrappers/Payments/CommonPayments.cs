using System;

namespace MirraGames.SDK.Common {

    [Awaitable, Wrapper]
    public abstract partial class CommonPayments : IPayments {

        protected readonly IData data;
        protected string[] Purchases { get; set; }
        protected virtual ProductData DefaultProduct { get; }

        public CommonPayments(IData data) {
            this.data = data;
        }

        protected abstract ProductData GetProductDataImpl(string productTag);
        protected abstract bool IsAlreadyPurchasedImpl(string productTag);
        protected abstract void PurchaseImpl(string productTag, Action onSuccess, Action onError = null);
        protected abstract void RestorePurchasesImpl(Action<IRestoreData> onRestoreData);

        public ProductData GetProductData(string productTag) {
            Logger.CreateText(this, nameof(GetProductData), productTag);
            try {
                //if (IsPaymentsAvailable == false) {
                //    Logger.NotAvailableWarning(this, nameof(GetProductData));
                //    return default;
                //}
                return GetProductDataImpl(productTag);
            }
            catch (Exception exception) {
                Logger.CreateError(this, nameof(GetProductData), exception);
                return DefaultProduct;
            }
        }

        public bool IsAlreadyPurchased(string productTag) {
            Logger.CreateText(this, nameof(IsAlreadyPurchased), productTag);
            try {
                //if (IsPaymentsAvailable == false) {
                //    Logger.NotAvailableWarning(this, nameof(IsAlreadyPurchased));
                //    return false;
                //}
                return IsAlreadyPurchasedImpl(productTag);
            }
            catch (Exception exception) {
                Logger.CreateError(this, nameof(IsAlreadyPurchased), exception);
                return false;
            }
        }

        public void Purchase(string productTag, Action onSuccess, Action onError = null) {
            Logger.CreateText(this, nameof(Purchase), productTag);
            try {
                //if (IsPaymentsAvailable == false) {
                //    Logger.NotAvailableWarning(this, nameof(Purchase));
                //    return;
                //}
                void onSuccessCallback() {
                    Logger.CreateText(this, nameof(onSuccessCallback), productTag);
                    onSuccess?.Invoke();
                }
                void onErrorCallback() {
                    Logger.CreateText(this, nameof(onErrorCallback), productTag);
                    onError?.Invoke();
                }
                PurchaseImpl(productTag, onSuccessCallback, onErrorCallback);
            }
            catch (Exception exception) {
                Logger.CreateError(this, nameof(Purchase), exception);
                onError?.Invoke();
            }
        }

        public void RestorePurchases(Action<IRestoreData> onRestoreData) {
            Logger.CreateText(this, nameof(RestorePurchases));
            try {
                //if (IsPaymentsAvailable == false) {
                //    Logger.NotAvailableWarning(this, nameof(RestorePurchases));
                //    return;
                //}
                void onRestoreDataCallback(IRestoreData restoreData) {
                    Logger.CreateText(this, nameof(onRestoreDataCallback));
                    onRestoreData?.Invoke(restoreData);
                }
                RestorePurchasesImpl(onRestoreDataCallback);
            }
            catch (Exception exception) {
                Logger.CreateError(this, nameof(RestorePurchases), exception);
                onRestoreData?.Invoke(null);
            }
        }

        public string GetSupplyId(string productTag) {
            return $"consume-count-{productTag}";
        }

        public int GetSupplyCount(string productTag) {
            string supplyId = GetSupplyId(productTag);
            return data.GetInt(supplyId);
        }

        public void SetSupplyCount(string productTag, int supplyCount) {
            string supplyId = GetSupplyId(productTag);
            data.SetInt(supplyId, supplyCount);
            data.Save();
        }

        public void SupplyProduct(string productTag, Action productSupply, bool countProduct = true) {
            Logger.CreateText(this, nameof(SupplyProduct), productTag);
            productSupply?.Invoke();
            if (countProduct) {
                int supplyCount = GetSupplyCount(productTag);
                SetSupplyCount(productTag, supplyCount + 1);
            }
        }

    }

}
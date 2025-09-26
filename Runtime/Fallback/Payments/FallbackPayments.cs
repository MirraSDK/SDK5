using MirraGames.SDK.Common;
using System;

namespace MirraGames.SDK.Fallback {

    [Provider(typeof(IPayments))]
    public class FallbackPayments : CommonPayments {

        public FallbackPayments(IData data) : base(data) {
            SetInitialized();
        }

        protected override ProductData GetProductDataImpl(string productTag) {
            Logger.NotImplementedWarning(this, nameof(GetProductDataImpl));
            return DefaultProduct;
        }

        protected override bool IsAlreadyPurchasedImpl(string productTag) {
            Logger.NotImplementedWarning(this, nameof(IsAlreadyPurchasedImpl));
            return default;
        }

        protected override void PurchaseImpl(string productTag, Action onSuccess, Action onError = null) {
            Logger.NotImplementedWarning(this, nameof(PurchaseImpl));
            onError?.Invoke();
        }

        protected override void RestorePurchasesImpl(Action<IRestoreData> onRestoreData) {
            Logger.NotImplementedWarning(this, nameof(RestorePurchasesImpl));
            onRestoreData?.Invoke(default);
        }

    }

}
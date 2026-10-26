using MirraGames.SDK.Common;
using System;

namespace MirraGames.SDK.YouTubePlayables {

    /// <summary>
    /// YouTube Playables forbids off-platform purchases, so every purchase is refused.
    /// </summary>
    [Provider(typeof(IPayments))]
    public class YouTubePlayablesPayments : CommonPayments {

        public YouTubePlayablesPayments(IData data) : base(data) {
            SetInitialized();
        }

        protected override ProductData GetProductDataImpl(string productTag) {
            return DefaultProduct;
        }

        protected override bool IsAlreadyPurchasedImpl(string productTag) {
            return false;
        }

        protected override void PurchaseImpl(string productTag, Action onSuccess, Action onError = null) {
            onError?.Invoke();
        }

        protected override void RestorePurchasesImpl(Action<IRestoreData> onRestoreData) {
            onRestoreData?.Invoke(default);
        }

    }

}

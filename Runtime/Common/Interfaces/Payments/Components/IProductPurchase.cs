using System;

namespace MirraGames.SDK.Common {

    public interface IProductPurchase {

        ProductData GetProductData(string productTag);
        bool IsAlreadyPurchased(string productTag);
        void Purchase(string productTag, Action onSuccess, Action onError = null);

    }

}
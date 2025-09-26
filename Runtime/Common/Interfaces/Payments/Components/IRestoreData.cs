using System;

namespace MirraGames.SDK.Common {

    public interface IRestoreData {

        string[] AllPurchases { get; }
        string[] PendingProducts { get; }
        void RestoreProduct(string productTag, Action onProductRestore);

    }

}
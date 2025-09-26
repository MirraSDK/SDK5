using System;
using System.Collections.Generic;
using System.Linq;

namespace MirraGames.SDK.Common {

    public class RestoreData : IRestoreData {

        public RestoreData(IProductSupply productSupply, string[] purchases) {
            this.productSupply = productSupply;
            AllPurchases = purchases;
            List<string> suppliedProducts = new();
            List<string> pendingProducts = new();
            foreach (string product in purchases) {
                if (suppliedProducts.Contains(product) || pendingProducts.Contains(product)) {
                    continue;
                }
                int supplyCount = productSupply.GetSupplyCount(product);
                int purchasesCount = purchases.Count(p => p == product);
                if (supplyCount < purchasesCount) {
                    int missingCount = purchasesCount - supplyCount;
                    Logger.CreateText(this, $"product '{product}' not supplied '{missingCount}' times");
                    pendingProducts.Add(product);
                }
                else {
                    Logger.CreateText(this, $"product '{product}' already restored");
                    suppliedProducts.Add(product);
                }
            }
            PendingProducts = pendingProducts.ToArray();
        }

        private readonly IProductSupply productSupply;

        public string[] AllPurchases { get; }
        public string[] PendingProducts { get; }

        public void RestoreProduct(string productTag, Action onProductRestore) {
            int supplyCount = productSupply.GetSupplyCount(productTag);
            int purchasesCount = AllPurchases.Count(p => p == productTag);
            int missingCount = purchasesCount - supplyCount;
            if (missingCount < 0) {
                Logger.CreateError(this, $"something went really wrong: supply '{supplyCount}' purchases '{purchasesCount}'");
                return;
            }
            Logger.CreateText(this, $"restore product '{productTag}' by '{missingCount}' times");
            for (int x = 0; x < missingCount; x++) {
                productSupply.SupplyProduct(productTag, onProductRestore, false);
            }
            productSupply.SetSupplyCount(productTag, supplyCount + missingCount);
        }

    }

}
using MirraGames.SDK.SourceGenerator;
using MirraGames.SDK.Common;
using UnityEngine;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Features {

    public class ProductRestore_Features : FeaturesContainer {

        public new class UxmlFactory : UxmlFactory<ProductRestore_Features> { }

        public ProductRestore_Features() {
            SetInfo("Product Restore", nameof(IPayments), nameof(PaymentsProvider));

            CreateButton($"{nameof(IProductRestore.RestorePurchases)}", () => {
                MirraSDK.Payments.RestorePurchases((IRestoreData restoreData) => {

                    // Список всех покупок игрока (содержит и выданные и невыданные товары)
                    string[] allPurchases = restoreData.AllPurchases;
                    Debug.Log($"Игрок совершил '{allPurchases.Length}' успешных покупок");

                    // Список невыданных товаров, которые нужно выдать игроку (содержит уникальные теги товаров, т.е. если товар не выдан множество раз, он будет упомянут только один раз в массиве)
                    string[] pendingProducts = restoreData.PendingProducts;
                    Debug.Log($"Игрок не получил '{pendingProducts.Length}' разных товаров: [{string.Join(", ", pendingProducts)}]");

                    foreach (string productTag in pendingProducts) {

                        // Метод RestoreProduct выдает товар игроку, и регистрирует его как выданный
                        // Колбек onProductRestore будет выполнен столько раз, сколько раз данный товар не был выдан игроку
                        // От 0 раз до того сколько раз товар не был выдан игроку
                        restoreData.RestoreProduct(productTag, onProductRestore: () => {

                            // Ваш метод выдачи товара игроку
                            Debug.Log($"Товар '{productTag}' восстановлен");

                        });

                    }

                });
            });
        }

    }

}
using MirraGames.SDK.SourceGenerator;
using MirraGames.SDK.Common;
using UnityEngine;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Features {

    public class ProductPurchase_Features : FeaturesContainer {

        public new class UxmlFactory : UxmlFactory<ProductPurchase_Features> { }

        public ProductPurchase_Features() {
            SetInfo("Product Purchase", nameof(IPayments), nameof(PaymentsProvider));

            CreateButton(nameof(IProductPurchase.GetProductData), () => {
                Debug.Log(MirraSDK.Payments.GetProductData("customProduct"));
            });
            CreateButton(nameof(IProductPurchase.IsAlreadyPurchased), () => {
                Debug.Log(MirraSDK.Payments.IsAlreadyPurchased("customProduct"));
            });
            CreateButton($"{nameof(IProductPurchase.Purchase)} (callback)", () => {
                MirraSDK.Payments.Purchase("customProduct", () => {
                    Debug.Log("Product purchase success: 'customProduct'");
                }, () => {
                    Debug.LogError("Product purchase error: 'customProduct'");
                });
            });
        }

    }

}
using MirraGames.SDK.Common;
using MirraGames.SDK.SourceGenerator;
using UnityEngine;
using UnityEngine.UIElements;

namespace MirraGames.SDK.Features {

    public class ProductSupply_Features : FeaturesContainer {

        public new class UxmlFactory : UxmlFactory<ProductSupply_Features> { }

        public ProductSupply_Features() {
            SetInfo("Product Supply", nameof(IPayments), nameof(PaymentsProvider));

            CreateButton(nameof(IProductSupply.GetSupplyId), () => {
                Debug.Log(MirraSDK.Payments.GetSupplyId("customProduct"));
            });
            CreateButton(nameof(IProductSupply.GetSupplyCount), () => {
                Debug.Log(MirraSDK.Payments.GetSupplyCount("customProduct"));
            });
            CreateButton(nameof(IProductSupply.SetSupplyCount), () => {
                MirraSDK.Payments.SetSupplyCount("customProduct", 10);
            });
            CreateButton($"{nameof(IProductSupply.SupplyProduct)} (callback)", () => {
                MirraSDK.Payments.SupplyProduct("customProduct", () => {
                    Debug.Log("Product supplied.");
                }, true);
            });
        }

    }

}
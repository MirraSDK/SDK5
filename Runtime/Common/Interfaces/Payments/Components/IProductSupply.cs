using System;

namespace MirraGames.SDK.Common {

    public interface IProductSupply {

        string GetSupplyId(string productTag);
        int GetSupplyCount(string productTag);
        void SetSupplyCount(string productTag, int supplyCount);
        void SupplyProduct(string productTag, Action productSupply, bool countProduct = true);

    }

}
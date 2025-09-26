using System;

namespace MirraGames.SDK.Common {

    public interface IProductRestore {

        void RestorePurchases(Action<IRestoreData> onRestoreData);

    }

}
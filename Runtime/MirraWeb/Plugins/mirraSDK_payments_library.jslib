const mirraSDK_payments_library = {

    mirraSDK_payments_getProductPrice: function (productTagUTF8) {
        const productTag = UTF8ToString(productTagUTF8);
        return Module.mirraSDK.payments.getProductPrice(productTag);
    },

    mirraSDK_payments_getProductCurrency: function (productTagUTF8) {
        const productTag = UTF8ToString(productTagUTF8);
        const currency = Module.mirraSDK.payments.getProductCurrency(productTag);
        return Module.allocateString(currency);
    },

    mirraSDK_payments_isProductPurchased: function (productTagUTF8) {
        const productTag = UTF8ToString(productTagUTF8);
        return Module.mirraSDK.payments.isProductPurchased(productTag);
    },

    mirraSDK_payments_purchase: function (senderId, productTagUTF8, onSuccessPtr, onErrorPtr) {
        const productTag = UTF8ToString(productTagUTF8);
        const onSuccess = () => {
            Module.invokeMonoPCallback(senderId, onSuccessPtr);
        };
        const onError = () => {
            Module.invokeMonoPCallback(senderId, onErrorPtr);
        };
        Module.mirraSDK.payments.purchase(productTag, onSuccess, onError);
    },

    mirraSDK_payments_updatePurchases: function (senderId, onSuccessPtr) {
        (async () => {
            await Module.mirraSDK.payments.updatePurchases();
            const purchasesJson = JSON.stringify({
                purchases: Module.mirraSDK.payments.purchases
            });
            const purchasesJsonPtr = Module.allocateString(purchasesJson);
            Module.invokeMonoPCallback(senderId, onSuccessPtr, purchasesJsonPtr);
        })();
    },

};
mergeInto(LibraryManager.library, mirraSDK_payments_library);
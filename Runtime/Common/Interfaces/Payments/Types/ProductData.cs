namespace MirraGames.SDK.Common {

    public record ProductData {

        public ProductData(string tag, int priceInteger, string currency) {
            Tag = tag;
            PriceInteger = priceInteger;
            PriceFloat = priceInteger;
            Currency = currency;
        }

        public ProductData(string tag, float priceFloat, string currency) {
            Tag = tag;
            PriceInteger = (int)priceFloat;
            PriceFloat = priceFloat;
            Currency = currency;
        }

        public ProductData(string tag, int priceInteger, float priceFloat, string currency) {
            Tag = tag;
            PriceInteger = priceInteger;
            PriceFloat = priceFloat;
            Currency = currency;
        }

        public string Tag { get; }
        public int PriceInteger { get; }
        public float PriceFloat { get; }
        public string Currency { get; }

        public string GetFullPriceInteger(int addition = 0) {
            return $"{PriceInteger + addition} {Currency}";
        }

        public string GetFullPriceFloat(float addition = 0.0f) {
            return $"{PriceFloat + addition} {Currency}";
        }

    }

}
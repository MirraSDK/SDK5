namespace MirraGames.SDK.Common {

    public static class Naming {

        public const string PackageName = "com.romanlee17.mirrasdk5";

        public const string MirraSDK5 = "MirraSDK5";
        public const string Toolkit = "Toolkit";
        public const string EmptyJson = "{}";

        public const string Assets = "Assets";
        public const string Resources = "Resources";
        public const string Packages = "Packages";
        public const string Builds = "Builds";
        public const string Build = "Build";
        public const string Editor = "Editor";
        public const string Override = "Override";
        public const string Visible = "Visible";

        public const string AssetExtension = ".asset";

        public const string Fallback = "Fallback";
        public const string Name = "Name";

        public const string ContentContainer = "ContentContainer";
        public const string Shadow = "Shadow";
        public const string Interstitial = "Interstitial";
        public const string Rewarded = "Rewarded";

        public const string Close = "Close";
        public const string Success = "Success";
        public const string Purchase = "Purchase";

        public const string SuccessWithSupply = "SuccessWithSupply";
        public const string SuccessWithoutSupply = "SuccessWithoutSupply";
        public const string ProductTag = "ProductTag";

        public const string InternalDll = "__Internal";

        public static class USS {

            public const string ContentContainer = "content-container";
            public const string UnityCheckmark = "unity-checkmark";

        }

        public static string Key(params string[] subkeys) {
            return string.Join(".", subkeys);
        }

        public static string Key(params object[] subkeys) {
            return string.Join(".", subkeys);
        }

    }

}
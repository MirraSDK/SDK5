namespace MirraGames.SDK.Common {

    public readonly struct NavigationClickEvent {

        public NavigationClickEvent(string elementName) {
            ElementName = elementName;
        }

        public readonly string ElementName;

    }

}
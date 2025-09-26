namespace MirraGames.SDK.Common {

    public readonly struct PauseSourceEvent {

        public readonly string Source;
        public readonly bool Pause;

        public PauseSourceEvent(string source, bool pause) {
            Source = source;
            Pause = pause;
        }

    }

}
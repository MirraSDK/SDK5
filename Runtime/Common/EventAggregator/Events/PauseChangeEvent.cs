namespace MirraGames.SDK.Common {

    public readonly struct PauseChangeEvent {

        public readonly bool IsPaused;

        public PauseChangeEvent(bool isPaused) {
            IsPaused = isPaused;
        }

    }

}
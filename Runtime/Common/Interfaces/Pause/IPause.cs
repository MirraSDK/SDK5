using System;

namespace MirraGames.SDK.Common {

    [Module]
    public partial interface IPause {

        /// <summary>
        /// Whether any source, such as the platform or an ad, keeps the game paused.
        /// </summary>
        bool IsPaused { get; }

        void Register(object source, bool value);

        /// <summary>
        /// Calls the listener with the new state whenever the game becomes paused or unpaused.
        /// Platform pauses arrive synchronously, so the game can react before its main loop stops.
        /// </summary>
        void AddListener(Action<bool> onPauseChange);

        void RemoveListener(Action<bool> onPauseChange);

    }

}
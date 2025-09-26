namespace MirraGames.SDK.Common {

    public interface IEventListener<TEvent> {

        void OnEvent(TEvent eventData);

    }

}
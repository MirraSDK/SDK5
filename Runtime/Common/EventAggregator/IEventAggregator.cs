namespace MirraGames.SDK.Common {

    public interface IEventAggregator {

        void Publish<TEvent>(object publisher, TEvent eventData);
        void Subscribe<TEvent>(IEventListener<TEvent> eventListener);
        void Unsubscribe<TEvent>(IEventListener<TEvent> eventListener);
        void Dispose();

    }

}
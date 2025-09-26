using System;
using System.Collections.Generic;

namespace MirraGames.SDK.Common {

    public class EventAggregator : IEventAggregator, IDisposable {

        public EventAggregator() {
            lockObject = new();
            eventListenerCollection = new();
        }

        private readonly object lockObject;
        private readonly Dictionary<Type, EventListeners> eventListenerCollection;
        private readonly HashSet<object> eventStack = new();
        private int eventDepth = 0;

        private void RemoveMissingListeners() {
            foreach (EventListeners eventListeners in eventListenerCollection.Values) {
                eventListeners.RemoveAll(eventListener => eventListener == null);
            }
        }

        public void Publish<TEvent>(object publisher, TEvent eventData) {
            lock (lockObject) {
                Type eventType = typeof(TEvent);
                RemoveMissingListeners();
                eventDepth++;
                if (eventListenerCollection.ContainsKey(eventType)) {
                    EventListeners eventListeners = eventListenerCollection[eventType];
                    try {
                        if (eventStack.Contains(eventData)) {
                            throw new InvalidOperationException("Recursive event publishing detected.");
                        }
                        eventStack.Add(eventData);
                        foreach (object eventListener in eventListeners) {
                            eventDepth++;
                            try {
                                ((IEventListener<TEvent>)eventListener).OnEvent(eventData);
                            }
                            catch (Exception exception) {
                                Logger.CreateError(exception);
                            }
                            finally {
                                eventDepth--;
                            }
                        }
                    }
                    catch (Exception exception) {
                        Logger.CreateError(exception);
                    }
                    finally {
                        eventStack.Remove(eventData);
                    }
                }
                eventDepth--;
            }
        }

        public void Subscribe<TEvent>(IEventListener<TEvent> eventListener) {
            lock (lockObject) {
                RemoveMissingListeners();
                Type eventType = typeof(TEvent);
                if (eventListenerCollection.ContainsKey(eventType) == false) {
                    eventListenerCollection[eventType] = new();
                }
                eventListenerCollection[eventType].Add(eventListener);
            }
        }

        public void Unsubscribe<TEvent>(IEventListener<TEvent> eventListener) {
            lock (lockObject) {
                RemoveMissingListeners();
                Type eventType = typeof(TEvent);
                if (eventListenerCollection.ContainsKey(eventType)) {
                    eventListenerCollection[eventType].Remove(eventListener);
                }
            }
        }

        ~EventAggregator() {
            Dispose(false);
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (disposing == true) {
                eventListenerCollection.Clear();
            }
        }

    }

}
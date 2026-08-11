using System;

public interface IEventBus<FBase> where FBase : class
{
    void Subscribe<T>(Action<T> handler) where T : FBase;
    void Unsubscribe<T>(Action<T> handler) where T : FBase;
    void Publish<T>(T eventData) where T : FBase;
}
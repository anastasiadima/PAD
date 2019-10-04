using System;

namespace MessageBroker.Interfaces
{
     public interface IMessageHub: IDisposable
     {
          /// <summary>
          /// Registers a callback which is invoked on every message published by the <see cref="IMessageHub"/>.
          /// <remarks>Invoking this method with a new <paramref name="onMessage"/>overwrites the previous one.</remarks>
          /// </summary>
          /// <param name="onMessage">
          /// The callback to invoke on every message
          /// <remarks>The callback receives the type of the message and the message as arguments</remarks>
          /// </param>
          void RegisterGlobalHandler(Action<Type, object> onMessage);

          /// <summary>
          /// Publishes the <paramref name="message"/> on the <see cref="IMessageHub"/>.
          /// </summary>
          /// <param name="message">The message to published</param>
          void Publish<T>(T message);

          /// <summary>
          /// Subscribes a callback against the <see cref="IMessageHub"/> for a specific type of message.
          /// </summary>
          /// <typeparam name="T">The type of message to subscribe to</typeparam>
          /// <param name="action">The callback to be invoked once the message is published on the <see cref="IMessageHub"/></param>
          /// <returns>The token representing the subscription</returns>
          void Subscribe<T>(Action<T> action);

          /// <summary>
          /// Unsubscribes a subscription from the <see cref="IMessageHub"/>.
          /// </summary>
          /// <param name="id">The token representing the subscription</param>
          void Unsubscribe(int id);

          void Connect();
     }
}

using MessageBroker.Interfaces;
using System;
using System.Reflection;

namespace MessageBroker.Models
{
     public class MessageHub: IMessageHub
     {
          private readonly RoomSubscriber subscribers;
          private Action<Type, object> _globalHandler;

          public MessageHub()
          {
          }

          public void Connect()
          {
          }

          public void Dispose()
          { 
          }

          public void Publish()
          {

          }

          public void Publish<T>(T message)
          {
               var msgType = typeof(T);
               var msgTypeInfo = msgType.GetTypeInfo();

               _globalHandler?.Invoke(msgType, message);
          }

          public void RegisterGlobalHandler(Action<Type, object> onMessage)
          {
               _globalHandler = onMessage;
          }

          public void Subscribe()
          {

          }

          public void Subscribe<T>(Action<T> action)
          {
               throw new NotImplementedException();
          }

          public void Unsubscribe()
          {

          }

          public void Unsubscribe(int token)
          {
               //delete row with id
          }
     }
}

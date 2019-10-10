using MessageBroker.Interfaces;
using System;
using System.Net.Sockets;

namespace MessageBroker.Models
{
     public class RoomSubscriber
     {
          private readonly IMessageHub messageHub;

          public RoomSubscriber(IMessageHub messageHub)
          {
               this.messageHub = messageHub;
          }

          public void GetMessages()
          {
               // transmitem messajul la client
          }

          public void Unsubscribe(int id)
          {
               messageHub.Unsubscribe(id);
          }

          public void Subscribe(RoomType roomType, SocketModel socketModel)
          {
              //messageHub.Subscribe(roomType, socketModel);
          }
          
          public void GetSubscribedRooms(Tuple<int, string> tuple)
          {
               //this.messageHub.
          }
     }
}

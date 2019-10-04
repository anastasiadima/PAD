using MessageBroker.Interfaces;

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

          public void Subscribe(RoomType roomType)
          {
              // messageHub.Subscribe(roomType);
          }
     }
}

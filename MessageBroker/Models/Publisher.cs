using MessageBroker.Interfaces;

namespace MessageBroker
{
     public class Publisher
     {
          private readonly IMessageHub messageHub;

          public Publisher(IMessageHub messageHub)
          {
               this.messageHub = messageHub;
          }

          public void Publish( Message message)
          {
               messageHub.Publish(message);
          }
     }
}

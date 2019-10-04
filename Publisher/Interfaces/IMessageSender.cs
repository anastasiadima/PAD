using Publisher.Models;

namespace Publisher.Interfaces
{
     public interface IMessageSender
     {
          void SendMessage(Message message);
          void ReadMessage();
     }
}

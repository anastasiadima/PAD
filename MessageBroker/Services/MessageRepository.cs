using MessageBroker.Interfaces;

namespace MessageBroker.Services
{
     public class MessageRepository: EfRepository<Message, int>, IMessageRepository
     {
          public MessageRepository(DatabaseContext db): base(db) { }
     }
}

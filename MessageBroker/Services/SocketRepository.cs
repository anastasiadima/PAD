using MessageBroker.Interfaces;
using MessageBroker.Models;
using System.Linq;

namespace MessageBroker.Services
{
     public class SocketRepository: EfRepository<SocketModel, int>, ISocketRepository
     {
          public SocketRepository(DatabaseContext db): base(db) { }

          public int GetSocketId(int port,string address)
          {
               var model = Set.FirstOrDefault(s => s.IpAddress == address && s.Port == port);

               if (model != null)
               {
                    return model.Id;
               }
               return 0;
          }
     }
}

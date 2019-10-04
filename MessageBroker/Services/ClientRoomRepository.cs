using System.Collections.Generic;
using System.Linq;
using MessageBroker.Interfaces;

namespace MessageBroker.Services
{
     public class ClientRoomRepository:  EfRepository<ClientRoom, int>, IClientRoomRepository
     {
          public ClientRoomRepository(DatabaseContext db) : base(db) { }

          public IList<RoomType> GetRoomsForClientId(int clientId)
          {
               return Set.Where(x => x.SocketId == clientId).Select(x =>(RoomType) x.Room).ToList();
          }

          public int GetClientRoomId(ClientRoom clientRoom)
          {
               var model = Set.FirstOrDefault(x => x.Room == clientRoom.Room && x.SocketId == clientRoom.SocketId);
               if (model != null)
               {
                    return model.Id;
               }
               return 0;
          }

          public IList<int> GetSocketsIdListForRoomType(int room)
          {
               return Set.Where(x => x.Room == room).Select(x => x.SocketId).ToList();
          }
     }
}

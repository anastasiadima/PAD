using System;
using System.Collections.Generic;
using System.Text;

namespace MessageBroker.Interfaces
{
     public interface IClientRoomRepository: IRepository<ClientRoom, int>
     {
          IList<RoomType> GetRoomsForClientId(int clientId);
          int GetClientRoomId(ClientRoom clientRoom);
          IList<int> GetSocketsIdListForRoomType(int room);
     }
}

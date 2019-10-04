using MessageBroker.Models;

namespace MessageBroker.Interfaces
{
     public interface ISocketRepository: IRepository<SocketModel, int>
     {
          int GetSocketId(int port, string address);
     }
}

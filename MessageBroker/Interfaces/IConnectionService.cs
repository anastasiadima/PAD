namespace MessageBroker.Interfaces
{
     public interface IConnectionService
     {
          void Connect();
          void SendMessage(string ipAddress, int port, string message);
     }
}

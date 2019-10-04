namespace Publisher.Interfaces
{
     public interface IConnectionService
     {
          void Connect();
          void SendMessage(string message);
     }
}

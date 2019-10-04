using System.ComponentModel.DataAnnotations;

namespace MessageBroker.Models
{
     public class SocketModel
     {
          [Key]
          public int Id { get; set; }
          public int Port { get; set; }
          public string IpAddress { get; set; }
     }
}

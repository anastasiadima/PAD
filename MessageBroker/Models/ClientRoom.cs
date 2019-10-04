using System.ComponentModel.DataAnnotations;

namespace MessageBroker
{
     public class ClientRoom
     { 
          [Key]
          public int Id { get; set; }
          public int SocketId { get; set; }
          public int Room { get; set; }
     }

     public enum RoomType
     {
          Sport = 3,
          Entertaiment = 1,
          Politic = 2,
          None = 0
     }
}

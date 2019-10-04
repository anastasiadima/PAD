using System;

namespace Publisher.Models
{
     public class Message
     {
          public int Id { get; set; } 
          public RoomType RoomType { get; set; }
          public DateTime SendDate { get; set; }
          public bool IsRedirect { get; set; }
          public string Text { get; set; }
     }

     public enum RoomType
     {
          Sport = 3,
          Entertaiment = 1,
          Politic = 2,
          None = 0
     }
}

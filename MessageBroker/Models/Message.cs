using System;
using System.ComponentModel.DataAnnotations;

namespace MessageBroker
{
     public class Message
     {
          [Key]
          public int Id { get; set; }
          public int RoomType { get; set; }
          public DateTime SendDate { get; set; }
          public string Text { get; set; }
          public bool IsRedirect { get; set; }
     }
}

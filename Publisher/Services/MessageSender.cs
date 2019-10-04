using Publisher.Interfaces;
using Publisher.Models;
using System;
using Newtonsoft.Json;

namespace Publisher.Services
{
     public class MessageSender : IMessageSender
     {
          private readonly IConnectionService connectionService;
          public MessageSender(IConnectionService connection)
          {
               this.connectionService = connection;
          }
          public void ReadMessage()
          {
               Read:
               Console.WriteLine("Rooms: \n 1. Entertaiment \n 2. Politic \n 3.Sport \n Enter room number:");
               var answer = Console.ReadLine();
               var message = new Message();

               if (answer == "1")
               {
                    message.RoomType = RoomType.Entertaiment;
               } else if (answer == "2")
               {
                    message.RoomType = RoomType.Politic;
               } else if (answer == "3")
               {
                    message.RoomType = RoomType.Sport;
               } else
               {
                    goto Read;
               }
               Console.WriteLine("Enter Message:");
               var messageText = Console.ReadLine();

               if (messageText != null && messageText.Length > 0)
               {
                    message.SendDate = DateTime.Now;
                    message.IsRedirect = false;
                    message.Text = messageText;
                    SendMessage(message);
               }
               goto Read;
          }

          public void SendMessage(Message message)
          {
               var jsonMessage = JsonConvert.SerializeObject(message);
               this.connectionService.SendMessage(jsonMessage);
          }
     }
}

using Publisher.Interfaces;
using Publisher.Services;
using System;

namespace Publisher
{
     class Program
     {
          public static IMessageSender messageSender;
          private static IConnectionService connectionService;

          static void Main(string[] args)
          {
               connectionService = new TcpConnectionService("127.0.0.1", 5000);
               connectionService.Connect();
               Console.WriteLine("Publisher \n");
               messageSender = new MessageSender(connectionService);
               messageSender.ReadMessage();
               Console.ReadKey();
          }
     }
}

using MessageBroker.Interfaces;
using MessageBroker.Models;
using MessageBroker.Services;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MessageBroker
{
     class Program
     {
          static void Main(string[] args)
          {
               Console.WriteLine("Server starts--");
               DatabaseContext db= new DatabaseContext();
               IClientRoomRepository clientRoomRepository = new ClientRoomRepository(db);
               ISocketRepository  socketRepository = new SocketRepository(db);
               var tcp = new TcpConnectionService();
               Thread thread = new Thread(tcp.Connect);
               thread.Start();
          }
     }
}

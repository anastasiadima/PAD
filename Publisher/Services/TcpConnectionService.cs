using Publisher.Interfaces;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Publisher.Services
{
     public class TcpConnectionService : IConnectionService
     {
          private string IpAddress { get; set; }
          private int PortNumber { get; set; }
          private string Message { get; set; }
          private Socket Socket { get; set; }
          private static ManualResetEvent connectDone = new ManualResetEvent(false);

          public TcpConnectionService(string ipAddress, int port)
          {
               this.IpAddress = ipAddress;
               this.PortNumber = port;
          }

          public void Connect()
          {
               IPEndPoint remoteService = new IPEndPoint(IPAddress.Parse(IpAddress), PortNumber);
               Socket socket = new Socket(remoteService.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

               try
               {
                    socket.BeginConnect(remoteService,
               new AsyncCallback(ConnectCallback), socket);
                    connectDone.WaitOne();
                    Console.WriteLine("Connected..");
                    this.Socket = socket;
               }
               catch (Exception e)
               {
                    Console.WriteLine(e.Message);
               }
          }
          private static void ConnectCallback(IAsyncResult ar)
          {
               try
               {
                    // Retrieve the socket from the state object.  
                    Socket client = (Socket)ar.AsyncState;

                    // Complete the connection.  
                    client.EndConnect(ar);

                    Console.WriteLine("Socket connected to {0}",
                        client.RemoteEndPoint.ToString());

                    // Signal that the connection has been made.  
                    connectDone.Set();
               }
               catch (Exception e)
               {
                    Console.WriteLine(e.ToString());
               }
          }

          public void SendMessage(string message)
          {
               var buffer = new byte[1024];
               this.Message = message;
               message = "Publisher" + message;
               buffer = Encoding.ASCII.GetBytes(message);
               this.Socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), this.Socket);
          }

          private void SendCallback(IAsyncResult asyncResult)
          {
               Socket socket = (Socket)asyncResult.AsyncState;
               int byteSend = socket.EndSend(asyncResult);
               if(byteSend == 0)
               {
                    SendMessage(this.Message);
               }
          }
     }
}

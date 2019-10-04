using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Client
{
     class Program
     {
          public static ManualResetEvent done = new ManualResetEvent(false);
          public static byte[] Buffer = new byte[1024];
          public static string nickname = "";
          private static ManualResetEvent connectDone =new ManualResetEvent(false);
          static void Main(string[] args)
          {
               //Thread thread = new Thread(Connect);
               //thread.Start();

               Thread thread1 = new Thread(ChatService);
               thread1.Start();
          }

          public static void ChatService()
          {
               Console.WriteLine("Client program--");

               IPEndPoint remoteService = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);

               Socket socket = ConnectToServerTcp();
               if (socket.Connected)
               {
                    Console.WriteLine("You are subcribed to rooms:");
                    var buffer = Encoding.ASCII.GetBytes("GetRooms");
                    //socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);

                    var sendBytes = socket.Send(buffer);
                    var receivedBytes = 0;
                    if (sendBytes > 0)
                    {
                         buffer = new byte[1024];
                         receivedBytes = socket.Receive(buffer);
                    }
                    //socket.BeginReceive(Buffer, 0, Buffer.Length, 0, new AsyncCallback(ReadCallback), socket);
                    var result = true;
                    if (receivedBytes> 0)
                    {
                         Byte[] receiveBuffer = new byte[receivedBytes];
                         Array.Copy(buffer, receiveBuffer, receivedBytes);
                         string text = Encoding.ASCII.GetString(receiveBuffer);
                         Console.WriteLine(text);
                    }
                    while (result)
                    {
                         Console.WriteLine(" \n1. Subscribe \n2. Unsubscribe \n3.Get info\nEnter your choise:");
                         var answer = Console.ReadLine();
                         //result = false;

                         switch (answer)
                         {
                              case "1":
                                   Console.WriteLine("Subscribe to one of chanels: \n 1.Entertaiment \n 2. Politic \n 3. Sport");
                                   Console.WriteLine("Enter coresponding number:");
                                   string roomNr;
                                   roomNr = Console.ReadLine();

                                   if (int.TryParse(roomNr, out var number))
                                   {
                                        if (number == 1 || number == 2 || number == 3)
                                        {
                                             SendSubcribeMessage(number, socket);
                                        }
                                        else
                                        {
                                             Console.WriteLine("Number is invalid");
                                        }
                                   }
                                   else
                                   {
                                        Console.WriteLine("Input is invalid");
                                   }
                                   break;
                              case "2":
                                   Console.WriteLine("Subscribe to one of chanels: \n 1.Entertaiment \n 2. Politic \n 3. Sport");
                                   Console.WriteLine("Enter coresponding number:");
                                   string roomNr2;
                                   roomNr2 = Console.ReadLine();

                                   if (int.TryParse(roomNr2, out var number2))
                                   {
                                        if (number2 == 1 || number2 == 2 || number2 == 3)
                                        {
                                             SendUnSubcribeMessage(number2, socket);
                                        }
                                        else
                                        {
                                             Console.WriteLine("Number is invalid");
                                        }
                                   }
                                   else
                                   {
                                        Console.WriteLine("Input is invalid");
                                   }
                                   break;
                              case "3":
                                   GetInfo(socket);
                                   socket.BeginReceive(Buffer, 0, Buffer.Length, 0, new AsyncCallback(ReadCallback), socket);
                                   break;
                              default:
                                   result = true;
                                   break;
                         }
                    }
                   
                    try
                    {
                         done.Reset();
                         socket.BeginReceive(Buffer, 0, Buffer.Length, 0, new AsyncCallback(ReadCallback), socket);

                         //socket.Shutdown(SocketShutdown.Both);
                         //socket.Close();
                    }
                    catch { }
               }
          }

          private static void GetInfo(Socket socket)
          {
               var buffer = Encoding.ASCII.GetBytes("GetInfo");
               socket.Send(buffer);
               //socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
          }

          private static void SendUnSubcribeMessage(int roomNumber, Socket socket)
          {
               var buffer = Encoding.ASCII.GetBytes("UnSubscribe" + roomNumber.ToString());
               socket.Send(buffer);
               //socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
          }

          private static void SendSubcribeMessage(int roomNumber, Socket socket)
          {
               var buffer = Encoding.ASCII.GetBytes("Subscribe" + roomNumber.ToString());
               socket.Send(buffer);
               //socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
          }

          public static void SendCallback(IAsyncResult ar)
          {
               Socket socket = (Socket)ar.AsyncState;
               int byteSend = socket.EndSend(ar);
               socket.BeginReceive(Buffer, 0, Buffer.Length, 0, new AsyncCallback(ReadCallback), socket);
          }

          public static void ReadCallback(IAsyncResult asyncResult)
          {
               Socket handler = (Socket)asyncResult.AsyncState;
               //prelucram mesajele venite ca rs

               int bytesToRead = handler.EndReceive(asyncResult);
               Byte[] receiveBuffer = new byte[bytesToRead];
               Array.Copy(Buffer, receiveBuffer, bytesToRead);
               string text = Encoding.ASCII.GetString(receiveBuffer);
               Buffer = new byte[1024];

               Console.Write("---");
               Console.SetCursorPosition(0, Console.CursorTop);
               ClearCurrentConsoleLine();
               Console.WriteLine(text);
               handler.BeginReceive(Buffer, 0, Buffer.Length, 0, new AsyncCallback(ReadCallback), handler);
               //ReadMessage(handler);
          }

          //public static void Subscr

          public static void ReadMessage(Socket socket)
          {
               Console.Write("-->");

               var cmd = Console.ReadLine();
               if (!string.IsNullOrEmpty(cmd))
               {
                    var buffer = Encoding.ASCII.GetBytes(nickname + " : " + cmd);
                    socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
               }

               ReadMessage(socket);
          }


          public static void ClearCurrentConsoleLine()
          {
               int currentLineCursor = Console.CursorTop;
               Console.SetCursorPosition(0, Console.CursorTop);
               Console.Write(new string(' ', Console.WindowWidth));
               Console.SetCursorPosition(0, currentLineCursor);
          }

          public static Socket ConnectToServerTcp()
          {
               IPEndPoint remoteService = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);
               Socket socket = new Socket(remoteService.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

               try
               {
                    Console.WriteLine("Trying to connect..");
                    socket.BeginConnect(remoteService,
                new AsyncCallback(ConnectCallback), socket);
                    connectDone.WaitOne();
                    Console.WriteLine("Connected..");
               }
               catch (Exception e)
               {
                    Console.WriteLine(e.Message);
               }

               return socket;
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
     }
}
